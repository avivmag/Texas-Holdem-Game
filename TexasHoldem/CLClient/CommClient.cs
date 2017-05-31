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
using System.Threading;

namespace CLClient
{
    public static class CommClient
    {
        private static TcpClient client = new TcpClient("192.168.43.62", 2345);

        #region Constants

        private const string SERVER_IP              = "127.0.0.1";
        private const int SERVER_PORT               = 2345;
        private const int MAIN_CLIENT               = 305278202;
        private const int MESSAGE_CLIENT            = 9440990;
        private const string SUBSCRIBE_TO_MESSAGE   = "Messages";
        private const string SUBSCRIBE_TO_GAME      = "Game";

        #endregion

        /// <summary>
        /// Client pool.
        /// </summary>
        private static Dictionary<int, TcpClient> clients = new Dictionary<int, TcpClient>();

        /// <summary>
        /// Private server listener class to wrap paremeters for listen threads.
        /// </summary>
        private class serverListener
        {
            public TcpClient client;
            public IObservable toUpdate;

            public serverListener(TcpClient client, IObservable toUpdate)
            {
                this.client = client;
                this.toUpdate = toUpdate;
            }
        }


        #region Static functionality

        /// <summary>
        /// Closes a connection to the server. If none given, closes all connections.
        /// </summary>
        public static void closeConnection(int? clientId = null)
        {
            if (clientId == null)
            {
                // Close all client connections.
                foreach (KeyValuePair<int, TcpClient> client in clients)
                {
                    client.Value.Close();
                }

                // Clear client pool.
                clients.Clear();
            }
            else
            {
                // Close client and remove from client pool.
                clients[clientId.Value].Close();
                clients.Remove(clientId.Value);
            }
        }

