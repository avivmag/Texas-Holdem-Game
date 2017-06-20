using System;
using System.Collections.Generic;
using Backend;
using Backend.Game;
using Backend.Game.DecoratorPreferences;
using Backend.User;
using Backend.Messages;
using static Backend.Game.DecoratorPreferences.GamePolicyDecPref;
using Database;

namespace ApplicationFacade
{
	public class GameCenter
	{
        private List<TexasHoldemGame> texasHoldemGames;

        public List<TexasHoldemGame> TexasHoldemGames
        {
            get { foreach (TexasHoldemGame t in texasHoldemGames)
                    if (numberOfPlayersInGame(t) == 0)
                        texasHoldemGames.Remove(t);
                  return texasHoldemGames;
                }
            set => texasHoldemGames = value;
        }

        public List<League> leagues { get; set; }
        //public List<SystemUser> loggedInUsers { get; set; }
        //public List<SystemUser> userList { get; set; }
        private static GameCenter center;
        private IDB db;
        public MessageObserver messageObserver = new MessageObserver();


        private GameCenter()
        {
            texasHoldemGames = new List<TexasHoldemGame>();
            leagues = new List<League>();
            //userList = new List<SystemUser>();
            //loggedInUsers = new List<SystemUser>();
            db = new DBImpl();
        }

        public static GameCenter getGameCenter()
        {
            if (center == null)
            {
                center = new GameCenter();
            }
            return center;
        }

        public void shutDown()
        {
            texasHoldemGames.Clear();
            leagues.Clear();
            //loggedInUsers.Clear();
        }

