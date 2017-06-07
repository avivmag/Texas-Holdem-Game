using Obser;
using System;
using System.Net.Sockets;

namespace CLServer
{
    public class ServerObserver : ObserverAbstract<TcpClient>
    {
        private ClientInfo clientInfo;

        public ServerObserver(ClientInfo clientInfo)
        {
            this.clientInfo = clientInfo;
        }

        public override void update(object obj)
        {
            if (obj.GetType() == typeof(string))
            {
                Console.WriteLine("Updating observers about new game message.");
                CLImpl.SendMessage(clientInfo, new { response = "Message", obj });
            }
            else
            {
                Console.WriteLine("Updating observers update in game.");
                CLImpl.SendMessage(clientInfo, new { response = "Game", obj });
            }
        }
    }
}
