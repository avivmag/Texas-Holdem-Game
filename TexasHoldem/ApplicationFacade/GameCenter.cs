using System;
using System.Collections.Generic;
using Backend;
using Backend.Game;
using Backend.Game.DecoratorPreferences;
using Backend.User;
using DAL;
using static Backend.Game.DecoratorPreferences.GamePolicyDecPref;

namespace ApplicationFacade
{
	public class GameCenter : Backend.Messages.Notification
	{
		public List<TexasHoldemGame> texasHoldemGames { get; set; }
        public List<League> leagues { get; set; }
        public List<SystemUser> loggedInUsers { get; set; }
        private DALDummy dal;
        private static GameCenter center;

        private GameCenter()
        {
            texasHoldemGames = new List<TexasHoldemGame>();
            leagues = new List<League>();
            loggedInUsers = new List<SystemUser>();
            dal = new DALDummy();
        }

        public static GameCenter getGameCenter()
        {
            if (center == null)
                center = new GameCenter();
            return center;
        }

        public void shutDown()
        {
            texasHoldemGames.Clear();
            leagues.Clear();
            loggedInUsers.Clear();
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

        public object logout(int userId)
        {
            SystemUser systemUser = getUserById(userId);
            if (systemUser == null)
                return new ReturnMessage(false, "User does not exists");

            if (systemUser.spectatingGame.Count > 0)
                return new ReturnMessage(false, "you are active in some games as spectator, leave them and then log out.");

            if (userPlay(systemUser))
            {
                return new ReturnMessage(false, "you are active in some games as a player, leave them and then log out.");
            }

            ReturnMessage m = dal.logOutUser(systemUser.name);
            loggedInUsers.Remove(systemUser);
            return m;
        }

        public ReturnMessage register(string user, string password, string email, string userImage)
        {
            if (user == null || password == null || email == null || userImage == null || user.Equals("") || password.Equals("") || email.Equals("") || userImage.Equals(""))
                return new ReturnMessage(false, "all attributes must be filled.");

            SystemUser systemUser = dal.getUserByName(user);
            if (systemUser != null)
                return new ReturnMessage(false, "user name or email already taken");

            //creating the user.
            systemUser = new SystemUser(user, password, email, userImage, 0);
            //after a registeration the user stay login
            loggedInUsers.Add(systemUser);
            //adding the user to the db.
            return dal.registerUser(systemUser);
        }

        public ReturnMessage login(string user, string password)
        {
            if (user == null || password == null || user.Equals("") || password.Equals(""))
                return new ReturnMessage(false, "all attributes must be filled.");

            SystemUser systemUser = dal.getUserByName(user);
            if (systemUser == null)
                return new ReturnMessage(false, "user does not exists.");

            foreach (SystemUser u in loggedInUsers)
                if (systemUser.id == u.id)
                    return new ReturnMessage(false, "The user is already logged in");

            if (systemUser.password.Equals(password))
            {
                loggedInUsers.Add(systemUser);
                return new ReturnMessage(true, "");
            }
            else
                return new ReturnMessage(false, "Incorrect password mismatched.");
        }

        public TexasHoldemGame createGame(int gameCreatorId, MustPreferences pref)
        {
            SystemUser user = getUserById(gameCreatorId);
            if (user == null)
                return null;
            
            TexasHoldemGame game = new TexasHoldemGame(user, pref);
            ReturnMessage m = game.gamePreferences.canPerformUserActions(game, user, "create");

            if (m.success)
                dal.addGame(game);
            return game;
        }

        public TexasHoldemGame createGame(int gameCreator, string gamePolicy, int? gamePolicyLimit, int? buyInPolicy, int? startingChipsAmount, int? minimalBet, int? minPlayers, int? maxPlayers, bool? isSpectatingAllowed, bool? isLeague)
        {
            SystemUser user = getUserById(gameCreator);
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



            TexasHoldemGame game = new TexasHoldemGame(user, mustPref);
            texasHoldemGames.Add(game);
            dal.addGame(game);
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
            foreach (TexasHoldemGame game in dal.getAllGames())
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
                        if (getUserById(p.systemUserID).name.Equals(name, StringComparison.OrdinalIgnoreCase))
                        {
                            ans.Add(game);
                            break;
                        }
                    }
                }
            }

