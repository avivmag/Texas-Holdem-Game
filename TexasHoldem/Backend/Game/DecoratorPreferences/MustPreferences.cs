using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class MustPreferences : DecoratorPreferencesInterface
    {
        private int minRank;
        private int maxRank;
        public bool isSpectateAllowed { get; set; }
        public bool isLeague { get; set; }
        public OptionalPreferences firstDecPref { get; }

        public MustPreferences(OptionalPreferences firstDecPref, bool isSpectateAllowed)
        {
            this.firstDecPref = firstDecPref;
            this.isSpectateAllowed = isSpectateAllowed;
            minRank = -2;
            maxRank = -2;
            isLeague = false;
        }

        public MustPreferences(OptionalPreferences firstDecPref, bool isSpectateAllowed, int minRank, int maxRank)
        {
            this.firstDecPref = firstDecPref;
            this.isSpectateAllowed = isSpectateAllowed;
            this.minRank = minRank;
            this.maxRank = maxRank;
            isLeague = true;
        }

        public ReturnMessage canPerformUserActions(TexasHoldemGame game, Player p, SystemUser user, string action)
        {
            switch (action)
            {
                case "join":
                    if (isLeague)
                    {
                        //if the user stands in the rank or he is a new player
                        if ((user.rank >= minRank && user.rank <= maxRank) || user.newPlayer)
                            return firstDecPref.canPerformUserActions(game, p, user, action);
                        else
                            return new ReturnMessage(false, "The user can't join to this league game because his rank not matching.");
                    }
                    else
                    {
                        if (firstDecPref != null)
                            return firstDecPref.canPerformUserActions(game, p, user, action);
                        else
                            return new ReturnMessage(true,"");
                    }

                case "spectate":
                    if (isSpectateAllowed)
                        return new ReturnMessage(true, "");
                    else
                        return new ReturnMessage(false, "The game can't be spectate");

                case "leave":
                    if (firstDecPref != null)
                        return firstDecPref.canPerformUserActions(game, p, user, action);
                    else
                        return new ReturnMessage(true, "");
                default:
                    return new ReturnMessage(false, "Wrong input to the canPerformUserActions.");
            }
        }

        public ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, int amount, string action)
        {
            if (firstDecPref != null)
                return firstDecPref.canPerformGameActions(game, user, amount, action);
            else
                return new ReturnMessage(true, "");
        }

        public bool isContain(DecoratorPreferencesInterface pref)
        {
            if (pref.GetType() != GetType())
                return false;
            MustPreferences mustPref = (MustPreferences)pref;
            if (mustPref.isLeague == isLeague && mustPref.isSpectateAllowed == isSpectateAllowed)
                if (firstDecPref != null)
                    return firstDecPref.isContain(mustPref.firstDecPref);
                else
                    return true;
            else
                return false;
        }
    }
}
