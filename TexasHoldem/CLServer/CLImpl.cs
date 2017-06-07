using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SL;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Reflection;
using System.Web.Script.Serialization;

namespace CLServer
{
    public class CLImpl
    {
        private static SLInterface sl = new SLImpl();

        #region Constants

        private const string SUBSCRIBE_TO_MESSAGE   = "Messages";
        private const string SUBSCRIBE_TO_GAME      = "Game";
        private const string LOCAL_IP               = "127.0.0.1";
        private const int DESKTOP_PORT              = 2345;
        private const int WEB_PORT                  = 4343;

        #endregion
        
        #region Co-Server Functions

        /// <summary>
        /// Gets IP to open tcp socket to.
        /// </summary>
        /// <returns>The server's IP.</returns>
        private static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        /// <summary>
        /// Tries to execute the action given by the client.
        /// </summary>
        /// <param name="jsonObject">The Object received from the client.</param>
        private static void tryExecuteAction(ClientInfo clientInfo, JObject jsonObject)
        {
            if (jsonObject == null)
            {
                throw new ArgumentException("jsonObject cannot be null.");
            }

            var actionToken = jsonObject["action"];

            // In case the json object does not have property 'action'
            if ((actionToken == null) ||
               (actionToken.Type == JTokenType.String && String.IsNullOrWhiteSpace(actionToken.ToString())) ||
               (actionToken.Type != JTokenType.String))
            {
                throw new ArgumentException("Error: No action specified.");
            }

            var action = (string)actionToken;

            Console.WriteLine("Trying to execute action: {0}", action);

            var method = typeof(CLImpl).GetMethod(action, BindingFlags.NonPublic | BindingFlags.Static);

            if (method == null)
            {
                throw new ArgumentException("Error: No known action called.");
            }

            method.Invoke(null, new object[] { clientInfo, jsonObject });
        }

        /// <summary>
        /// Sends a message to the client.
        /// </summary>
        /// <param name="client">The client to send to.</param>
        /// <param name="message">The message to send. (Optional)</param>
        /// <param name="response">The response type, (Optional - only refers to HTTP responses.)</param>
        public static void SendMessage(ClientInfo clientInfo, object message = null, int response = 200)
        {
            if (clientInfo == null)
            {
                return;
            }

            if (clientInfo.type == ClientInfo.CLIENT_TYPE.TCP)
            {
                SendTcpMessage((TcpClient)clientInfo.client, message);
            }
            else if (clientInfo.type == ClientInfo.CLIENT_TYPE.HTTP)
            {
                sendHttpMessage((HttpListenerContext)clientInfo.client, message, response);
            }
            else
            {
                return;
            }
        }

        #endregion

        #region TCP-Server Functions

