using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLServer
{
    public class ClientInfo
    {
        public enum CLIENT_TYPE { TCP, HTTP };
        public Object client { get; set; }
        public CLIENT_TYPE type { get; set; }

        public ClientInfo(Object client, CLIENT_TYPE type)
        {
            this.client = client;
            this.type = type;
        }
    }
}
