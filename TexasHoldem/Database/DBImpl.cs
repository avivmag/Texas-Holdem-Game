using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Backend.User;

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

        public bool isUserExist(string name)
        {
            DataTable table = new DataTable();
            connection = new SqlConnection(connectionString);
            string queryMessage = "SELECT Id FROM SystemUsers" +
                                    "WHERE UserName = @name LIMIT 1";
            SqlCommand command = new SqlCommand(queryMessage, connection);
            adapter = new SqlDataAdapter(command);

            using (connection)
            using (command)
            using (adapter)
            {
                //connection.open();
                command.Parameters.AddWithValue("@name", name);
                //command.ExecuteNonQuery();
                adapter.Fill(table);
                return table.Columns.Count > 0;
            }
        }

        //TODO: Aviv - continue this after checking other stuff
        public void RegisterUser(int Id, string UserName, string password, string email, string image)
        {
            
            string queryUpdate = "INSERT INTO SystemUsers (Id,UserName,password,email,image,money,salt) " +
                                    "VALUES (@Id,@UserName,@password,@email,@image,@money,@salt)";

            connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryUpdate, connection);
            using (connection)
            using (command)
            {
                connection.Open();

                //command.Parameters.AddWithValue("@userID", userID);
                //command.Parameters.AddWithValue("@UserName", userName);

                command.ExecuteScalar();
            }

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
