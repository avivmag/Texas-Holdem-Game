using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Security.Cryptography;
using Backend.User;

namespace Database 
{
    public class DBImpl : IDB
    {
        int SALT_SIZE = 16;
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter adapter;
        //private MD5 md5Hash;

        //private List<SystemUser> userList;

        //private string GetMd5Hash(string input)
        //{
        //    // Convert the input string to a byte array and compute the hash.
        //    byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        //    // Create a new Stringbuilder to collect the bytes
        //    // and create a string.
        //    StringBuilder sBuilder = new StringBuilder();

        //    // Loop through each byte of the hashed data 
        //    // and format each one as a hexadecimal string.
        //    for (int i = 0; i < data.Length; i++)
        //    {
        //        sBuilder.Append(data[i].ToString("x2"));
        //    }

        //    // Return the hexadecimal string.
        //    return sBuilder.ToString();
        //}
        //// Verify a hash against a string.
        //private bool VerifyMd5Hash(string input, string hash)
        //{
        //    // Hash the input.
        //    string hashOfInput = GetMd5Hash(input);

        //    // Create a StringComparer an compare the hashes.
        //    StringComparer comparer = StringComparer.OrdinalIgnoreCase;

        //    if (0 == comparer.Compare(hashOfInput, hash))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //private string getRandomSalt()
        //{
        //    Random rnd = new Random();
        //    char[] saltArr = new char[SALT_SIZE];
        //    for (int i = 0; i < saltArr.Length; i++)
        //        saltArr[i] = (char)rnd.Next(0, 256);

        //    return new string(saltArr);
        //}

        private byte[] getRandomSalt()
        {
            var salt = new byte[SALT_SIZE];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }
            return salt; 
        }

        public DBImpl(){
            //var connectionString = ConfigurationManager.ConnectionStrings["TablesConnectionString"].ConnectionString;
            this.connectionString = Properties.Settings.Default.TablesConnectionString;
            //md5Hash = MD5.Create();
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

        /// <summary>
        /// check if user with that name exists
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true if the user with that name exists</returns>
        public bool isUserExist(string name)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT Id FROM SystemUsers WHERE UserName = @name LIMIT 1";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@name", name);