            return ans;
        }

        public ReturnMessage editUserProfile(int userId, string name, string password, string email, string avatar, int money)
        {
            SystemUser user = getUserById(userId);
            if (user == null)
                return new ReturnMessage(false, "Could not find the logged user.");
            List<SystemUser> allUsers = dal.getAllUsers();

            //Validates attributes.
            if (name.Equals("") || password.Equals(""))
                return new ReturnMessage(false, "Can't change to empty user name or password.");
            if (money<0)
                return new ReturnMessage(false, "Can't change money to a negative value.");

            //Check that attributes are not already exists.
            foreach (SystemUser u in allUsers)
                if (u.id != userId && (u.name.Equals(name, StringComparison.OrdinalIgnoreCase) || u.email.Equals(email, StringComparison.OrdinalIgnoreCase))) //comparing two passwords including cases i.e AbC = aBc
                    return new ReturnMessage(false, "Username or email already exists.");

            //changes the attributes
            user.name = name;
            user.password = password;
            user.email = email;
            user.userImage = avatar;
            dal.editUser(user);
            return new ReturnMessage(true,"");
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

        public SystemUser getUserByName(string name)
        {
            return dal.getUserByName(name);
        }

        public SystemUser getUserById(int userId)
        {
            foreach (SystemUser user in loggedInUsers)
                if (user.id == userId)
                    return user;
            return null;
        }

        #region game
        public ReturnMessage bet(int gameId, int playerIndex, int coins)
        {
            TexasHoldemGame game = getGameById(gameId);
            //Player player = null;
            //foreach (Player p in game.players)
            //    if (p.systemUserID == playerUserId)
            //        player = p;
            //if (player == null)
            //    return new ReturnMessage(false, "could not find the player");
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
        public TexasHoldemGame getGameState(int gameId)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game;
        }
        public ReturnMessage ChoosePlayerSeat(int gameId, int playerIndex)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game.ChoosePlayerSeat(playerIndex);
        }
        public Player GetPlayer(int gameId, int playerIndex)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game.GetPlayer(playerIndex);
        }
        public Card[] GetPlayerCards(int gameId, int playerIndex)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game.GetPlayerCards(playerIndex);
        }
        public IDictionary<int, Card[]> GetShowOff(int gameId)
        {
            TexasHoldemGame game = getGameById(gameId);
            return game.GetShowOff();
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

            //game type policy settings
            GamePolicyDecPref gamePolicyDec = null;
            if (gamePolicy != null)
            {
                GameTypePolicy policy;
                Enum.TryParse(gamePolicy, out policy);
                if (gamePolicyLimit.HasValue)
                {
                    gamePolicyDec = new GamePolicyDecPref(policy, gamePolicyLimit.Value, null);
                }
            }
            if (gamePolicyDec != null)
            {
                nextPref = gamePolicyDec;
                nextPref = nextPref.nextDecPref;
            }

            //buy in policy settings
            BuyInPolicyDecPref buyInPolicyPref = buyInPolicy.HasValue ? new BuyInPolicyDecPref(buyInPolicy.Value, null) : null;
            if (buyInPolicyPref != null)
            {
                nextPref = buyInPolicyPref;
                nextPref = nextPref.nextDecPref;
            }

            StartingAmountChipsCedPref startingChipsAmountPref = startingChipsAmount.HasValue ? new StartingAmountChipsCedPref(startingChipsAmount.Value, null) : null;
            if (startingChipsAmountPref != null)
            {
                nextPref = startingChipsAmountPref;
                nextPref = nextPref.nextDecPref;
            }

            MinBetDecPref MinimalBetPref = minimalBet.HasValue ? new MinBetDecPref(minimalBet.Value, null) : null;
            if (MinimalBetPref != null)
            {
                nextPref = MinimalBetPref;
                nextPref = nextPref.nextDecPref;
            }

            MinPlayersDecPref minimalPlayerPref = minPlayers.HasValue ? new MinPlayersDecPref(minPlayers.Value, null) : null;
            if (minimalPlayerPref != null)
            {
                nextPref = minimalPlayerPref;
                nextPref = nextPref.nextDecPref;
            }

            MaxPlayersDecPref maximalPlayerPref = maxPlayers.HasValue ? new MaxPlayersDecPref(maxPlayers.Value, null) : null;
            if (maximalPlayerPref != null)
            {
                nextPref = maximalPlayerPref;
                nextPref = nextPref.nextDecPref;
            }
            return mustPref;
        }


        //public TexasHoldemGame createRegularGame(SystemUser user, GamePreferences preferences)
        //{
        //	var game = new Game.TexasHoldemGame(user, preferences);
        //          texasHoldemGames.Add(game);
        //          return game;
        //      }
    }
}