        /// <summary>
        /// Sends a message to the server. via the MAIN_CLIENT stream.
        /// </summary>
        /// <param name="obj">An anonymous object. MUST have action property.</param>
        /// <returns></returns>
        public static JObject sendMessage(object obj, TcpClient client = null, bool isResponseNeeded = true)
        {
            // If no client was explicitly assigned, get default TcpClient to send message to.
            if (client == null)
            {
                client = clients[MAIN_CLIENT];
            }

            var jsonObj             = JObject.FromObject(obj);
            var serializedJsonObj   = JsonConvert.SerializeObject(jsonObj);

            var networkStream = client.GetStream();

            if (networkStream.CanWrite)
            {
                var jsonObjArray = Encoding.ASCII.GetBytes(serializedJsonObj);

                networkStream.Write(jsonObjArray, 0, jsonObjArray.Length);
            }

            if (isResponseNeeded)
            {
                return getJsonObjectFromStream(client);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the next json object from the stream.
        /// </summary>
        /// <param name="client">The Client stream that holds a message.</param>
        /// <returns>Server message as JSON object.</returns>
        private static JObject getJsonObjectFromStream(TcpClient client)
        {
            var message = new byte[1024 * 10];

            try
            {
                var bytesRead = client.GetStream().Read(message, 0, message.Length);
            }
            catch
            {
                return null;
            }

            string myObject = Encoding.ASCII.GetString(message);

            Object deserializedProduct = JsonConvert.DeserializeObject(myObject);

            var toRet = JObject.FromObject(deserializedProduct);

            return toRet;
        }
    
        /// <summary>
        /// Checks if it is a valid response.
        /// </summary>
        /// <param name="jsonMessage">The message to check if valid.</param>
        /// <returns>The jsonMessage from server.</returns>
        private static JToken getResponse(JObject jsonMessage)
        {
            if (jsonMessage == null)
            {
                return null;
            }

            var responseJson = jsonMessage["message"];

            if ((responseJson == null) || (responseJson.Type == JTokenType.Array && !responseJson.HasValues) ||
               (responseJson.Type == JTokenType.Object && !responseJson.HasValues) ||
               (responseJson.Type == JTokenType.String && responseJson.ToString() == String.Empty) ||
               (responseJson.Type == JTokenType.Null) ||
               ((responseJson.Type == JTokenType.Object) && (responseJson["exception"] != null)))
            {
                return null;
            }
            else
            {
                return responseJson;
            }
        }

        /// <summary>
        /// Adds the client stream to the client pool.
        /// </summary>
        /// <param name="clientId">The client's Id.</param>
        private static void addClientStream(int clientId)
        {
            clients.Add(clientId, new TcpClient(SERVER_IP, SERVER_PORT));
        }

        /// <summary>
        /// Subscribes stream to server side operations.
        /// </summary>
        /// <param name="messageStreamId">The stream's client id to subscribe.</param>
        private static void subscribeStreamToServer(int clientId, string to, object optional = null)
        {
            var subscribeMessage = new { action = "Subscribe", to, optional };
            sendMessage(subscribeMessage, clients[clientId], false);
        }

        /// <summary>
        /// Listens to client's stream in the background, and updates a given object by the server's request.
        /// </summary>
        /// <param name="client">Stream's client to listen to.</param>
        /// <param name="toUpdate">The object to update.</param>
        private static void Listen(Object obj)
        {
            var client      = ((serverListener)obj).client;
            var toUpdate    = ((serverListener)obj).toUpdate;

            while (true)
            {
                var response = getJsonObjectFromStream(client);

                var jsonResponse = getResponse(response);

                if (jsonResponse == null)
                {
                    return;
                }

                var responseStringToken = jsonResponse["response"];

                if ((responseStringToken == null) ||
                    (responseStringToken.Type != JTokenType.String) ||
                    (String.IsNullOrWhiteSpace((string)responseStringToken)))
                {
                    return;
                }

                if ((string)responseStringToken == "Game")
                {
                    var gameResponseToken = jsonResponse["obj"];
                    if (responseStringToken != null)
                    {
                        var gameResponse = gameResponseToken.ToObject<TexasHoldemGame>();
                        toUpdate.update(gameResponse);
                    }
                }
            }
        }
       
        #endregion

        #region PL Functions

        public static SystemUser Login(string username, string password)
        {
            addClientStream(MAIN_CLIENT);

            var message         = new { action = "Login", username, password };
            var jsonMessage     = sendMessage(message);
            var responseJson    = getResponse(jsonMessage);
            
            if (responseJson == null)
            {
                closeConnection(MAIN_CLIENT);
                return null;
            }
            var response        = responseJson.ToObject<SystemUser>();

            addClientStream(MESSAGE_CLIENT);

            // Open a different channel to recieve system messages from the server.
            subscribeStreamToServer(MESSAGE_CLIENT, SUBSCRIBE_TO_MESSAGE);

            // Null needs to change to message observer.
            Thread listenThread = new Thread(Listen);

            var listener = new serverListener(clients[MESSAGE_CLIENT], null);

            listenThread.Start(listener);

            return response;
        }

        public static ReturnMessage Logout(int userId)
        {
            var message     = new { action = "Logout", userId };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<ReturnMessage>();
            if (response.success)
            {
                closeConnection(MAIN_CLIENT);
            }
            return response;
        }

        public static TexasHoldemGame CreateGame(int gameCreatorId, string gamePolicy, int gamePolicyLimit, int buyInPolicy, int startingChips, int minimalBet, int minimalPlayers, int maximalPlayers, bool? spectateAllowed, bool? isLeague)
        {
            var message = new
            {
                action = "CreateGame",
                gameCreatorId,
                gamePolicy,
                gamePolicyLimit,
                buyInPolicy,
                startingChips,
                minimalBet,
                minimalPlayers,
                maximalPlayers,
                spectateAllowed,
                isLeague
            };

            var jsonMessage     = sendMessage(message);
            var responseJson    = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<TexasHoldemGame>();

            response.gamePreferences.flatten();

            addClientStream(response.gameId);

            subscribeStreamToServer(response.gameId, SUBSCRIBE_TO_GAME, response.gameId);

            var serverListener = new serverListener(clients[response.gameId], response);

            Thread listenThread = new Thread(Listen);

            listenThread.Start(serverListener);

            return response;
        }

        public static TexasHoldemGame getGame(int gameId)
        {
            var message     = new { action = "GetGame", gameId };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<TexasHoldemGame>();

            response.gamePreferences.flatten();

            return response;
        }

        public static TexasHoldemGame joinActiveGame(int userId, int gameId)
        {
            var message     = new { action = "JoinActiveGame", userId, gameId };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }

            var response = responseJson.ToObject<TexasHoldemGame>();

            response.gamePreferences.flatten();

            addClientStream(response.gameId);

            subscribeStreamToServer(response.gameId, SUBSCRIBE_TO_GAME, response.gameId);

            var serverListener = new serverListener(clients[response.gameId], response);

            Thread listenThread = new Thread(Listen);

            listenThread.Start(serverListener);

            return response;
        }

        public static TexasHoldemGame spectateActiveGame(int userId, int gameId)
        {
            var message     = new { action = "SpectateActiveGame", userId, gameId };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }

            var response    = responseJson.ToObject<TexasHoldemGame>();

            response.gamePreferences.flatten();

            return response;
        }

        public static List<TexasHoldemGame> findAllActiveAvailableGames()
        {
            var message         = new { action = "FindAllActiveAvailableGames" };
            var jsonMessage     = sendMessage(message);
            var responseJson    = getResponse(jsonMessage);
            if (responseJson == null)
            {
                return null;
            }

            var response = responseJson.ToObject<List<TexasHoldemGame>>();

            foreach (var thg in response)
            {
                thg.gamePreferences.flatten();
            }

            return response;
        }