        // Maintain leagues for players. Should be invoked once a week.
        public void maintainLeagues(List<SystemUser> users)
        {
            List<SystemUser> tempList = new List<SystemUser>(users);
            int numOfUsers = users.Count;

            if (numOfUsers < 2)
            {
                League l = new League();
                foreach (SystemUser user in tempList)
                {
                    l.addUser(user);
                }
            }
            else
            {
                int numOfLeagues = (int)Math.Ceiling(numOfUsers / Math.Sqrt(numOfUsers));
                int numOfPlayersInLeague = numOfUsers / numOfLeagues;
                leagues.Clear();

                for (int j = 0; j < numOfLeagues; j++)
                {
                    League l = new League();
                    for (int i = 0; i < numOfPlayersInLeague; i++)
                    {
                        SystemUser currHighestRankUser = getHighest(tempList);
                        if (currHighestRankUser != null)
                        {
                            tempList.Remove(currHighestRankUser);
                            l.addUser(currHighestRankUser);
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (l.Users.Count > 0)
                        leagues.Add(l);
                }
            }
        }
        
        public ReturnMessage logout(int userId)
        {
            SystemUser systemUser = db.getUserById(userId);
            if (systemUser == null)
                return new ReturnMessage(false, "User does not exists");

            if (systemUser.spectatingGame.Count > 0)
                return new ReturnMessage(false, "you are active in some games as spectator, leave them and then log out.");

            if (userPlay(systemUser))
            {
                return new ReturnMessage(false, "you are active in some games as a player, leave them and then log out.");
            }
            //if (loggedInUsers.Contains(systemUser)){
            //    loggedInUsers.Remove(systemUser);
            return new ReturnMessage(true, null);
            //}
            //return new ReturnMessage(false, "you are not logged in.");
        }

        public List<Object> getUsersDetails()
        {
            return db.getUsersDetails();
        }

        public List<SystemUser> getAllUsers()
        {
            return db.getAllSystemUsers();
        }

        public bool removeUser(int userId)
        {
            return db.deleteUser(userId);
        }

        public void sendSystemMessage(string message)
        {
            messageObserver.Update(message);
        }

        public bool removeGame(int gameId)
        {
            foreach (TexasHoldemGame game in texasHoldemGames)
            {
                if (gameId == game.gameId)
                {
                    texasHoldemGames.Remove(game);
                    return true;
                }
            }
            return false;
        }

        public SystemUser register(string userName, string password, string email, string userImage)
        {
            if (userName == null || password == null || email == null || userImage == null || userName.Equals("") || password.Equals("") || email.Equals("") || userImage.Equals(""))
                throw new ArgumentException("Not all parameters were given.");

            SystemUser user = db.getUserByName(userName);
            if (user != null)
                return null;// throw new ArgumentException("User already exists.");
            
            //after a registeration the user stay login
            db.RegisterUser(userName, password, email, userImage);
            //loggedInUsers.Add(user);
            
            //userList.Add(user);
            return db.getUserByName(userName);
        }

        public SystemUser login(string user, string password)
        {
            if (user == null || password == null || user.Equals("") || password.Equals(""))
                throw new ArgumentException("No such user.");

            SystemUser systemUser = db.getUserByName(user);
            if (systemUser == null)
                throw new ArgumentException("No such user.");

            int id = db.Login(user, password);
            if (id == -1)
                throw new InvalidOperationException("Incorrect password");

            return db.getUserById(id);
        }

        public TexasHoldemGame createGame(int gameCreator, string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? minimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague)
        {
            SystemUser user = db.getUserById(gameCreator);
            if (user == null)
                return null;
            League l = null;
            
            if (isLeague.HasValue && isLeague.Value)
                l = getUserLeague(user);

            MustPreferences mustPref;
            if (l != null)
                mustPref = getMustPref(gamePolicy, gamePolicyLimit, buyInPolicy, startingChipsAmount, minimalBet, minPlayers, maxPlayers, isSpectatingAllowed, isLeague, l.minRank, l.maxRank);
            else
                mustPref = getMustPref(gamePolicy, gamePolicyLimit, buyInPolicy, startingChipsAmount, minimalBet, minPlayers, maxPlayers, isSpectatingAllowed, isLeague);


            //this is the callback that is there for when we want to update user rank
            TexasHoldemGame game = new TexasHoldemGame(user, mustPref, userIdDeltaRankMoney => db.EditUserById(userIdDeltaRankMoney[0], null, null, null, null, userIdDeltaRankMoney[2], userIdDeltaRankMoney[1], false), userIdLeaderB => db.EditUserLeaderBoardsById(userIdLeaderB[0], userIdLeaderB[1], userIdLeaderB[2]));
            texasHoldemGames.Add(game);
            return game;
        }

        public List<TexasHoldemGame> getAllGames()
        {
            return new List<TexasHoldemGame>(texasHoldemGames);
        }

        public List<TexasHoldemGame> filterActiveGamesByGamePreferences(MustPreferences pref)
        {
            List<TexasHoldemGame> ans = new List<TexasHoldemGame> { };
            foreach (TexasHoldemGame g in texasHoldemGames)
                if (g.gamePreferences.isContain(pref))
                    ans.Add(g);
            return ans;
        }

        public List<TexasHoldemGame> filterActiveGamesByGamePreferences(string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? minimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague, int minRank, int maxRank)
        {
            MustPreferences mustPref = getMustPref(gamePolicy,gamePolicyLimit,buyInPolicy,startingChipsAmount,minimalBet,minPlayers,maxPlayers,isSpectatingAllowed,isLeague,minRank,maxRank);
            List<TexasHoldemGame> ans = new List<TexasHoldemGame>();
            foreach (TexasHoldemGame game in getAllGames())
                if (game.gamePreferences.isContain(mustPref))
                    ans.Add(game);
            return ans;
        }

        public List<TexasHoldemGame> filterActiveGamesByGamePreferences(string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? minimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague)
        {
            MustPreferences mustPref = getMustPref(gamePolicy, gamePolicyLimit, buyInPolicy, startingChipsAmount, minimalBet, minPlayers, maxPlayers, isSpectatingAllowed, isLeague);
            List<TexasHoldemGame> ans = new List<TexasHoldemGame>();
            foreach (TexasHoldemGame game in getAllGames())
                if (game.gamePreferences.isContain(mustPref))
                    ans.Add(game);
            return ans;
        }


        public List<TexasHoldemGame> filterActiveGamesByPotSize(int? size)
        {
            List<TexasHoldemGame> ans = new List<TexasHoldemGame> { };
            foreach (TexasHoldemGame g in texasHoldemGames)
                if (g.pot <= size)
                    ans.Add(g);
            return ans;
        }

        public ReturnMessage endGame(int gameId)
        {
            foreach (TexasHoldemGame game in texasHoldemGames)
                if (game.gameId == gameId)
                {
                    game.active = false;
                    texasHoldemGames.Remove(game);
                    return new ReturnMessage(true, "");
                }
            return new ReturnMessage(false, "couldn't find the wanted games");
                    
        }

        public List<TexasHoldemGame> filterActiveGamesByPlayerName(string name)
        {
            List<TexasHoldemGame> ans = new List<TexasHoldemGame> ();
            foreach (TexasHoldemGame game in texasHoldemGames)
            {
                foreach (Player p in game.players)
                {
                    if (p != null)
                    {
                        if (db.getUserById(p.systemUserID).name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            ans.Add(game);
                            break;
                        }
                    }
                }
            }

            return ans;
        }

        public ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar, int money, int rank)
        {
            SystemUser user = db.getUserById(userId);
            if (user == null)
                return new ReturnMessage(false, "Could not find the logged user.");

            //Validates attributes.
            if (name.Equals("") || password.Equals(""))
                return new ReturnMessage(false, "Can't change to empty user name or password.");
            if (money < 0)
                return new ReturnMessage(false, "Can't change money to a negative value.");

            user = db.getUserByName(name);
            if (user != null && user.id != userId)
                return new ReturnMessage(false, "Username already exists.");

            user = db.getUserByEmail(email);
            if (user != null && user.id != userId)
                return new ReturnMessage(false, "email already exists.");

            return new ReturnMessage(db.EditUserById(userId, name, password, email, avatar, money, rank, false), "");
        }

        public ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar, int money)
        {
            SystemUser user = db.getUserById(userId);
            if (user == null)
                return new ReturnMessage(false, "Could not find the logged user.");

            //Validates attributes.
            if (name.Equals(""))
                return new ReturnMessage(false, "Can't change to empty user name.");

            if (password.Equals(""))
                password = null;

            if (money < 0)
                return new ReturnMessage(false, "Can't change money to a negative value.");
            
            user = db.getUserByName(name);
            if (user != null && user.id != userId)
                return new ReturnMessage(false, "Username already exists.");
            
            user = db.getUserByEmail(email);
            if (user != null && user.id != userId)
                return new ReturnMessage(false, "email already exists.");
            
            return new ReturnMessage(db.EditUserById(userId, name, password, email, avatar, money, null, false), "");
        }

