using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class MinBetDecPref : DecoratorPreferencesInterface
    {
        private int minimalBet;
        private DecoratorPreferencesInterface nextDecPref;

        public MinBetDecPref(int minimalBet, DecoratorPreferencesInterface nextDecPref)
        {
            this.minimalBet = minimalBet;
            this.nextDecPref = nextDecPref;
        }
        public ReturnMessage canPerformUserActions(Player p, SystemUser user, string action)
        {
            switch (action)
            {
                case "join":
                    break;

                case "spectate":
                    break;

                case "leave":
                    break;


                default:
                    return null;
            }
            return null;
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
