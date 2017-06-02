using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace Database 
{
    class DBImpl : IDB
    {
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter adapter;
        //private List<SystemUser> userList;

        public DBImpl(){
            connectionString = ConfigurationManager.ConnectionStrings["Database.Properties.Settings.TablesConnectionString"].ConnectionString;
        }

        public DataTable uploadSystemUser()
        {
            DataTable systemUserTable = new DataTable();
            string queryTable = "SELECT * FROM SystemUsers";
            using (connection = new SqlConnection(connectionString))
            using (adapter = new SqlDataAdapter(queryTable, connection))
            {
                adapter.Fill(systemUserTable);

                //userList = systemUserTable;
            }

            return systemUserTable;
        }


        public void editUserName(int userID, string userName)
        {
            string queryUpdate = "UPDATE SystemUsers SET UserName = @UserName " +
                                  "WHERE Id = @userID ";
            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryUpdate, connection);
            using (connection)
            using (command)
            {
                connection.Open();

                command.Parameters.AddWithValue("@userID", userID);
                command.Parameters.AddWithValue("@UserName", userName);

                command.ExecuteScalar();
            }

        }

        public string getEnterMessage(string stringCommand)
        {
            string ans;
            DataTable messagesTableEnter = new DataTable();
            connection = new SqlConnection(connectionString);
            string queryMessage = "SELECT M.MessageEnter FROM MessageEnter M" +
                                    "WHERE M.command = @command ";
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


    }
}
