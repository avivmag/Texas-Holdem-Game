using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class MustPreferences : DecoratorPreferencesInterface
    {
        private int minRank;
        private int maxRank;
        public bool isSpectateAllowed { get; set; }
        public bool isLeague { get; set; }
        private DecoratorPreferencesInterface firstDecPref;

        public MustPreferences(DecoratorPreferencesInterface firstDecPref, bool isSpectateAllowed, bool isLeague, int minRank, int maxRank)
        {
            this.firstDecPref = firstDecPref;
            this.isSpectateAllowed = isSpectateAllowed;
            this.isLeague = isLeague;
            this.minRank = minRank;
            this.maxRank = maxRank;
        }

        public ReturnMessage canPerformUserActions(Player p, SystemUser user, string action)
        {
            switch (action)
            {
                case "join":
                    if (isLeague)
                    {//if the user stands in the rank or he is a new player
                        if ((user.rank >= minRank && user.rank <= maxRank) || user.newPlayer)
                            return firstDecPref.canPerformUserActions(p, user, action);
                        else
                            return new ReturnMessage(false, "The user can't join to this league game because his rank not matching.");
                    }
                    else
                    {
                        return firstDecPref.canPerformUserActions(p, user, action);
                    }

                case "spectate":
                    if (isSpectateAllowed)
                        return new ReturnMessage(true, "");
                    else
                        return new ReturnMessage(false, "The game can't be spectate");
                
                    //case "leave":
                    //    return null;
                default:
                    return null;
            }
        }

        public ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, string action)
            {
            switch (action)
            {
                case "Bet":
                    return firstDecPref.canPerformGameActions(game, user, action);

                case "Raise":
                    return firstDecPref.canPerformGameActions(game, user, action);
                default:
                    return null;
            }
        }

        public bool isContain(MustPreferences pref)
        {
            if (pref.isLeague == isLeague && pref.isSpectateAllowed == isSpectateAllowed)
                return firstDecPref.isContain(pref);
            return false;
        }
    }
}