        public static List<TexasHoldemGame> filterActiveGamesByGamePreferences(string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChips, int? minimalBet, int? minimalPlayers, int? maximalPlayers, bool? spectateAllowed, bool? isLeague)
        {
            var message = new
            {
                action = "FilterActiveGamesByGamePreferences",
                gamePolicy,
                gamePolicyLimit,
                buyInPolicy,
                startingChips,
                minimalBet,
                minimalPlayers,
                maximalPlayers,
                spectateAllowed,
                isLeague
            };

            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<List<TexasHoldemGame>>();

            foreach (var thg in response)
            {
                thg.gamePreferences.flatten();
            }

            return response;
        }

        public static List<TexasHoldemGame> filterActiveGamesByPotSize(int potSize)
        {
            var message = new { action = "FilterActiveGamesByPotSize", potSize };

            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<List<TexasHoldemGame>>();

            foreach (var thg in response)
            {
                thg.gamePreferences.flatten();
            }

            return response;
        }

        public static List<TexasHoldemGame> filterActiveGamesByPlayerName(string playerName)
        {
            var message = new { action = "FilterActiveGamesByPlayerName", playerName };

            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<List<TexasHoldemGame>>();

            foreach (var thg in response)
            {
                thg.gamePreferences.flatten();
            }

            return response;
        }

        public static SystemUser Register(string username, string password, string email, string userImage)
        {
            addClientStream(MAIN_CLIENT);

            var message         = new
            {
                action = "Register",
                username,
                password,
                email,
                userImage
            };

            var jsonMessage     = sendMessage(message);
            var responseJson    = getResponse(jsonMessage);

            if (responseJson == null)
            {
                closeConnection(MAIN_CLIENT);
                return null;
            }
            var response = responseJson.ToObject<SystemUser>();

            // Add a stream to the message system.
            addClientStream(MESSAGE_CLIENT);

            // Open a different channel to recieve system messages from the server.
            subscribeStreamToServer(MESSAGE_CLIENT, SUBSCRIBE_TO_MESSAGE);

            // Null needs to change to some list of messages.
            Thread listenThread = new Thread(Listen);

            var listener = new serverListener(clients[MESSAGE_CLIENT], null);

            listenThread.Start(listener);

            return response;
        }

        public static bool? editUserProfile(int userId, string name, string password, string email, string avatar, int amount)
        {
            var message = new { action = "EditUserProfile", userId, name, password, email, avatar, amount };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<ReturnMessage>();

            return response.success;
        }

        #endregion

        #region gameWindow

        public static ReturnMessage Bet(int gameId, int playerIndex, int coins)
        {
            var message     = new { action = "Bet", gameId, playerIndex, coins };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<ReturnMessage>();

            return response;
        }

        public static ReturnMessage AddMessage(int gameId, int playerIndex, string messageText)
        {
            var message = new { action = "AddMessage", gameId, playerIndex, messageText };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<ReturnMessage>();

            return response;
        }

        public static ReturnMessage Fold(int gameId, int playerIndex)
        {
            var message = new { action = "Fold", gameId, playerIndex };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<ReturnMessage>();

            return response;
        }

        public static ReturnMessage Check(int gameId, int playerIndex)
        {
            var message = new { action = "Check", gameId, playerIndex };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<ReturnMessage>();

            return response;
        }

        public static ReturnMessage playGame(int gameId)
        {
            var message = new { action = "playGame", gameId };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<ReturnMessage>();

            return response;
        }

        public static TexasHoldemGame GetGameState(int gameId)
        {
            var message = new { action = "GetGameState", gameId };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<TexasHoldemGame>();

            response.gamePreferences.flatten();

            return response;
        }

        public static ReturnMessage ChoosePlayerSeat(int gameId, int playerSeatIndex)
        {
            var message = new { action = "ChoosePlayerSeat", gameId, playerSeatIndex };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<ReturnMessage>();

            return response;
        }

        public static Player GetPlayer(int gameId, int playerSeatIndex)
        {
            var message = new { action = "GetPlayer", gameId, playerSeatIndex };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<Player>();

            return response;
        }

        public static Card[] GetPlayerCards(int gameId, int playerSeatIndex)
        {
            var message = new { action = "GetPlayerCards", gameId, playerSeatIndex };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<Card[]>();

            return response;
        }

        public static IDictionary<int, Card[]> GetShowOff(int gameId)
        {
            var message = new { action = "GetShowOff", gameId };
            var jsonMessage = sendMessage(message);
            var responseJson = getResponse(jsonMessage);

            if (responseJson == null)
            {
                return null;
            }
            var response = responseJson.ToObject<IDictionary<int, Card[]>>();

            return response;
        }
        
        #endregion
    }
}
