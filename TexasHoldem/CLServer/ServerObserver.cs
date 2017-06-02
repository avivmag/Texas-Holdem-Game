using Obser;
using System.Net.Sockets;

namespace CLServer
{
    public class ServerObserver : ObserverAbstract<TcpClient>
    {
        private TcpClient client;

        public ServerObserver(TcpClient c)
        {
            client = c;
        }

        public override void update(object obj)
        {
            var clientInfo = new ClientInfo(client, ClientInfo.CLIENT_TYPE.TCP);

            CLImpl.SendMessage(clientInfo, new { response = "Game", obj });
        }
    }
}
