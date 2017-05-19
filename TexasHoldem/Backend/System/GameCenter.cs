using System;
using System.Collections.Generic;
using Backend.Game;
using Backend.User;

namespace Backend.System
{
	public class GameCenter : Messages.Notification
	{
		public List<TexasHoldemGame> texasHoldemGames { get; set; }
        public List<League> leagues { get; set; }
        public List<SystemUser> loggedInUsers { get; set; }
        private static GameCenter center;

        private GameCenter()
        {
            texasHoldemGames = new List<TexasHoldemGame>();
            leagues = new List<League>();
        }

        public static GameCenter getGameCenter()
        {
            if (center == null)
                center = new GameCenter();
            return center;
        }

		public TexasHoldemGame createRegularGame(SystemUser user, GamePreferences preferences)
		{
			var game = new Game.TexasHoldemGame(user, preferences);
            texasHoldemGames.Add(game);
            return game;
        }

        // Maintain leagues for players. Should be invoked once a week.
        public void maintainLeagues(List<SystemUser> users)
        {
            int numOfUsers = users.Count;

            if (numOfUsers < 2)
            {
                League l = new League();
                foreach (SystemUser user in users)
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
                        SystemUser currHighestRankUser = getHighest(users);
                        if (currHighestRankUser != null)
                        {
                            users.Remove(currHighestRankUser);
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
                    if (p.systemUserID == user.id)
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

        public SystemUser getUserById(int userId)
        {
            foreach (SystemUser user in loggedInUsers)
                if (user.id == userId)
                    return user;
            return null;
        }

        private ReturnMessage raiseBet(int gameId, int playerId, int coins)
        {
            // I am too tired right now, but I think you've got the idea,
            // now the gamecenter will find the game by gameId and at the bet, blah blah
            // and return if it succeded or not
            // be aware the logic seats in the entities themself

            return null;
        }
    }
}
