using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class GamePolicyDecPref : OptionalPreferences
    {
        public enum GameTypePolicy { Undef, Limit, No_Limit, Pot_Limit };
        private GameTypePolicy gamePolicy;
        private int limit;

        public GamePolicyDecPref(GameTypePolicy gamePolicy, int limit, OptionalPreferences nextDecPref) : base(nextDecPref)
        {
            this.gamePolicy = gamePolicy;
            this.limit = limit;
        }

        public override ReturnMessage canPerformUserActions(TexasHoldemGame game, SystemUser user, string action)
        {
            switch (action)
            {
                case "create":
                    {
                        switch (gamePolicy)
                        {
                            case GameTypePolicy.Limit:
                                if (limit >= 0)
                                    if (nextDecPref != null)
                                        return nextDecPref.canPerformUserActions(game, user, "create");
                                    else
                                        return new ReturnMessage(true, "");
                                else
                                    return new ReturnMessage(false, "The limit must be positive");
                            default:
                                if (nextDecPref != null)
                                    return nextDecPref.canPerformUserActions(game, user, "create");
                                else
                                    return new ReturnMessage(true, "");
                        }
                    }
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
                case "Raise":
                    switch (gamePolicy)
                    {
                        case GameTypePolicy.Limit:
                            if (game.currentBet + amount > limit)
                            {
                                return new ReturnMessage(false, "The entered amount is higher than the limit");
                            }
                            else if (nextDecPref != null)
                                return nextDecPref.canPerformGameActions(game, user, amount, action);
                            else
                                return new ReturnMessage(true, "");
                        case GameTypePolicy.No_Limit:
                            if (nextDecPref != null)
                                return nextDecPref.canPerformGameActions(game, user, amount, action);
                            else
                                return new ReturnMessage(true, "");
                        case GameTypePolicy.Pot_Limit:
                            if (game.currentBet + amount > game.pot)
                            {
                                return new ReturnMessage(false, "The entered amount is higher than the pot limit");
                            }
                            else if (nextDecPref != null)
                                return nextDecPref.canPerformGameActions(game, user, amount, action);
                            else
                                return new ReturnMessage(true, "");
                        default:
                            return new ReturnMessage(false, "Wrong game policy");
                    }
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
            GamePolicyDecPref matchingPref = (GamePolicyDecPref)getMatchingOptionalPref(opPref);
            //if we found matchig optinal pref and he have the same policy
            if (matchingPref != null && matchingPref.gamePolicy == gamePolicy)
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

        public override string ToString()
        {
            return string.Format("limit: {0}, gamePolicy {1}, {2}", limit, gamePolicy.ToString(), nextDecPref);
        }
    }
}
