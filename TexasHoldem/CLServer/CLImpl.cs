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
        private static void tryExecuteAction(TcpClient client, JObject jsonObject)
        {
            var action = jsonObject.Value<string>("action");

            Console.WriteLine("Trying to execute action: {0}", action);

            if (action == "Raise") {
                var gameId = jsonObject.Value<int?>("gameId");
                var playerIndex = jsonObject.Value<int?>("playedIndex");
                var coins = jsonObject.Value<int?>("coins");

                if (!gameId.HasValue || !playerIndex.HasValue || !coins.HasValue)
                {
                    throw new ArgumentException("Parameters Mismatch at Raise.");
                }
                else
                {
                    Console.WriteLine("Raising Bet. parameters are: gameId: {0}, playerIndex: {1}, coins: {2}", gameId, playerIndex, coins);
                }

                sl.raiseBet(gameId.Value, playerIndex.Value, coins.Value);
                var raiseResponse = new Boolean();
                raiseResponse = true;

                SendMessage(client, raiseResponse);
                return;
            }

            if (action == "Login") {
                var username = jsonObject.Value<string>("username");
                var password = jsonObject.Value<string>("password");

                if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Parameters Mismatch at Login.");
                }

                var loginResponse = sl.Login(username, password);

                SendMessage(client, loginResponse);
                return;
            }

            if (action == "CreateGame") {
                var gameCreatorId = jsonObject.Value<int?>("gameCreatorId");
                var gamePolicy = jsonObject.Value<int?>("gamePolicy");
                var buyInPolicy = jsonObject.Value<int?>("buyInPolicy");
                var startingChips = jsonObject.Value<int?>("startingChips");
                var minimalBet = jsonObject.Value<int?>("minimalBet");
                var minimalPlayers = jsonObject.Value<int?>("minimalPlayers");
                var maximalPlayers = jsonObject.Value<int?>("maximalPlayers");
                var spectateAllowed = jsonObject.Value<bool?>("spectateAllowed");

                if (!gameCreatorId.HasValue || !gamePolicy.HasValue)
                {
                    throw new ArgumentException("Parameters Mismatch at CreateGame.");
                }

                var createGameResponse = sl.createGame(gameCreatorId.Value, gamePolicy.Value, buyInPolicy, startingChips, minimalBet, minimalPlayers, maximalPlayers, spectateAllowed);

                SendMessage(client, createGameResponse);
                return;
            }

            if (action == "GetGame") {
                var gameId = jsonObject.Value<int?>("gameId");

                if (!gameId.HasValue)
                {
                    throw new ArgumentException("Parameters Mismatch at GetGame");
                }

                var getGameResponse = sl.getGameById(gameId.Value);

                SendMessage(client, getGameResponse);
                return;
            }

            if (action == "Register")
            {
                var username = jsonObject.Value<string>("username");
                var password = jsonObject.Value<string>("password");
                var email = jsonObject.Value<string>("email");
                var userImage = jsonObject.Value<string>("userImage");
                if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(email))
                {
                    throw new ArgumentException("Parameters Mismatch at Register");
                }

                var registerResponse = sl.Register(username, password, email, userImage);

                SendMessage(client, registerResponse);
                return;
            }

            if (action == "Logout")
            {
                var userId = jsonObject.Value<int?>("userId");

                if (!userId.HasValue)
                {
                    throw new ArgumentException("Parameters Mismatch at Logout");
                }

                var logoutResponse = sl.Logout(userId.Value);

                SendMessage(client, logoutResponse);
                return;
            }

            if (action == "JoinActiveGame")
            {
                var gameId = jsonObject.Value<int?>("gameId");
                var userId = jsonObject.Value<int?>("userId");

                if (!gameId.HasValue || !userId.HasValue)
                {
                    throw new ArgumentException("Parameters Mismatch at Join Active Game");
                }

                var joinActiveGameResponse = sl.joinActiveGame(userId.Value, gameId.Value);

                SendMessage(client, joinActiveGameResponse);
                return;
            }

            if (action == "SpectateActiveGame")
            {
                var gameId = jsonObject.Value<int?>("gameId");
                var userId = jsonObject.Value<int?>("userId");

                if (!gameId.HasValue || !userId.HasValue)
                {
                    throw new ArgumentException("Parameters Mismatch at Spectate Active Game");
                }

                var spectateActiveGameResponse = sl.spectateActiveGame(userId.Value, gameId.Value);

                SendMessage(client, spectateActiveGameResponse);
                return;
            }

            if (action == "FindAllActiveAvailableGames")
            {
                var findAllActiveAvailableGamesResponse = sl.findAllActiveAvailableGames();

                SendMessage(client, findAllActiveAvailableGamesResponse);
                return;
            }

            //TODO:: Obsolete because game preferences is now decorator. Not finished.
            if (action == "FilterActiveGamesByGamePreferences")
            {
                var gamePolicy = jsonObject.Value<int?>("gamePolicy");
                var buyInPolicy = jsonObject.Value<int?>("buyInPolicy");
                var startingChips = jsonObject.Value<int?>("startingChips");
                var minimalBet = jsonObject.Value<int?>("minimalBet");
                var minimalPlayers = jsonObject.Value<int?>("minimalPlayers");
                var maximalPlayers = jsonObject.Value<int?>("maximalPlayers");
                var spectateAllowed = jsonObject.Value<bool?>("spectateAllowed");
            }

            if (action == "FilterActiveGamesByPotSize")
            {
                var potSize = jsonObject.Value<int?>("potSize");

                if (!potSize.HasValue)
                {
                    throw new ArgumentException("Parameters Mismatch at Filter Active Games By Pot Size");
                }

                var filterActiveGamesByPotSizeResponse = sl.filterActiveGamesByPotSize(potSize);

                SendMessage(client, filterActiveGamesByPotSizeResponse);
                return;
            }

            if (action == "FilterActiveGamesByPlayerName")
            {

                var playerName = jsonObject.Value<string>("playerName");

                if (String.IsNullOrWhiteSpace(playerName))
                {
                    throw new ArgumentException("Parameters Mismatch at Filter Active Games By Player Name");
                }

                var filterActiveGamesByPlayerNameResponse = sl.filterActiveGamesByPlayerName(playerName);

                SendMessage(client, filterActiveGamesByPlayerNameResponse);
                return;
            }

            if (action == "EditUserProfile")
            {
                var userId = jsonObject.Value<int?>("userId");
                var name = jsonObject.Value<string>("name");
                var password = jsonObject.Value<string>("password");
                var email = jsonObject.Value<string>("email");
                var avatar = jsonObject.Value<string>("avatar");

                if (!userId.HasValue)
                {
                    throw new ArgumentException("Parameters Mismatch at Edit User Profile");
                }

                var editUserProfileResponse = sl.editUserProfile(userId.Value, name, password, email, avatar);

                SendMessage(client, editUserProfileResponse);
                return;
            }

            throw new ArgumentException("No known action specified.");
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
