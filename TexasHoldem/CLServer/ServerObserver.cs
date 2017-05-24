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
            CLImpl.SendMessage(client, new { response = "Game", obj });
        }
    }
}