        public League getUserLeague(SystemUser user)
        {
            foreach (League l in leagues)
            {
                if (l.Users.Contains(user))
                    return l;
            }
            return null;
        }

        public bool userPlay(SystemUser user)
        {
            foreach (TexasHoldemGame game in texasHoldemGames)
            {
                foreach(Player p in game.players)
                {
                    if (p!=null && p.systemUserID == user.id)
                        return true;
                }
            }
            return false;
        }

        public TexasHoldemGame getGameById(int gameId)
        {
            foreach (TexasHoldemGame game in texasHoldemGames)
                if (game.gameId == gameId)
                    return game;
            return null;
        }

        public int numberOfPlayersInGame(TexasHoldemGame thg)
        {
            int counter = 0;
            for (int i = 0; i < thg.players.Length; i++)
            {
                if (thg.players[i] != null)
                    counter++;
            }
            return counter;
        }

        public SystemUser getUserByName(string name)
        {
            return db.getUserByName(name);
        }

        public SystemUser getUserById(int userId)
        {
            return db.getUserById(userId);
        }

        #region game

        public ReturnMessage bet(int gameId, int playerIndex, int coins)
        {
            TexasHoldemGame game = getGameById(gameId);

            return game.bet(game.players[playerIndex], coins);
        }
        public ReturnMessage fold(int gameId, int playerIndex)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game.fold(game.players[playerIndex]);
        }
        public ReturnMessage check(int gameId, int playerIndex)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game.check(game.players[playerIndex]);
        }
        public ReturnMessage call(int gameId, int playerIndex, int minBet)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game.call(game.players[playerIndex], minBet);
        }
        public ReturnMessage playGame(int gameId)
        {
            TexasHoldemGame game = getGameById(gameId);
            game.playGame();
            return new ReturnMessage(true, "");
        }
        public TexasHoldemGame getGameState(int gameId)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game;
        }
        public Player GetPlayer(int gameId, int playerIndex)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game.GetPlayer(playerIndex);
        }
        public Dictionary<int, List<Card>> GetPlayerCards(int gameId, int userId)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game.GetPlayerCards(userId);
        }

        #endregion

        private SystemUser getHighest(List<SystemUser> users)
        {
            int maxRank = -1;
            SystemUser ans = null;
            foreach (SystemUser u in users)
            {
                if (u.rank > maxRank)
                {
                    ans = u;
                    maxRank = u.rank;
                }
            }
            return ans;
        }

        private MustPreferences getMustPref(string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? minimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague)
        {
            return getMustPref(gamePolicy,gamePolicyLimit,buyInPolicy,startingChipsAmount,minimalBet,minPlayers,maxPlayers,isSpectatingAllowed,isLeague,-2,-2);
        }

        private MustPreferences getMustPref(string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? minimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague, int minRank, int maxRank)
        {
            MustPreferences mustPref = null;
            if (isLeague.Value)
                mustPref = new MustPreferences(null, isSpectatingAllowed.Value, minRank, maxRank);
            else
                mustPref = new MustPreferences(null, isSpectatingAllowed.Value);

            

            OptionalPreferences nextPref = null;
            OptionalPreferences temp = null;
            bool found = false;
            //game type policy settings
            GamePolicyDecPref gamePolicyDec = null;
            BuyInPolicyDecPref buyInPolicyPref = buyInPolicy.HasValue ? new BuyInPolicyDecPref(buyInPolicy.Value, null) : null;
            StartingAmountChipsCedPref startingChipsAmountPref = startingChipsAmount.HasValue ? new StartingAmountChipsCedPref(startingChipsAmount.Value, null) : null;
            MinBetDecPref MinimalBetPref = minimalBet.HasValue ? new MinBetDecPref(minimalBet.Value, null) : null;
            MinPlayersDecPref minimalPlayerPref = minPlayers.HasValue ? new MinPlayersDecPref(minPlayers.Value, null) : null;
            MaxPlayersDecPref maximalPlayerPref = maxPlayers.HasValue ? new MaxPlayersDecPref(maxPlayers.Value, null) : null;
            if (gamePolicy != null)
            {
                if (gamePolicy == "none")
                    gamePolicy = "No_Limit";
                GameTypePolicy policy;
                Enum.TryParse(gamePolicy, out policy);
                if (gamePolicyLimit.HasValue)
                {
                    gamePolicyDec = new GamePolicyDecPref(policy, gamePolicyLimit.Value, null);
                }
                else
                {
                    gamePolicyDec = new GamePolicyDecPref(policy, 0, null);
                }
            }
            if (gamePolicyDec != null)
            {
                nextPref = gamePolicyDec;
                found = true;
                temp = nextPref.nextDecPref;
            }

            var optPreferences = new List<OptionalPreferences>();
            optPreferences.Add(gamePolicyDec);
            optPreferences.Add(buyInPolicyPref);
            optPreferences.Add(startingChipsAmountPref);
            optPreferences.Add(MinimalBetPref);
            optPreferences.Add(minimalPlayerPref);
            optPreferences.Add(maximalPlayerPref);

            var index = optPreferences.Count - 1;
            OptionalPreferences iterator = null;
            while(index >= 0)
            {
                if (optPreferences[index] == null){
                    index--;
                    continue;
                }
                else if (iterator == null)
                {
                    iterator = optPreferences[index];
                }
                else
                {
                    optPreferences[index].nextDecPref = iterator;
                    iterator = optPreferences[index];
                }
                index--;
                continue;
            }

            mustPref.firstDecPref = iterator;

            /*

            //buy in policy settings
            if (buyInPolicyPref != null)
            {
                if (found)
                {

                }
                else
                {
                    found = true;
                    nextPref = buyInPolicyPref;
                    temp = nextPref.nextDecPref;
                }
                
                
            }

            if (startingChipsAmountPref != null)
            {
                nextPref = startingChipsAmountPref;
                nextPref = nextPref.nextDecPref;
            }

            if (MinimalBetPref != null)
            {
                nextPref = MinimalBetPref;
                nextPref = nextPref.nextDecPref;
            }

            if (minimalPlayerPref != null)
            {
                nextPref = minimalPlayerPref;
                nextPref = nextPref.nextDecPref;
            }

            if (maximalPlayerPref != null)
            {
                nextPref = maximalPlayerPref;
                nextPref = nextPref.nextDecPref;
            }*/

            return mustPref;
        }

        public List<object> getLeaderboardsByParam(string param)
        {
            return db.getLeaderboardsByParam(param);
        }

        public object addMessage(int gameId, int userId, string messageText)
        {
            var game = getGameById(gameId);
            
            var user = getUserById(userId);
            if (game == null || user == null)
            {
                return null;
            }
            
            var playerName = String.Empty;
            /*
            // If a player tries to send a message before sitting down, send the message as anonymous.
            if (playerIndex == -1)
            {
                var rand = new Random();
                playerName = "Anonymous" + rand.Next(1, 9999);
            }
            else
            {
                playerName = game.players[playerIndex].name;
            }*/

            game.addMessage(user, messageText);

            return null;
        }
    }
}
