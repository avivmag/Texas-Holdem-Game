using System;
using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class MinBetDecPref : OptionalPreferences
    {
        public int minimalBet { get; }

        public MinBetDecPref(int minimalBet, OptionalPreferences nextDecPref): base(nextDecPref)
        {
            this.minimalBet = minimalBet;
        }

        public override ReturnMessage canPerformUserActions(TexasHoldemGame game, SystemUser user, string action)
        {
            switch (action)
            {
                case "create":
                    if (minimalBet >= 0)
                        if (nextDecPref != null)
                            return nextDecPref.canPerformUserActions(game, user, action);
                        else
                            return new ReturnMessage(true, "");
                    else
                        return
                            new ReturnMessage(false, "Minimal bet must be positive");
                default:
                    if (nextDecPref != null)
                        return nextDecPref.canPerformUserActions(game, user, action);
                    return new ReturnMessage(true, "");
            }
        }

        public override ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, int amount, string action)
        {
            switch (action)
            {
                case "Bet":
                    if (amount < minimalBet)
                        return new ReturnMessage(false, "The bet you entered is lower than the minimal bet: " + minimalBet.ToString());
                    else if (nextDecPref != null)
                        return nextDecPref.canPerformGameActions(game, user, amount, action);
                    else
                        return new ReturnMessage(true, "");

                case "Raise":
                    if (nextDecPref != null)
                        return nextDecPref.canPerformGameActions(game, user, amount, action);
                    else
                        return new ReturnMessage(true, "");

                default:
                    return new ReturnMessage(false, "Wrong input to the canPerformGameActions.");
            }
        }

        public override bool isContain(DecoratorPreferencesInterface pref)
        {
            if (pref.GetType().IsAssignableFrom(typeof(OptionalPreferences)))
                //if (pref.GetType() != typeof(OptionalPreferences))
                return false;
            OptionalPreferences opPref = ((OptionalPreferences)pref);
            MinBetDecPref matchingPref = (MinBetDecPref)getMatchingOptionalPref(opPref);
            //if we found matchig optinal pref and he have the same policy
            if (matchingPref != null && matchingPref.minimalBet == minimalBet)
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
            return string.Format("minBet: {0}, {1}", minimalBet, nextDecPref);
        }
    }
}
