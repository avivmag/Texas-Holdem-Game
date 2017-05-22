using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class MaxPlayersDecPref : OptionalPreferences
    {
        public int maxPlayers { get; }

        public MaxPlayersDecPref(int maxPlayers, OptionalPreferences nextDecPref): base(nextDecPref)
        {
            this.maxPlayers = maxPlayers;
        }

        public override ReturnMessage canPerformUserActions(TexasHoldemGame game, SystemUser user, string action)
        {
            switch (action)
            {
                case "create":
                    if (maxPlayers >= 2 && maxPlayers <= 9)
                        if (nextDecPref != null)
                            return nextDecPref.canPerformUserActions(game, user, action);
                        else
                            return new ReturnMessage(true, "");
                    else
                        return
                            new ReturnMessage(false, "max players must be between 2 and 9");

                case "join":
                    if (game.AvailableSeats == 0)
                        return new ReturnMessage(false, "There are no available seats.");
                    else if (nextDecPref != null)
                        return nextDecPref.canPerformUserActions(game, user, action);
                    else
                        return new ReturnMessage(true, "");

                case "spectate":
                case "leave":
                    if (nextDecPref != null)
                        return nextDecPref.canPerformUserActions(game, user, action);
                    else
                        return new ReturnMessage(true, "");
                default:
                    return new ReturnMessage(false, "Wrong input to the canPerformUserActions.");
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
            if (pref.GetType().IsAssignableFrom(typeof(OptionalPreferences)))
                //if (pref.GetType() != typeof(OptionalPreferences))
                return false;
            OptionalPreferences opPref = ((OptionalPreferences)pref);
            MaxPlayersDecPref matchingPref = (MaxPlayersDecPref)getMatchingOptionalPref(opPref);
            //if we found matchig optinal pref and he have the same policy
            if (matchingPref != null && matchingPref.maxPlayers == maxPlayers)
                //if we still need to check the rest of the chain
                if (matchingPref.nextDecPref != null)
                    //return its result
                    return nextDecPref.isContain(pref);
                //if we don't have anything else to check return true.
                else return true;
            //if we couldent found or if we found pref with different value.
            else if (matchingPref == null && nextDecPref != null)
                return nextDecPref.isContain(opPref);
            else
                return false;
        }
        public override string ToString()
        {
            return string.Format("maxPlayers: {0}, {1}", maxPlayers, nextDecPref);
        }
    }
}
