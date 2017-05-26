using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace DB
{
    public class UploadData
    {
        string connectionString;
        SqlConnection connection;
        SqlDataAdapter adapter;

        public  UploadData()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ClassLibrary1.Properties.Settings.DatabaseConnectionString"].ConnectionString;
        }

        public void uploadSystemUser()
        {
            using (connection = new SqlConnection(connectionString))
            using (adapter = new SqlDataAdapter("SELECT * FROM SystemUser", connection))
            {
    
                DataTable systemUserTable = new DataTable();
                adapter.Fill(systemUserTable);
            }
                
        }



    }
}
