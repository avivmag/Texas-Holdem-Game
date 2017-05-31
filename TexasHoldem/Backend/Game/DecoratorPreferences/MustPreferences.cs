using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class MustPreferences : DecoratorPreferencesInterface
    {
        private int minRank;
        private int maxRank;
        public bool isSpectateAllowed { get; set; }
        public bool isLeague { get; set; }
        public OptionalPreferences firstDecPref { get; set; }

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

        public ReturnMessage canPerformUserActions(TexasHoldemGame game, SystemUser user, string action)
        {
            switch (action)
            {
                case "create":
                    if ((isLeague && minRank >= 0 && maxRank >= 0 && maxRank >= minRank) || !isLeague)
                    {
                        if (firstDecPref != null)
                            return firstDecPref.canPerformUserActions(game, user, "create");
                        else
                            return new ReturnMessage(true, "");
                    }
                    else
                        return new ReturnMessage(false, "The attributes of the league are not matching");

                case "join":
                    if (isLeague)
                    {
                        if (firstDecPref != null)
                        {
                            //if the user stands in the rank or he is a new player
                            if ((user.rank >= minRank && user.rank <= maxRank) || user.newPlayer)
                                return firstDecPref.canPerformUserActions(game, user, action);
                            else
                                return new ReturnMessage(false, "The user can't join to this league game because his rank not matching.");

                        }
                        else
                        {
                            return new ReturnMessage(true, "");
                        }
                    }
                    else
                    {
                        if (firstDecPref != null)
                            return firstDecPref.canPerformUserActions(game, user, action);
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
                        return firstDecPref.canPerformUserActions(game, user, action);
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
                if (mustPref.firstDecPref != null)
                    return firstDecPref.isContain(mustPref.firstDecPref);
                else
                    return true;
            else
                return false;
        }

        public OptionalPreferences getOptionalPref(OptionalPreferences wantedPref)
        {
            OptionalPreferences temp = firstDecPref;
            while (temp != null)
            {
                if (temp.GetType() == wantedPref.GetType())
                    return temp;
                temp = temp.nextDecPref;
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format("isSpectateAllowed: {0}, isLeague: {1}, {2}", isSpectateAllowed, isLeague, firstDecPref);
        }
    }
}
