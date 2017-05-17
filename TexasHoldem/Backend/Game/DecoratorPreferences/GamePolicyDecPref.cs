using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class GamePolicyDecPref : DecoratorPreferencesInterface
    {
        public enum GameTypePolicy { Undef, limit, no_limit, pot_limit };
        private GameTypePolicy gamePolicy;
        private int limit;
        private DecoratorPreferencesInterface nextDecPref;

        public GamePolicyDecPref(GameTypePolicy gamePolicy,int limit, DecoratorPreferencesInterface nextDecPref)
        {
            this.gamePolicy = gamePolicy;
            this.nextDecPref = nextDecPref;
            this.limit = limit;
        }

        public ReturnMessage canPerformUserActions(Player p, SystemUser user, string action)
        {
            switch (action)
            {
                case "join":
                    return nextDecPref.canPerformUserActions(p, user, action);

                case "spectate":
                    return nextDecPref.canPerformUserActions(p, user, action);

                case "leave":
                    return nextDecPref.canPerformUserActions(p, user, action);


                default:
                    return null;
            }
        }

        public ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, string action)
            {
                switch (action)
                {
                    case "Bet":

                        break;

                    case "Raise":

                        break;

                }
            return null;
            }

        public bool isContain(MustPreferences pref)
        {
            throw new NotImplementedException();
        }
    }
}
