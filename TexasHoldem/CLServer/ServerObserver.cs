using Obser;
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
            CLImpl.SendMessage(clientInfo, new { response = "Game", obj });
        }
    }
}
