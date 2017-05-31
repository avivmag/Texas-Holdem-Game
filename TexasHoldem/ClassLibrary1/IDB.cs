using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public interface IDB
    {

        DataTable uploadSystemUser();
        string getEnterMessage(string stringCommand);
        
    }
}
