using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public abstract class OptionalPreferences : DecoratorPreferencesInterface
    {
        public OptionalPreferences nextDecPref { get; set; }

        public OptionalPreferences(OptionalPreferences nextPref)
        {
            nextDecPref = nextPref;
        }

        public abstract ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, int amount, string action);
        public abstract ReturnMessage canPerformUserActions(TexasHoldemGame game, SystemUser user, string action);
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
