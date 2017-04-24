using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.User;

namespace Backend.Game
{
    public class League
    {
        private List<SystemUser> Users;
        private List<LeagueTexasHoldemGame> Games;
        public int minRank { get; set; }
        public int maxRank { get; set; }
        public Guid leagueId { get; }
        public string leagueName { get; set; }
        public League(int minRank, int maxRank, string name)
        {
            this.minRank = minRank;
            this.maxRank = maxRank;
            this.leagueName = name;
            this.Users = new List<SystemUser>();
            this.Games = new List<LeagueTexasHoldemGame>();
            leagueId = Guid.NewGuid();
        }

        public ReturnMessage addUser(SystemUser u)
        {
            if (Users.Contains(u))
            {
                return new ReturnMessage(false, String.Format("User {0} is already in league {1}.", u.id, leagueId));
            }

            Users.Add(u);
            return new ReturnMessage(true, String.Format("User {0} was added to league {1}.", u.id, leagueId));
        }

        public ReturnMessage removeUser(SystemUser u)
        {
            if (!Users.Contains(u))
            {
                return new ReturnMessage(false, String.Format("User {0} is not in league {1}.", u.id, leagueId));
            }

            Users.Remove(u);
            return new ReturnMessage(true, String.Format("User {0} was added to league {1}.", u.id, leagueId));
        }

        public Boolean isUserInLeague(SystemUser u)
        {
            return Users.Contains(u);
        }

        public ReturnMessage addGame(LeagueTexasHoldemGame g)
        {
            if (Games.Contains(g))
            {
                return new ReturnMessage(false, String.Format("Game {0} is already in league {1}.", g.gameId, leagueId));
            }

            Games.Add(g);
            return new ReturnMessage(true, String.Format("Game {0} was added to league {1}.", g.gameId, leagueId));
        }

        public ReturnMessage removeGame(LeagueTexasHoldemGame g)
        {
            if (!Games.Contains(g))
            {
                return new ReturnMessage(false, String.Format("Game {0} is not in league {1}.", g.gameId, leagueId));
            }

            Games.Remove(g);
            return new ReturnMessage(true, String.Format("Game {0} was added to league {1}.", g.gameId, leagueId));
        }
    }
}