        /// <summary>
        /// Starts up the desktop client listener.
        /// </summary>
        /// <param name="address">The address of the server.</param>
        /// <param name="port">The port of the server.</param>
        private static void startDesktopListen(IPAddress address, int port)
        {
            TcpListener listener = null;
            try
            {
                listener = new TcpListener(address, port);

                listener.Start();

                Console.WriteLine(
                    String.Format("Server has been initialized at IP: {0} PORT: {1} For desktop use.",
                    address.ToString(),
                    port));

                while (true)
                {
                    Console.WriteLine("Waiting for new desktop connection.");

                    TcpClient client    = listener.AcceptTcpClient();

                    Console.WriteLine("Accepted new desktop client");

                    Thread clientThread = new Thread(ProcessDesktopClientRequests);

                    var clientInfo      = new ClientInfo(client, ClientInfo.CLIENT_TYPE.TCP);

                    clientThread.Start(clientInfo);
                }
            }
            catch (Exception e)
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

        /// <summary>
        /// Task to proccess the client's requests.
        /// </summary>
        /// <param name="obj">The tcp client.</param>
        private static void ProcessDesktopClientRequests(Object obj)
        {
            var clientInfo = (ClientInfo)obj;

            TcpClient client = (TcpClient)clientInfo.client;

            while (true)
            {
                var jsonObject = new JObject();
                try
                {
                    jsonObject = getJsonObjectFromDesktopStream(client);
                }
                catch
                {
                    Console.WriteLine("Client closed connection. Terminating thread: {0}", Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                try
                {
                    tryExecuteAction(clientInfo, jsonObject);
                }
                catch (TargetInvocationException tie)
                {
                    Console.WriteLine(tie.InnerException);
                    SendMessage(clientInfo, new { exception = "An Error Has Occured" });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    SendMessage(clientInfo, new { exception = "An Error Has Occured" });
                }
            }
        }

        /// <summary>
        /// returns a JObject as data from the client stream.
        /// </summary>
        /// <param name="client">The tcp client</param>
        /// <returns>The JObject</returns>
        private static JObject getJsonObjectFromDesktopStream(TcpClient client)
        {
            var message                 = new byte[1024 * 10];

            var bytesRead               = client.GetStream().Read(message, 0, message.Length);
            string myObject             = Encoding.ASCII.GetString(message);
            Object deserializedProduct  = JsonConvert.DeserializeObject(myObject);

            return JObject.FromObject(deserializedProduct);
        }

        /// <summary>
        /// Sends message via tcp network.
        /// </summary>
        /// <param name="client">The client to send to.</param>
        /// <param name="message">The message to send.</param>
        private static void SendTcpMessage(TcpClient client, object message = null)
        {
            JObject messageJObject = new JObject();
            if (message != null)
            {
                messageJObject["message"] = JToken.FromObject(message);
            }
            else
            {
                // If no message was to be sent, send an empty message.
                messageJObject["message"] = JToken.FromObject(new object());
            }

            var serializedMessage = JsonConvert.SerializeObject(messageJObject,
                                                                  Newtonsoft.Json.Formatting.None,
                                                                  new JsonSerializerSettings
                                                                  {
                                                                      NullValueHandling = NullValueHandling.Ignore
                                                                  });

            var messageByteArray = Encoding.ASCII.GetBytes(serializedMessage);

            try
            {
                if (client.GetStream().CanWrite)
                {
                    client.GetStream().Write(messageByteArray, 0, messageByteArray.Length);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
        
        #endregion

        #region HTTPS-Server Functions

        /// <summary>
        /// Starts up the web client listener.
        /// </summary>
        /// <param name="address">The server's address.</param>
        /// <param name="port">The server's port.</param>
        private static void startWebListen(IPAddress address, int port)
        {
            var httpListener = new HttpListener();

            //var httpsUri = String.Format(@"https://{0}:{1}/", address.ToString(), 555);
            var httpUri = String.Format(@"https://{0}:{1}/", address.ToString(), 4343);

            //httpListener.Prefixes.Add(httpsUri);
            httpListener.Prefixes.Add(httpUri);

            httpListener.Start();

            Console.WriteLine(
                String.Format("Server has been initialized at IP: {0} PORT: {1} For web use.",
                address.ToString(),
                port));

            while (true)
            {
                Console.WriteLine("Waiting for new web connection.");

                var clientContext   = httpListener.GetContext();

                Console.WriteLine("Accepted new web connection.");

                Thread clientThread = new Thread(ProcessWebClientRequests);

                var clientInfo      = new ClientInfo(clientContext, ClientInfo.CLIENT_TYPE.HTTP);

                clientThread.Start(clientInfo);
            }
        }

        /// <summary>
        /// Sends message through http network.
        /// </summary>
        /// <param name="httpContext">Http listener context to send response through.</param>
        private static void sendHttpMessage(HttpListenerContext httpContext, object message = null, int response = 200)
        {
            // Construct the callback message. (Imported code.)
            httpContext.Response.ContentType    = "application/json";
            var callback                        = httpContext.Request.QueryString["callback"];
            var Param1                          = httpContext.Request.QueryString["Param1"];
            object dataToSend                   = message;
            var js                              = new JavaScriptSerializer();
            var JSONstring                      = js.Serialize(dataToSend);
            var JSONPstring                     = string.Format("{0}{1}", callback, JSONstring);

            // Transform the callback message to byte array.
            var buf                             = Encoding.ASCII.GetBytes(JSONPstring);

            // Allow access control allow origin.
            httpContext.Response.StatusCode = response;
            httpContext.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
            httpContext.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            httpContext.Response.AddHeader("Access-Control-Max-Age", "1728000");
            httpContext.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            // Write back the response to the response stream.
            httpContext.Response.OutputStream.Write(buf, 0, buf.Length);
        }

        /// <summary>
        /// returns a JObject as data from the client stream.
        /// </summary>
        /// <param name="httpContext">The http context.</param>
        /// <returns>The JObject.</returns>
        private static JObject getJsonObjectFromWebStream(HttpListenerContext httpContext)
        {
            if (httpContext.Request.HttpMethod == "OPTIONS")
            {
                sendHttpMessage(httpContext);
            }

            if (!httpContext.Request.HasEntityBody)
            {
                return null;
            }



            using (System.IO.Stream body = httpContext.Request.InputStream)
            {
                using (System.IO.StreamReader reader = new System.IO.StreamReader(body, httpContext.Request.ContentEncoding))
                {
                    var myObject = reader.ReadToEnd();

                    Object deserializedProduct = JsonConvert.DeserializeObject(myObject);
                    
                    return JObject.FromObject(deserializedProduct);
                }
            }
        }

        /// <summary>
        /// Task to proccess the client's requests by web.
        /// </summary>
        /// <param name="obj">The web client.</param>
        private static void ProcessWebClientRequests(Object obj)
        {
            ClientInfo clientInfo = (ClientInfo)obj;

            var httpContext = (HttpListenerContext)clientInfo.client;

            var jsonObject = new JObject();
            try
            {
                jsonObject = getJsonObjectFromWebStream(httpContext);
            }
            catch
            {
                Console.WriteLine("Client closed connection. Terminating thread: {0}", Thread.CurrentThread.ManagedThreadId);
                return;
            }

            // Execute action and return response.
            if (jsonObject != null)
            {
                try
                {
                    tryExecuteAction(clientInfo, jsonObject);
                }
                catch (TargetInvocationException tie)
                {
                    Console.WriteLine(tie.InnerException);
                    SendMessage(clientInfo, new { exception = "An Error Has Occured" }, 500);
                    httpContext.Request.InputStream.Close();
                    httpContext.Response.OutputStream.Close();
                    return;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    SendMessage(clientInfo, new { exception = "An Error Has Occured" });
                    httpContext.Request.InputStream.Close();
                    httpContext.Response.OutputStream.Close();
                    return;
                }
            }

            httpContext.Request.InputStream.Close();
            httpContext.Response.OutputStream.Close();
        }
 
        #endregion

        #region GameWindow

        private static void Bet(ClientInfo client, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var playerIndexToken = jsonObject["playerIndex"];
            var coinsToken = jsonObject["coins"];

            if (((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer)) ||
                ((playerIndexToken == null) || (playerIndexToken.Type != JTokenType.Integer)) ||
                ((coinsToken == null) || (coinsToken.Type != JTokenType.Integer)))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Raise."));
            }

            var gameId = (int)gameIdToken;
            var playerIndex = (int)playerIndexToken;
            var coins = (int)coinsToken;

            Console.WriteLine("Bet. parameters are: gameId: {0}, playerIndex: {1}, coins: {2}", gameId, playerIndex, coins);
            
            SendMessage(client, new { response = sl.Bet(gameId, playerIndex, coins) });
        }

        private static void AddMessage(ClientInfo client, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var playerIndexToken = jsonObject["playerIndex"];
            var messageTextToken = jsonObject["messageText"];

            if (((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer)) ||
                ((playerIndexToken == null) || (playerIndexToken.Type != JTokenType.Integer)) ||
                ((messageTextToken == null) || (messageTextToken.Type != JTokenType.String)))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Add Message."));
            }

            var gameId = (int)gameIdToken;
            var playerIndex = (int)playerIndexToken;
            var messageText = (string)messageTextToken;

            Console.WriteLine("Message added. parameters are: gameId: {0}, playerIndex: {1}, messageText: {2}", gameId, playerIndex, messageText);
            
            SendMessage(client, new { response = sl.AddMessage(gameId, playerIndex, messageText) });
        }

        private static void Fold(ClientInfo client, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var playerIndexToken = jsonObject["playerIndex"];

            if (((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer)) ||
                ((playerIndexToken == null) || (playerIndexToken.Type != JTokenType.Integer)))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Fold."));
            }

            var gameId = (int)gameIdToken;
            var playerIndex = (int)playerIndexToken;

            Console.WriteLine("Fold. parameters are: gameId: {0}, playerIndex: {1}", gameId, playerIndex);
            
            SendMessage(client, new { response = sl.Fold(gameId, playerIndex) });
        }

        private static void Check(ClientInfo client, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var playerIndexToken = jsonObject["playerIndex"];

            if (((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer)) ||
                ((playerIndexToken == null) || (playerIndexToken.Type != JTokenType.Integer)))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Check."));
            }

            var gameId = (int)gameIdToken;
            var playerIndex = (int)playerIndexToken;

            Console.WriteLine("Check. parameters are: gameId: {0}, playerIndex: {1}", gameId, playerIndex);

            SendMessage(client, new { response = sl.Check(gameId, playerIndex) });
        }

        private static void Call(ClientInfo client, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var playerIndexToken = jsonObject["playerIndex"];

            if (((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer)) ||
                ((playerIndexToken == null) || (playerIndexToken.Type != JTokenType.Integer)))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Check."));
            }

            var gameId = (int)gameIdToken;
            var playerIndex = (int)playerIndexToken;

            Console.WriteLine("Call. parameters are: gameId: {0}, playerIndex: {1}", gameId, playerIndex);

            SendMessage(client, new { response = sl.Call(gameId, playerIndex) });
        }

        private static void removeUser(ClientInfo client, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var userIdToken = jsonObject["userId"];

            if (((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer)) ||
                ((userIdToken == null) || (userIdToken.Type != JTokenType.Integer)))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Check."));
            }

            var gameId = (int)gameIdToken;
            var userId = (int)userIdToken;

            Console.WriteLine("Leave game. parameters are: gameId: {0}, userId: {1}", gameId, userId);

            SendMessage(client, new { response = sl.removeUser(gameId, userId) });
        }

