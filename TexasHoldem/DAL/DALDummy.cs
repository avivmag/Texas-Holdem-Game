using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using System.Linq;
using Backend;
using Backend.Game.DecoratorPreferences;
using static Backend.Game.DecoratorPreferences.GamePolicyDecPref;

namespace DAL
{
    public class DALDummy : DALInterface
    {
        private List<SystemUser> userList;
        private List<SystemUser> loggedInUserDummies;
        private List<Player> playerDummies;
        private List<TexasHoldemGame> gameDummies;
        

        public DALDummy()
        {
            //leagues = new List<League>();
            //leagues.Add(new League(0, 1000, "Starter League"));
            //leagues.Add(new League(1000, 2000, "Experienced League"));

            userList = new List<SystemUser>
            {
                new SystemUser("Hadas", "Aa123456", "email0", "image0", 1000),
                new SystemUser("Gili", "123123", "email1", "image1", 0),
                new SystemUser("Or", "111111", "email2", "image2", 700),
                new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500)
            };

            int j = 10;
            for (int i = 0; i < 4; i++)
            {
                userList[i].rank = j;
                userList[i].newPlayer = false;
                j += 5;
                userList[i].id = i;
            }
                
            
            

            loggedInUserDummies = new List<SystemUser>();
            //setting the games
            //pref order: mustpref(spectate,league)->game type , buy in policy, starting chips, minimal bet, minimum players, maximum players.
            gameDummies = new List<TexasHoldemGame>
            {
                //regular games
                new TexasHoldemGame(userList[0],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (9,null) ))))),true)),
                new TexasHoldemGame(userList[0],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (9,null) ))))),false)),
                new TexasHoldemGame(userList[1],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),true)),
                new TexasHoldemGame(userList[1],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false)),
                new TexasHoldemGame(userList[2],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false)),
                new TexasHoldemGame(userList[2],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                                                                    new MaxPlayersDecPref (2,null) ))))),false)),
                //league games
                //new TexasHoldemGame(userList[3],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                //                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                //                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                //                                                    new MaxPlayersDecPref (2,null) ))))),false,l.minRank,l.maxRank)),
                //new TexasHoldemGame(userList[3],new MustPreferences(new GamePolicyDecPref(GameTypePolicy.No_Limit,0,
                //                                                    new BuyInPolicyDecPref(100,new StartingAmountChipsCedPref(500,
                //                                                    new MinBetDecPref(20,new MinPlayersDecPref(2,
                //                                                    new MaxPlayersDecPref (2,null) ))))),false,l.minRank,l.maxRank))
                //new TexasHoldemGame(userDummies[0], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true)),
                //new TexasHoldemGame(userDummies[0], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, false)),
                //new TexasHoldemGame(userDummies[1], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, true)),
                //new TexasHoldemGame(userDummies[1], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false)),
                //new TexasHoldemGame(userDummies[3], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 0, 1000)),
                //new TexasHoldemGame(userDummies[3], new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false, 1000, 2000))
            };
            //for (int i = 0; i < 6; i++)
            //{
            //    gameDummies[i].flop = new List<Card>();
            //    gameDummies[i].flop.Add(new Card(Card.cardType.club, 5));
            //    gameDummies[i].flop.Add(new Card(Card.cardType.diamond, 6));
            //    gameDummies[i].flop.Add(new Card(Card.cardType.heart, 7));
            //    gameDummies[i].gameId = i;
            //}

            playerDummies = new List<Player>
            {
                new Player(0, "", 100, userList[0].rank),
                new Player(1, "", 0, userList[1].rank),
                new Player(2, "", 200, userList[2].rank),
                new Player(3, "", 200, userList[3].rank)
            };
            for (int i = 0; i < 4; i++)
                playerDummies[i].systemUserID = i;
        }

        public TexasHoldemGame getGameById(int gameID)
        {
            for (int i = 0; i < gameDummies.Count(); i++)
                if (gameDummies[i].gameId == gameID)
                    return gameDummies[i];
            return null;
        }

        public SystemUser getUserById(int userID)
        {
            for (int i = 0; i < userList.Count; i++)
                if (userList[i].id == userID)
                    return userList[i];
            return null;
        }

        public SystemUser getUserByName(string name)
        {
            for (int i = 0; i < userList.Count; i++)
                if (userList[i].name.Equals(name))
                    return userList[i];
            return null;
        }

        public List<TexasHoldemGame> getAllGames()
        {
            var gameList = gameDummies.Cast<TexasHoldemGame>().ToList();
            return gameList;
        }

        public List<SystemUser> getAllUsers()
        {
            return userList.Cast<SystemUser>().ToList();
        }

        public void editUser(SystemUser user)
        {
            if (user.id < userList.Count)
                userList[user.id] = user;
        }
		
		public ReturnMessage registerUser(SystemUser user)
		{
			foreach (SystemUser systemUser in userList)
				if (systemUser.name.Equals(user.name))
					return new ReturnMessage(false, "This user name is already taken.");

			userList.Add(user);
			return new ReturnMessage(true, null);
		}

		public ReturnMessage logUser(string user)
		{
			foreach (SystemUser systemUser in loggedInUserDummies)
				if (systemUser.name.Equals(user))
					return new ReturnMessage(false, "You are already logged in to the system");

			int i = 0;
			for (; i < userList.Count; i++)
				if (userList[i].name.Equals(user))
					break;

			if (i == userList.Count)
				return new ReturnMessage(false, "You must be registered before attempting to log in.");

			loggedInUserDummies.Add(getUserByName(user));
			return new ReturnMessage(true, null);
		}
		public ReturnMessage logOutUser(string user)
		{
			foreach (SystemUser systemUser in loggedInUserDummies)
				if (systemUser.name.Equals(user))
				{
					loggedInUserDummies.Remove(systemUser);
					return new ReturnMessage(true, null);
				}
			
			return new ReturnMessage(false, "you are not logged in");
		}
		
        //public ReturnMessage addLeague(int minRank, int maxRank, string name)
        //{
        //    leagues.Add(new League(minRank, maxRank, name));

        //    return new ReturnMessage(true, null);
        //}

        //public ReturnMessage removeLeague(Guid leagueId)
        //{
        //    var league = leagues.Where(l => l.leagueId == leagueId).SingleOrDefault();

        //    if(league == null)
        //    {
        //        return new ReturnMessage(false, String.Format("Cannot Remove league. no such league exists with Id {0}", leagueId));
        //    }
        //    leagues.Remove(league);

        //    return new ReturnMessage(true, null);
        //}

        
        //public List<League> getAllLeagues()
        //{
        //    return leagues;
        //}

        //public ReturnMessage setLeagueCriteria(int minRank, int maxRank, string leagueName, Guid leagueId, int userId)
        //{
        //    return null;
        //}

        public ReturnMessage addGame(TexasHoldemGame game)
        {
            return new ReturnMessage(true, null);
        }

        public int getHighestUserId()
        {
            var userId = getAllUsers().OrderByDescending(u => u.money).First().id;

            return userId;
        }
	}
}
