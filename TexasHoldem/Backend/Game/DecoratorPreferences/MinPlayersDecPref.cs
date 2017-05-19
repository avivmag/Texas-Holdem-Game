using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class MinPlayersDecPref : OptionalPreferences
    {
        private int minPlayers;

        public MinPlayersDecPref(int minPlayers, OptionalPreferences nextDecPref): base (nextDecPref)
        {
            this.minPlayers = minPlayers;
        }

        public override ReturnMessage canPerformUserActions(TexasHoldemGame game, SystemUser user, string action)
        {
            //TODO: leave PLayer?
            switch (action)
            {
                case "create":
                    if (minPlayers >= 2 && minPlayers<=9)
                        if (nextDecPref != null)
                            return nextDecPref.canPerformUserActions(game, user, action);
                        else
                            return new ReturnMessage(true, "");
                    else
                        return
                            new ReturnMessage(false, "Minimal player must be between 2 and 9");
                default:
                    if (nextDecPref != null)
                        return nextDecPref.canPerformUserActions(game, user, action);
                    return new ReturnMessage(true, "");
            }
        }

        public override ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, int amount, string action)
        {
            if (nextDecPref != null)
                return nextDecPref.canPerformGameActions(game, user, amount, action);
            else
                return new ReturnMessage(true, "");
        }

        public override bool isContain(DecoratorPreferencesInterface pref)
        {
            if (pref.GetType() != typeof(OptionalPreferences))
                return false;
            OptionalPreferences opPref = ((OptionalPreferences)pref);
            MinPlayersDecPref matchingPref = (MinPlayersDecPref)getMatchingOptionalPref(opPref);
            //if we found matchig optinal pref and he have the same policy
            if (matchingPref != null && matchingPref.minPlayers == minPlayers)
                //if we still need to check the rest of the chain
                if (nextDecPref != null)
                    //return its result
                    return nextDecPref.isContain(pref);
                //if we don't have anything else to check return true.
                else return true;
            //if we couldent found or if we found pref with different value.
            else
                return false;
        }
    }
}
