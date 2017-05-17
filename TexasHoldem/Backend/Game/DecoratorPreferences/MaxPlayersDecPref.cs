using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class MaxPlayersDecPref : DecoratorPreferencesInterface
    {
        private int maxPlayers;
        private DecoratorPreferencesInterface nextDecPref;

        public MaxPlayersDecPref(int maxPlayers, DecoratorPreferencesInterface nextDecPref)
        {
            this.maxPlayers = maxPlayers;
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
