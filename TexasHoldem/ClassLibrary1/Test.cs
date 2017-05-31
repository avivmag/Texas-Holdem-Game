using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using ClassLibrary1;

namespace ClassLibrary1
{
    public partial class Test : Form
    {
        private string connectionString;
        SqlConnection connection;
        SqlDataAdapter adapter;

        public Test()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ClassLibrary1.Properties.Settings.DatabaseConnectionString"].ConnectionString;
        }

        private void Test_Load(object sender, EventArgs e)
        {

            DataTable systemUserTable = new DataTable();

            using (connection = new SqlConnection(connectionString))
            using (adapter = new SqlDataAdapter("SELECT * FROM SystemUser", connection))
            {
                adapter.Fill(systemUserTable);

                //userList = systemUserTable;
            }

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
                command.Parameters.AddWithValue("@command", stringCommand);
                adapter.Fill(messagesTableEnter);
                ans = messagesTableEnter.ToString();
            }
            //I think it should all presented in messageBox.Show
            return ans;
        }

        private void listSystemUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       
    }
}
