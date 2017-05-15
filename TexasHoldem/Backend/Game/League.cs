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
        public int minRank { get; set; }
        public int maxRank { get; set; }
        public List<SystemUser> Users { get; }

        public League()
        {
            minRank = Int32.MaxValue;
            maxRank = -2;
            Users = new List<SystemUser>();
        }

        public ReturnMessage addUser(SystemUser u)
        {
            if (Users.Contains(u))
            {
                return new ReturnMessage(false, String.Format("User {0} is already in this league.", u.id));
            }

            Users.Add(u);
            if (minRank > u.rank)
                this.minRank = u.rank;
            if (maxRank < u.rank)
                this.maxRank = u.rank;
            return new ReturnMessage(true, String.Format("User {0} was added to the league.", u.id));
        }

        public ReturnMessage removeUser(SystemUser u)
        {
            if (!Users.Contains(u))
            {
                return new ReturnMessage(false, String.Format("User {0} is not in the league.", u.id));
            }

            Users.Remove(u);
            return new ReturnMessage(true, String.Format("User {0} was removed from the league.", u.id));
        }

        public void removeAllUsers()
        {
            this.Users.Clear();
        }

        public Boolean isUserInLeague(SystemUser u)
        {
            return Users.Contains(u);
        }
    }
}
