using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Database
{
    interface IDB
    {

        DataTable uploadSystemUser();
        string getEnterMessage(string stringCommand);
        void editUserName(int userID, string newData);

    }
}
