using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CLClient.Entities;

namespace CLClient
{
    public static class CommClient
    {
        private static TcpClient client;

        #region Static functionality

        public static void startClientConnection()
        {
            client = new TcpClient("127.0.0.1", 2345);
        }

        public static JObject sendMessage(object obj)
        {

            var jsonObj             = JObject.FromObject(obj);
            var serializedJsonObj   = JsonConvert.SerializeObject(jsonObj);

            var networkStream = client.GetStream();

            if (networkStream.CanWrite)
            {
                var jsonObjArray = Encoding.ASCII.GetBytes(serializedJsonObj);

                networkStream.Write(jsonObjArray, 0, jsonObjArray.Length);
            }
                return getJsonObjectFromStream(client);
        }

        private static JObject getJsonObjectFromStream(TcpClient client)
        {
            var message = new byte[1024];

            var bytesRead = client.GetStream().Read(message, 0, 1024);

            string myObject = Encoding.ASCII.GetString(message);

            Object deserializedProduct = JsonConvert.DeserializeObject(myObject);

            Console.WriteLine(deserializedProduct);

            return JObject.FromObject(deserializedProduct);
        }

        #endregion

        #region PL Functions

        public static SystemUser Login(string username, string password)
        {
            var message = new { action = "Login", username = username, password = password };

            var responseJson = sendMessage(message);

            var response = responseJson.ToObject<SystemUser>();

            return response;
        }

        #endregion
    }
}
