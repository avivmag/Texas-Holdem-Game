using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public abstract class OptionalPreferences : DecoratorPreferencesInterface
    {
        public OptionalPreferences nextDecPref;

        public OptionalPreferences(OptionalPreferences nextPref)
        {
            this.nextDecPref = nextPref;
        }

        public abstract ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, int amount, string action);
        public abstract ReturnMessage canPerformUserActions(TexasHoldemGame game, Player p, SystemUser user, string action);
        public abstract bool isContain(DecoratorPreferencesInterface pref);

        public OptionalPreferences getMatchingOptionalPref(OptionalPreferences toGet)
        {
            while (toGet != null)
            {
                if (GetType() == toGet.GetType())
                    return toGet;
                toGet = toGet.nextDecPref;
            }
            return null;
        }
    }
}