        private static void playGame(ClientInfo client, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];

            if (((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer)))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Check."));
            }
            
            var response = sl.playGame((int)gameIdToken);

            SendMessage(client, response);
        }

        private static void GetGameState(ClientInfo client, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];

            if ((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Get Game State."));
            }
            
            var response = sl.GetGameState((int)gameIdToken);

            SendMessage(client, response);
        }

        private static void GetPlayer(ClientInfo clientInfo, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var playerSeatIndexToken = jsonObject["playerSeatIndex"];

            if ((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer) ||
                (playerSeatIndexToken == null) || (playerSeatIndexToken.Type != JTokenType.Integer))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Get Player."));
            }

            var gameId = (int)gameIdToken;
            var playerSeatIndex = (int)playerSeatIndexToken;

            SendMessage(clientInfo, new { response = sl.GetPlayer(gameId, playerSeatIndex) });
        }

        private static void GetPlayerCards(ClientInfo client, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var userId = jsonObject["userId"];

            if ((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer) ||
                (userId == null) || (userId.Type != JTokenType.Integer))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Get Player Cards."));
            }

            var gameId = (int)gameIdToken;
            var playerSeatIndex = (int)userId;

            var response = sl.GetPlayerCards((int)gameIdToken, (int)userId);

            SendMessage(client, response);
        }
        
        #endregion

        #region Actions

        private static void Login(ClientInfo clientInfo, JObject jsonObject)
        {
            var usernameToken = jsonObject["username"];
            var passwordToken = jsonObject["password"];

            if ((usernameToken == null) || (usernameToken.Type != JTokenType.String) || 
                (String.IsNullOrWhiteSpace(usernameToken.ToString())) ||

                (passwordToken == null) || (passwordToken.Type != JTokenType.String) ||
                (String.IsNullOrWhiteSpace(passwordToken.ToString())))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Login."));
            }

            var loginResponse = sl.Login((string)usernameToken, (string)passwordToken);

            SendMessage(clientInfo, loginResponse);
        }

        private static void CreateGame(ClientInfo clientInfo, JObject jsonObject) {
            var gameCreatorIdToken      = jsonObject["gameCreatorId"];
            var gamePolicyToken         = jsonObject["gamePolicy"];
            var gamePolicyLimitToken    = jsonObject["gamePolicyLimit"];
            var buyInPolicyToken        = jsonObject["buyInPolicy"];
            var startingChipsToken      = jsonObject["startingChips"];
            var minimalBetToken         = jsonObject["minimalBet"];
            var minimalPlayersToken     = jsonObject["minimalPlayers"];
            var maximalPlayersToken     = jsonObject["maximalPlayers"];
            var spectateAllowedToken    = jsonObject["spectateAllowed"];
            var isLeagueToken           = jsonObject["isLeague"];


            if ((gameCreatorIdToken == null) || (gameCreatorIdToken.Type != JTokenType.Integer) /*||
                (gamePolicyToken == null) || (gamePolicyToken.Type != JTokenType.String) ||
                String.IsNullOrWhiteSpace((string)(gamePolicyToken))*/)
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Create Game."));
            }
            
            var createGameResponse = sl.createGame(
                (int)gameCreatorIdToken, 
                (string)gamePolicyToken,
                (int?) gamePolicyLimitToken,
                (int?)buyInPolicyToken, 
                (int?)startingChipsToken, 
                (int?)minimalBetToken, 
                (int?)minimalPlayersToken, 
                (int?)maximalPlayersToken, 
                (bool?)spectateAllowedToken,
                (bool?)isLeagueToken);

            SendMessage(clientInfo, createGameResponse);
            return;
        }

        private static void getGame(ClientInfo clientInfo, JObject jsonObject) {
            var gameIdToken = jsonObject["gameId"];

            if (gameIdToken == null || gameIdToken.Type != JTokenType.Integer)
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Get Game"));
            }

            var getGameResponse = sl.getGameById((int)gameIdToken);

            SendMessage(clientInfo, getGameResponse);
            return;
        }

        private static void Register(ClientInfo clientInfo, JObject jsonObject)
        {
            var usernameToken   = jsonObject["username"];
            var passwordToken   = jsonObject["password"];
            var emailToken      = jsonObject["email"];
            var userImageToken  = jsonObject["userImage"];
            if ((usernameToken == null) || (usernameToken.Type != JTokenType.String) ||
                (String.IsNullOrWhiteSpace(usernameToken.ToString())) ||

                (passwordToken == null) || (passwordToken.Type != JTokenType.String) ||
                (String.IsNullOrWhiteSpace(passwordToken.ToString())) ||

                (emailToken == null) || (emailToken.Type != JTokenType.String) ||
                (String.IsNullOrWhiteSpace(emailToken.ToString())) ||

                (userImageToken == null) || (userImageToken.Type != JTokenType.String) ||
                (String.IsNullOrWhiteSpace(userImageToken.ToString())))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Register"));
            }

            var registerResponse = sl.Register(
                (string)usernameToken, 
                (string)passwordToken, 
                (string)emailToken, 
                (string)userImageToken);
            
            SendMessage(clientInfo, registerResponse);
            return;
        }

        private static void Logout(ClientInfo clientInfo, JObject jsonObject)
        {
            var userIdToken = jsonObject["userId"];

            if (userIdToken == null || userIdToken.Type != JTokenType.Integer)
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Logout"));
            }

            var logoutResponse = sl.Logout((int)userIdToken);

            SendMessage(clientInfo, logoutResponse);
            return;
        }

        private static void JoinGame(ClientInfo clientInfo, JObject jsonObject)
        {
            var userIdToken = jsonObject["userId"];
            var gameIdToken = jsonObject["gameId"];
            var playerSeatIndexToken = jsonObject["playerSeatIndex"];
            
            if ((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer) ||
                (userIdToken == null) || (userIdToken.Type != JTokenType.Integer) ||
                (playerSeatIndexToken == null) || (playerSeatIndexToken.Type != JTokenType.Integer))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Join Active Game"));
            }

            var joinGameResponse = sl.joinGame((int)userIdToken, (int)gameIdToken, (int) playerSeatIndexToken);
            
            SendMessage(clientInfo, joinGameResponse);
        }

        private static void GetGameForPlayers(ClientInfo clientInfo, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var userIdToken = jsonObject["userId"];

            if ((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer) ||
                (userIdToken == null) || (userIdToken.Type != JTokenType.Integer))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at get Game for player"));
            }
            
            var getGameForPlayersResponse = sl.GetGameForPlayers((int)userIdToken, (int)gameIdToken);

            SendMessage(clientInfo, getGameForPlayersResponse);
            return;
        }

        private static void SpectateActiveGame(ClientInfo clientInfo, JObject jsonObject)
        {
            var gameIdToken = jsonObject["gameId"];
            var userIdToken = jsonObject["userId"];

            if ((gameIdToken == null) || (gameIdToken.Type != JTokenType.Integer) ||
                (userIdToken == null) || (userIdToken.Type != JTokenType.Integer))
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Spectate Active Game"));
            }

            var spectateActiveGameResponse = sl.spectateActiveGame((int)userIdToken, (int)gameIdToken);

            SendMessage(clientInfo, spectateActiveGameResponse);
            return;
        }

        private static void FindAllActiveAvailableGames(ClientInfo clientInfo, JObject jsonObject)
        {
            var findAllActiveAvailableGamesResponse = sl.findAllActiveAvailableGames();

            SendMessage(clientInfo, findAllActiveAvailableGamesResponse);
            return;
        }

        private static void FilterActiveGamesByGamePreferences(ClientInfo clientInfo, JObject jsonObject)
        {
            var gamePolicy          = jsonObject["gamePolicy"];
            var limitPolicy         = jsonObject["gamePolicyLimit"];
            var buyInPolicy         = jsonObject["buyInPolicy"];
            var startingChips       = jsonObject["startingChips"];
            var minimalBet          = jsonObject["minimalBet"];
            var minimalPlayers      = jsonObject["minimalPlayers"];
            var maximalPlayers      = jsonObject["maximalPlayers"];
            var spectateAllowed     = jsonObject["spectateAllowed"];
            var isLeague            = jsonObject["isLeague"];

            var filterActiveGamesByGamePreferencesResponse = sl.filterActiveGamesByGamePreferences(
                (string)gamePolicy,
                (int?)limitPolicy,
                (int?)buyInPolicy,
                (int?)startingChips,
                (int?)minimalBet,
                (int?)minimalPlayers,
                (int?)maximalPlayers,
                (bool?)spectateAllowed,
                (bool?)isLeague);

            SendMessage(clientInfo, filterActiveGamesByGamePreferencesResponse);
            return;

        }

        private static void FilterActiveGamesByPotSize(ClientInfo clientInfo, JObject jsonObject)
        {
            var potSizeToken = jsonObject["potSize"];

            if ((potSizeToken == null) || (potSizeToken.Type != JTokenType.Integer))
            {
                throw new TargetInvocationException(
                    new ArgumentException("Error: Parameters Mismatch at Filter Active Games By Pot Size"));
            }

            var filterActiveGamesByPotSizeResponse = sl.filterActiveGamesByPotSize((int?)potSizeToken);

            SendMessage(clientInfo, filterActiveGamesByPotSizeResponse);
            return;
        }

        private static void FilterActiveGamesByPlayerName(ClientInfo clientInfo, JObject jsonObject)
        {
            var playerNameToken = jsonObject["playerName"];

            if ((playerNameToken == null) || (playerNameToken.Type != JTokenType.String) ||
                (String.IsNullOrWhiteSpace(playerNameToken.ToString())))
            {
                throw new TargetInvocationException(
                    new ArgumentException("Error: Parameters Mismatch at Filter Active Games By Player Name"));
            }

            var filterActiveGamesByPotSizeResponse = sl.filterActiveGamesByPlayerName((string)playerNameToken);

            SendMessage(clientInfo, filterActiveGamesByPotSizeResponse);
            return;
        }

        private static void EditUserProfile(ClientInfo clientInfo, JObject jsonObject)
        {
            var userIdToken     = jsonObject["userId"];
            var nameToken       = jsonObject["name"];
            var passwordToken   = jsonObject["password"];
            var emailToken      = jsonObject["email"];
            var avatarToken     = jsonObject["avatar"];
            var amountToken     = jsonObject["amount"];

            Console.WriteLine("trying");
            Console.WriteLine(passwordToken);

            if (userIdToken == null || userIdToken.Type != JTokenType.Integer)
            {
                throw new TargetInvocationException(new ArgumentException("Error: Parameters Mismatch at Edit User Profile"));
            }

            var editUserProfileResponse = sl.editUserProfile(
                (int)userIdToken,
                (string)nameToken,
                (string)passwordToken,
                (string)emailToken,
                (string)avatarToken,
                (int)amountToken);
                

            SendMessage(clientInfo, editUserProfileResponse);
            return;
        }

        private static void Subscribe(ClientInfo clientInfo, JObject jsonObject)
        {
            var subscribeToToken = jsonObject["to"];

            if ((subscribeToToken == null) ||
                (subscribeToToken.Type != JTokenType.String) ||
                (String.IsNullOrWhiteSpace((string)subscribeToToken)))
            {
                Console.WriteLine("Parameters mismatch at Subscribe -- subscribeTo.");
                SendMessage(clientInfo, new { exception = "Could not register." });
                return;
            }

            var subscribeTo = (string)subscribeToToken;

            if (subscribeTo == SUBSCRIBE_TO_GAME)
            {
                SubscribeToGame(clientInfo, jsonObject);
                return;
            }

            if (subscribeTo == SUBSCRIBE_TO_MESSAGE)
            {
                // TODO:: MESSAGE SYSTEM!! ^^
            }

        }

        private static void SubscribeToGame(ClientInfo clientInfo, JObject jsonObject)
        {
            var optionalToken = jsonObject["optional"];

            if ((optionalToken == null) ||
                (optionalToken.Type != JTokenType.Integer))
            {
                Console.WriteLine("Parameters mismatch at Subscribe -- optional.");
                SendMessage(clientInfo, new { exception = "Could not register." });
                return;
            }
            var optional = (int)optionalToken;

            // Check if game exists.
            if (sl.getGameById(optional) != null)
            {
                // Subscribe this channel to game.
                sl.SubscribeToGameState(new ServerObserver((TcpClient)clientInfo.client), (int)optionalToken);
            }
        }

        #endregion

        #region Web-Requests

        /// <summary>
        /// Just a dummy leaderboard function and how it has to be, in order to build web client on top of it.
        /// </summary>
        /// <param name="clientInfo">The client's connection info.</param>
        /// <param name="jsonObject">The json object.</param>
        private static void LeaderBoard(ClientInfo clientInfo, JObject jsonObject)
        {
            var rand = new Random();
            var dummyList = new List<object>();
            for (int i=0; i<20; i++)
            {
                dummyList.Add(new
                {
                    playerName          = "abuya",
                    highestCash         = rand.Next(5000, 50000),
                    totalGrossProfit    = rand.Next(100000, 1000000),
                    gamesPlayed         = rand.Next(10, 120)
                });
            }

            //var param = (string)jsonObject["param"];
            //var leaderBoards = sl.getLeaderboardsByParam(param);

            SendMessage(clientInfo, dummyList);
        }

        /// <summary>
        /// Gets the details of all the users in the system.
        /// </summary>
        /// <param name="clientInfo">The client's connection info.</param>
        /// <param name="jsonObject">The json object.</param>
        private static void GetUsersDetails(ClientInfo clientInfo, JObject jsonObject)
        {
            var userList = sl.getUsersDetails();

            SendMessage(clientInfo, userList);
        }
        #endregion

        static void Main()
        {
            var realIP = false;

            var IP = LOCAL_IP;

            try
            {
                if (realIP)
                {
                    IP = GetLocalIPAddress();
                }
                Console.WriteLine("Server IP is: {0}", IP);
            }
            catch
            {
                Console.WriteLine("Not connected to internet. aborting.");
                return;
            }
            var address = IPAddress.Parse(IP);

            Task.Factory.StartNew(() =>
            {
                startDesktopListen(address, DESKTOP_PORT);
            });

            Task.Factory.StartNew(() =>
            {
                startWebListen(address, WEB_PORT);
            });

            ManualResetEvent manualResetEvent = new ManualResetEvent(false);

            manualResetEvent.WaitOne();
        }
    }
}
