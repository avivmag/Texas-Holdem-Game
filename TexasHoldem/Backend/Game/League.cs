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
        public int minRank { get; }
        public int maxRank { get; }
        public Guid leagueId { get; }
        public League(int minRank, int maxRank)
        {
            this.minRank = minRank;
            this.maxRank = maxRank;
            this.Users = new List<SystemUser>();
            this.Games = new List<LeagueTexasHoldemGame>();
            leagueId = new Guid();
        }

        public Message addUser(SystemUser u)
        {
            if (Users.Contains(u))
            {
                return new Message(false, String.Format("User {0} is already in league {1}.", u.id, leagueId));
            }

            Users.Add(u);
            return new Message(true, String.Format("User {0} was added to league {1}.", u.id, leagueId));
        }

        public Message removeUser(SystemUser u)
        {
            if (!Users.Contains(u))
            {
                return new Message(false, String.Format("User {0} is not in league {1}.", u.id, leagueId));
            }

            Users.Remove(u);
            return new Message(true, String.Format("User {0} was added to league {1}.", u.id, leagueId));
        }

        public Boolean isUserInLeague(SystemUser u)
        {
            return Users.Contains(u);
        }

        public Message addGame(LeagueTexasHoldemGame g)
        {
            if (Games.Contains(g))
            {
                return new Message(false, String.Format("Game {0} is already in league {1}.", g.id, leagueId));
            }

            Games.Add(g);
            return new Message(true, String.Format("Game {0} was added to league {1}.", g.id, leagueId));
        }

        public Message removeGame(LeagueTexasHoldemGame g)
        {
            if (!Games.Contains(g))
            {
                return new Message(false, String.Format("Game {0} is not in league {1}.", g.id, leagueId));
            }

            Games.Remove(g);
            return new Message(true, String.Format("Game {0} was added to league {1}.", g.id, leagueId));
        }
    }
}