            connection.Open();
            reader = cmd.ExecuteReader();
            bool ans = reader.HasRows;
            connection.Close();
            return ans;
        }
        /// <summary>
        /// Register a new user to the system.
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="image"></param>
        /// <returns>true if the user has been added</returns>
        public bool RegisterUser(string UserName, string password, string email, string image)
        {
            //password = GetMd5Hash(string.Concat(new string[] { password, salt }));
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            
            cmd.CommandText = "INSERT SystemUsers (UserName,password,email,image,salt) " +
                                    "VALUES (@UserName,HASHBYTES(\'MD5\', CONCAT(@password,@salt)),@email,@image,@salt)";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@image", image);
            cmd.Parameters.AddWithValue("@salt", getRandomSalt());

            connection.Open();
            bool ans = cmd.ExecuteNonQuery() > 0;
            connection.Close();
            return ans;
        }

        /// <summary>
        /// Edit user profile by ID, if you don't want to change some of the fields just put null there.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="UserName"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="image"></param>
        /// <param name="money"></param>
        /// <param name="rankToAdd"></param>
        /// <param name="playedAnotherGame"></param>
        /// <returns>true if user has been edited succesfully</returns>
        public bool EditUserById(int? Id, string UserName, string password, string email, string image, int? money, int? rankToAdd, bool playedAnotherGame)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            int psikCount = -1 +
            (UserName == null ? 0 : 1) +
            (password == null ? 0 : 1) +
            (email == null ? 0 : 1) +
            (image == null ? 0 : 1) +
            (money == null ? 0 : 1) +
            (rankToAdd == null ? 0 : 1) +
            (playedAnotherGame ? 1 : 0);

            cmd.CommandText = "Update SystemUsers SET " +
                (UserName == null ? "" : "UserName=@UserName" + (psikCount-- > 0 ? "," : "" )) +
                (password == null ? "" : "password=HASHBYTES(\'MD5\', CONCAT(@password,@salt)),salt=@salt" + (psikCount-- > 0 ? "," : "")) +
                (email == null ? "" : "email=@email" + (psikCount-- > 0 ? "," : "")) +
                (image == null ? "" : "image=@image" + (psikCount-- > 0 ? "," : "")) +
                (money == null ? "" : "money=@money" + (psikCount-- > 0 ? "," : "")) +
                (rankToAdd == null ? "" : "rank=(CASE WHEN rank+@rankToAdd > 0 THEN rank+@rankToAdd ELSE 0 END)" + (psikCount-- > 0 ? "," : "")) +
                (!playedAnotherGame ? "" : "gamesPlayed=gamesPlayed+1") +
                " WHERE Id=@Id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", Id);
            if (UserName != null) cmd.Parameters.AddWithValue("@UserName", UserName);
            if (password != null) cmd.Parameters.AddWithValue("@password", password);
            if (email != null) cmd.Parameters.AddWithValue("@email", email);
            if (image != null) cmd.Parameters.AddWithValue("@image", image);
            if (password != null) cmd.Parameters.AddWithValue("@salt", getRandomSalt());
            if (money != null) cmd.Parameters.AddWithValue("@money", money);
            if (rankToAdd != null) cmd.Parameters.AddWithValue("@rankToAdd", rankToAdd);

            connection.Open();
            bool ans = cmd.ExecuteNonQuery() > 0;
            connection.Close();
            return ans;
        }
        /// <summary>
        /// Login mechanism
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="password"></param>
        /// <returns>if success returns the id of the user, else returns -1</returns>
        public int Login(string UserName, string password)
        {
            //password = GetMd5Hash(string.Concat(new string[] { password, salt }));
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT Id FROM SystemUsers WHERE UserName=@UserName AND password=HASHBYTES(\'MD5\', CONCAT(@password,salt))";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@password", password);

            connection.Open();
            reader = cmd.ExecuteReader();
            if (!reader.HasRows || !reader.Read())
                return -1;

            int ans = (int)reader["Id"];
            connection.Close();
            return ans;

            //int ans = (int)reader["Id"];
            //string UserName2 = (string)reader["UserName"];
            //string password2 = (string)reader["password"];
            //string salt = (string)reader["salt"];
            //string email = (string)reader["email"];
            //string image = (string)reader["image"];
            //int money = (int)reader["money"];
            //int rank = (int)reader["rank"];

        }

        public List<SystemUser> getAllSystemUsers()
        {
            List<SystemUser> users = new List<SystemUser>();
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT Id,UserName,email,image,money,rank,gamesPlayed FROM SystemUsers";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;

            connection.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
                users.Add(new SystemUser(int.Parse(reader["Id"].ToString()), reader["UserName"].ToString(), reader["email"].ToString(), reader["image"].ToString(), int.Parse(reader["money"].ToString()), int.Parse(reader["rank"].ToString()), int.Parse(reader["gamesPlayed"].ToString())));
            
            connection.Close();
            return users;
        }
        public SystemUser getUserById(int Id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT UserName,email,image,money,rank,gamesPlayed FROM SystemUsers WHERE Id=@Id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", Id);

            connection.Open();
            reader = cmd.ExecuteReader();
            if (!reader.HasRows || !reader.Read())
                return null;
            SystemUser su = new SystemUser(Id, reader["UserName"].ToString(), reader["email"].ToString(), reader["image"].ToString(), int.Parse(reader["money"].ToString()), int.Parse(reader["rank"].ToString()), int.Parse(reader["gamesPlayed"].ToString()));

            connection.Close();
            return su;
        }
        public SystemUser getUserByName(string name)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT Id,email,image,money,rank,gamesPlayed FROM SystemUsers WHERE UserName=@UserName";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@UserName", name);

            connection.Open();

            reader = cmd.ExecuteReader();
            if (!reader.HasRows || !reader.Read())
                return null;
            //            string s = reader["email"].ToString();
            SystemUser su = new SystemUser(int.Parse(reader["Id"].ToString()), name, reader["email"].ToString(), reader["image"].ToString(), int.Parse(reader["money"].ToString()), int.Parse(reader["rank"].ToString()), int.Parse(reader["gamesPlayed"].ToString()));

            connection.Close();
            return su;
        }
        public SystemUser getUserByEmail(string email)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT Id,UserName,image,money,rank,gamesPlayed FROM SystemUsers WHERE email=@email";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@email", email);

            connection.Open();
            reader = cmd.ExecuteReader();
            if (!reader.HasRows || !reader.Read())
                return null;
            SystemUser su = new SystemUser(int.Parse(reader["Id"].ToString()), reader["UserName"].ToString(), email, reader["image"].ToString(), int.Parse(reader["money"].ToString()), int.Parse(reader["rank"].ToString()), int.Parse(reader["gamesPlayed"].ToString()));

            connection.Close();
            return su;
        }
        public bool deleteUser(int Id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DELETE FROM SystemUsers WHERE Id=@Id";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;
            cmd.Parameters.AddWithValue("@Id", Id);

            connection.Open();
            bool ans = cmd.ExecuteNonQuery() > 0;
            connection.Close();
            return ans;
        }
        public bool deleteUsers()
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "DELETE FROM SystemUsers";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = connection;

            connection.Open();
            bool ans = cmd.ExecuteNonQuery() > 0;
            connection.Close();
            return ans;
        }
        //public void editUserName(int userID, string userName)
        //{
        //    string queryUpdate = "UPDATE SystemUsers SET UserName = @UserName " +
        //                          "WHERE Id = @userID ";
        //    connection = new SqlConnection(connectionString);
        //    SqlCommand command = new SqlCommand(queryUpdate, connection);
        //    using (connection)
        //    using (command)
        //    {
        //        connection.Open();

        //        command.Parameters.AddWithValue("@userID", userID);
        //        command.Parameters.AddWithValue("@UserName", userName);

        //        command.ExecuteScalar();
        //    }

        //}

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
