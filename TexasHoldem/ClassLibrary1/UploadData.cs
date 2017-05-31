using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary1;

namespace DB
{
    public class UploadData : IDB
    {
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter adapter;
        //private List<SystemUser> userList;

        public  UploadData()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ClassLibrary1.Properties.Settings.DatabaseConnectionString"].ConnectionString;
        }

        //or return void
        public DataTable uploadSystemUser()
        {
            DataTable systemUserTable = new DataTable();

            using (connection = new SqlConnection(connectionString))
            using (adapter = new SqlDataAdapter("SELECT * FROM SystemUser", connection))
            {
                adapter.Fill(systemUserTable);
                
                //userList = systemUserTable;
            }

            return systemUserTable;
        }

        public string getEnterMessage(string stringCommand)
        {
            string ans;
            DataTable messagesTableEnter = new DataTable();
            connection = new SqlConnection(connectionString);
            string queryMessage = "SELECT M.MessageEnter FROM MessagesEnter M" +
                                    "WHERE M.commandName = @command ";
            SqlCommand command = new SqlCommand(queryMessage, connection);
            adapter = new SqlDataAdapter(command);

            using (connection)
            using (command)
            using (adapter)
            {
                //connection.open();
                command.Parameters.AddWithValue("@command", stringCommand);
                //command.ExecuteNonQuery();
                adapter.Fill(messagesTableEnter);
                ans = messagesTableEnter.ToString();
            }
            //I think it should all presented in messageBox.Show
                return ans;
        }

        public void editUserName(int userID, string newData)
        {

            string queryUpdate = "UPDATE SystemUser SET UserName = @UserName " +
                                    "WHERE UserID = @userID ";
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryUpdate, connection);
            using (connection)
            using (command)
            {
                connection.Open();

                command.Parameters.AddWithValue("@userID", userID);
                command.Parameters.AddWithValue("@UserName", newData);

                command.ExecuteScalar();
            }


        }




    }
}
