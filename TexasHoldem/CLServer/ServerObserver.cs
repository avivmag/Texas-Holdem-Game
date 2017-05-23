using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

using System.Net;

namespace CLServer
{
    public class ServerObserver : Obser.ObserverAbstract<TcpClient>
    {
        private TcpClient client;

        public ServerObserver(TcpClient c)
        {
            client = c;
        }

        public override void update()
        {
            Console.WriteLine("UPDATE " + client.ToString());
            CLImpl.SendMessage(client, new { response = "UpdatedGame" });
        }
    }
}
