using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.User;

namespace Backend.Game.DecoratorPreferences
{
    public class BuyInPolicyDecPref : DecoratorPreferencesInterface
    {
        private int buyInPolicy;
        private DecoratorPreferencesInterface nextDecPref;

        public BuyInPolicyDecPref(int buyInPolicy, DecoratorPreferencesInterface nextDecPref)
        {
            this.buyInPolicy = buyInPolicy;
            this.nextDecPref = nextDecPref;
        }

        public ReturnMessage canPerformGameActions(TexasHoldemGame game, SystemUser user, string action)
        {
            switch (action)
            {


                case "Bet":

                    break;

                case "Raise":

                    break;
                default:
                    return null;
            }
            return null;
        }


        public ReturnMessage canPerformUserActions(Player p, SystemUser user, string action)
        {
            ReturnMessage m = new ReturnMessage();
            switch (action)
            {
               case "join":
                    if (user.money >= buyInPolicy)
                        return nextDecPref.canPerformUserActions(p, user, action);
                    else
                    {
                        return new ReturnMessage(false, "The user dont have enough money to join the game");
                    }
        
               case "spectate":
                    return nextDecPref.canPerformUserActions(p,user, action);

               case "leave":
                    return nextDecPref.canPerformUserActions(p,user, action);

               case "Bet":

                    break;

                case "Raise":

                    break;
                default:
                    return null;
                   
            }
            return null;
        }

        public bool isContain(DecoratorPreferencesInterface pref)
        {
            throw new NotImplementedException();
        }

        public bool isContain(MustPreferences pref)
        {
            throw new NotImplementedException();
        }
    }
}
