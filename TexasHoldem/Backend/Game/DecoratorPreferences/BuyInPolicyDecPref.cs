using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class BuyInPolicyDecPref : OptionalPreferences
    {
        public int buyInPolicy { get; }

        public BuyInPolicyDecPref(int buyInPolicy, OptionalPreferences nextDecPref): base(nextDecPref)
        {
            this.buyInPolicy = buyInPolicy;
        }

        public override ReturnMessage canPerformUserActions(TexasHoldemGame game, SystemUser user, string action)
        {
            ReturnMessage m = new ReturnMessage();
            switch (action)
            {
                case "create":
                    if (buyInPolicy >= 0)
                        if (nextDecPref != null)
                            return nextDecPref.canPerformUserActions(game, user, "create");
                        else
                            return new ReturnMessage(true, "");
                    else
                        return new ReturnMessage(false, "Buy in policy can't be negative");

                case "join":
                    if (user.money >= buyInPolicy)
                        if (nextDecPref != null)
                            return nextDecPref.canPerformUserActions(game, user, action);
                        else
                            return new ReturnMessage(true, "");
                    else
                        return new ReturnMessage(false, "The user don't have enough money to join the game");

                case "spectate":
                case "leave":
                    if (nextDecPref !=null)
                        return nextDecPref.canPerformUserActions(game, user, action);
                    return new ReturnMessage(true, "");

                default:
                    return new ReturnMessage(false, "Wrong input to the canPerformUserActions.");
            }
        }

        public override ReturnMessage canPerformGameActions(TexasHoldemGame game, int amount, string action)
        {
            if (nextDecPref != null)
                return nextDecPref.canPerformGameActions(game, amount, action);
            return new ReturnMessage(true, "");
        }

        public override bool isContain(DecoratorPreferencesInterface pref)
        {
            if (pref.GetType().IsAssignableFrom(typeof(OptionalPreferences)))
                //if (pref.GetType() != typeof(OptionalPreferences))
                return false;
            OptionalPreferences opPref = ((OptionalPreferences)pref);
            BuyInPolicyDecPref matchingPref = (BuyInPolicyDecPref)getMatchingOptionalPref(opPref);
            //if we found matchig optinal pref and he have the same policy
            if (matchingPref != null && matchingPref.buyInPolicy== buyInPolicy)
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
            return string.Format("buyInPolicy: {0}, {1}", buyInPolicy, nextDecPref);
        }
    }
}
