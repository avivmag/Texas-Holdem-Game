using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SL;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.IO;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;

namespace CLServer
{
    public class CLImpl
    {
        private static SLInterface sl = null;

        /// <summary>
        /// Task to proccess the client's requests.
        /// </summary>
        /// <param name="obj">The tcp client.</param>
        private static void ProcessClientRequests(Object obj)
        {
            TcpClient client = (TcpClient)obj;

            try
            {
                while (true)
                {
                    var jsonObject = getJsonObjectFromStream(client);

                    tryExecuteAction(client, jsonObject);
                }
            }

            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                SendMessage(client, new { exception = "An Error Has Occured" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        /// <summary>
        /// Sends an exception message to the client.
        /// </summary>
        /// <param name="client">The client to send to.</param>
        /// <param name="message">The message to send. (Optional)</param>
        private static void SendMessage(TcpClient client, object message = null)
        {
            var exceptionString = JObject.FromObject(message);

            var serializedException = JsonConvert.SerializeObject(exceptionString);

            var exceptionByteArray = Encoding.ASCII.GetBytes(serializedException);

            try
            {
                if (client.GetStream().CanWrite)
                {
                    client.GetStream().Write(exceptionByteArray, 0, exceptionByteArray.Length);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        /// <summary>
        /// returns a JObject as data from the client stream.
        /// </summary>
        /// <param name="client">The tcp client</param>
        /// <returns>The JObject</returns>
        private static JObject getJsonObjectFromStream(TcpClient client)
        {
            var message = new byte[1024];

            var bytesRead = client.GetStream().Read(message, 0, 1024);

            string myObject = Encoding.ASCII.GetString(message);
            Object deserializedProduct = JsonConvert.DeserializeObject(myObject);
            return JObject.FromObject(deserializedProduct);
        }

        /// <summary>
        /// Tries to execute the action given by the client.
        /// </summary>
        /// <param name="jsonObject">The Object received from the client.</param>
        /// old stuff saved in case things will not work
        /*
private static void tryExecuteAction(TcpClient client, JObject jsonObject)
{
    var action = jsonObject.Value<string>("action");

    Console.WriteLine("Trying to execute action: {0}", action);

    switch (action)
    {
        case "Raise":
            // Get values from JSON.
            var gameId = jsonObject.Value<int?>("gameId");
            var playerId = jsonObject.Value<int?>("playedId");
            var coins = jsonObject.Value<int?>("coins");

            if (gameId == null || playerId == null || coins == null)
            {
                throw new ArgumentException("parameters mismatch.");
            }
            else
            {
                Console.WriteLine("Raising Bet. parameters are: gameId: {0}, playerId: {1}, coins: {2}", gameId, playerId, coins);
            }
            //sl.raiseBet(gameId, playerId, coins);
            break;

        case "Login":
            var username = jsonObject.Value<string>("username");
            var password = jsonObject.Value<string>("password");

            if (username == null || password == null)
            {
                throw new ArgumentException("Parameters Mismatch.");
            }

            var response = sl.Login(username, password);

            SendMessage(client, response);
            break;

        default:
            throw new ArgumentException("No known action specified.");
    }
}     
*/
        ///
        private static void tryExecuteAction(TcpClient client, JObject jsonObject)
        {
            var action = jsonObject.Value<string>("action");

            Console.WriteLine("Trying to execute action: {0}", action);


            // null means calling a static method
            object[] temp = new object[2];
            temp[0] = jsonObject;
            temp[1] = client;
            try
            {
                typeof(CLImpl).GetMethod(action).Invoke(null, temp);
            }
            catch(TargetException e)
            {
                // class specified not found
            }
        }

        private static void Raise(object[] args)
        {
            // this two lines must be in every method
            JObject jsonObject = (JObject)args[0];
            TcpClient client = (TcpClient)args[1];
            
            // Get values from JSON.
            var gameId = jsonObject.Value<int?>("gameId");
            var playerId = jsonObject.Value<int?>("playedId");
            var coins = jsonObject.Value<int?>("coins");

            if (gameId == null || playerId == null || coins == null)
            {
                throw new ArgumentException("parameters mismatch.");
            }
            else
            {
                Console.WriteLine("Raising Bet. parameters are: gameId: {0}, playerId: {1}, coins: {2}", gameId, playerId, coins);
            }
            //sl.raiseBet(gameId, playerId, coins);
        }

        private static void Login(object[] args)
        {
            // this two lines must be in every method
            JObject jsonObject = (JObject)args[0];
            TcpClient client = (TcpClient)args[1];

            var username = jsonObject.Value<string>("username");
            var password = jsonObject.Value<string>("password");

            if (username == null || password == null)
            {
                throw new ArgumentException("Parameters Mismatch.");
            }

            var response = sl.Login(username, password);

            SendMessage(client, response);
        }

        static void Main()
        {
            TcpListener listener = null;
            try
            {
                var address = IPAddress.Parse("127.0.0.1");
                var port    = 2345;
                listener    = new TcpListener(address, port);

                listener.Start();

                Console.WriteLine(
                    String.Format("Server has been initialized at IP: {0} PORT: {1}", 
                    address.ToString(), 
                    port));

                while (true)
                {
                    Console.WriteLine("Waiting for new connection.");

                    TcpClient client = listener.AcceptTcpClient();

                    Console.WriteLine("Accepted new client");

                    Thread t = new Thread(ProcessClientRequests);
                    t.Start(client);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                if (listener != null)
                {
                    listener.Stop();
                }
            }
        }
    }
}
