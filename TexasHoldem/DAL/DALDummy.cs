using Backend.Game;
using Backend.User;
using System.Collections.Generic;
using System.Linq;
using Backend;
using System;
using Backend.System;

namespace DAL
{
    public class DALDummy : DALInterface
    {
        private List<SystemUser> userDummies;
		private List<SystemUser> loggedInUserDummies;
        private List<League> leagues;
		private List<Player> playerDummies;
        private List<Spectator> spectatorDummies;
        private List<TexasHoldemGame> gameDummies;

        public DALDummy()
        {
            leagues = new List<League>();
            leagues.Add(new League(0, 1000));
            leagues.Add(new League(1000, 2000));
            
                userDummies = new List<SystemUser>
            {
                new SystemUser("Hadas", "Aa123456", "email0", "image0", 1000),
                new SystemUser("Gili", "123123", "email1", "image1", 0),
                new SystemUser("Or", "111111", "email2", "image2", 700),
                new SystemUser("Aviv", "Aa123456", "email3", "image3", 1500)
            };

            // Should call GameCenter.Maintain Leagues right about now.

            for (int i = 0; i < 4; i++)
                userDummies[i].id = i;

			loggedInUserDummies = new List<SystemUser>();
            gameDummies = new List<TexasHoldemGame>
            {
                new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, true)),
                new TexasHoldemGame(0, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 9, false)),
                new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, true)),
                new TexasHoldemGame(1, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false)),
                new LeagueTexasHoldemGame(3, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false), leagues[0]),
                new LeagueTexasHoldemGame(3, new GamePreferences(GamePreferences.GameTypePolicy.no_limit, 100, 500, 20, 2, 2, false), leagues[1])
            };
            for (int i = 0; i < 6; i++)
                gameDummies[i].id = i;

            playerDummies = new List<Player>
            {
                new Player(0, 100, userDummies[0].rank),
                new Player(1, 0, userDummies[1].rank),
                new Player(2, 200, userDummies[2].rank),
                new Player(3, 200, userDummies[3].rank)
            };
            for (int i = 0; i < 4; i++)
                playerDummies[i].id = i;

        }

        public TexasHoldemGame getGameById(int gameID)
        {
            for (int i = 0; i < gameDummies.Count(); i++)
                if (gameDummies[i].id == gameID)
                    return gameDummies[i];
            return null;
        }

		public SystemUser getUserById(int userID)
		{
			for (int i = 0; i < userDummies.Count; i++)
				if (userDummies[i].id == userID)
					return userDummies[i];
			return null;
		}

		public SystemUser getUserByName(string name)
		{
			for (int i = 0; i < userDummies.Count; i++)
				if (userDummies[i].name.Equals(name))
					return userDummies[i];
			return null;
		}

		public List<TexasHoldemGame> getAllGames()
        {
            var gameList = gameDummies.Cast<TexasHoldemGame>().ToList();
            return gameList;
        }

        public List<SystemUser> getAllUsers()
        {
            return userDummies.Cast<SystemUser>().ToList();
        }

        public void editUser(SystemUser user)
        {
            if (user.id < userDummies.Count)
                userDummies[user.id - 1] = user;
        }

		public Message registerUser(SystemUser user)
		{
			foreach (SystemUser systemUser in userDummies)
				if (systemUser.name.Equals(user.name))
					return new Message(false, "This user name is already taken.");

			userDummies.Add(user);
			return new Message(true, null);
		}

		public Message logUser(string user)
		{
			foreach (SystemUser systemUser in loggedInUserDummies)
				if (systemUser.name.Equals(user))
					return new Message(false, "You are already logged in to the system");

			int i = 0;
			for (; i < userDummies.Count; i++)
				if (userDummies[i].name.Equals(user))
					break;

			if (i == userDummies.Count)
				return new Message(false, "You must be registered before attempting to log in.");

			loggedInUserDummies.Add(getUserByName(user));
			return new Message(true, null);
		}
		public Message logOutUser(string user)
		{
			foreach (SystemUser systemUser in loggedInUserDummies)
				if (systemUser.name.Equals(user))
				{
					loggedInUserDummies.Remove(systemUser);
					return new Message(true, null);
				}
			
			return new Message(false, "you are not logged in");
		}

        public Message addGame(TexasHoldemGame game)
        {
            return new Message(true, null);
        }
	}
}
