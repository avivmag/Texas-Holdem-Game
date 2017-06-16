using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLClient.Entities.DecoratorPreferences
{
    public class Preference
    {
        public Preference firstDecPref { get; set; }
        public Preference nextDecPref { get; set; }
        public int buyInPolicy { get; set; }
        public enum GameTypePolicy { /*Undef,*/ Limit, No_Limit, Pot_Limit };
        public GameTypePolicy gamePolicy { get; set; }
        public int limit { get; set; }
        public int minPlayers { get; set; }
        public int maxPlayers { get; set; }
        public int minimalBet { get; set; }
        public int startingChipsPolicy { get; set; }
        public bool isSpectateAllowed { get; set; }
        public bool isLeague { get; set; }


        public Preference()
        {
        }

        public void flatten()
        {
            if (nextDecPref == null && firstDecPref == null)
            {
                return;
            }

            flatten(this, firstDecPref);
            flatten(this, nextDecPref);

            if (minPlayers == default(int)){
                minPlayers = 2;
            }
            if (maxPlayers == default(int))
            {
                maxPlayers = 9;
            }
        }

        private void flatten(Preference fix, Preference next)
        {
            if (next == null)
            {
                return;
            }
            if (next.limit != default(int))
            {
                fix.limit = next.limit;
            }
            if (next.buyInPolicy != default(int))
            {
                fix.buyInPolicy = next.buyInPolicy;
            }
            if (next.gamePolicy != default(GameTypePolicy))
            {
                fix.gamePolicy = next.gamePolicy;
            }
            if (next.isLeague != default(bool))
            {
                fix.isLeague = next.isLeague;
            }
            if (next.isSpectateAllowed != default(bool))
            {
                fix.isSpectateAllowed = next.isSpectateAllowed;
            }
            if (next.maxPlayers != default(int))
            {
                fix.maxPlayers = next.maxPlayers;
            }
            if (next.minimalBet != default(int))
            {
                fix.minimalBet = next.minimalBet;
            }
            if (next.minPlayers != default(int))
            {
                fix.minPlayers = next.minPlayers;
            }
            if (next.startingChipsPolicy != default(int))
            {
                fix.startingChipsPolicy = next.startingChipsPolicy;
            }

            if (next.nextDecPref == null && next.firstDecPref == null)
            {
                return;
            }

            flatten(fix, next.firstDecPref);
            flatten(fix, next.nextDecPref);
        }
    }
}
