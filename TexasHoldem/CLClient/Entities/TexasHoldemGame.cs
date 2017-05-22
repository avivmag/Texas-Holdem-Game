using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLClient.Entities.DecoratorPreferences;

namespace CLClient.Entities
{
    public class TexasHoldemGame
    {
        public enum HandsRanks { HighCard, Pair, TwoPairs, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush }
        public enum BetAction { fold, bet, call, check, raise }

        public int gameId { get; set; }
        public int currentDealer { get; set; }
        public int currentBig { get; set; }
        public int currentSmall { get; set; }
        public int currentBlindBet { get; set; }
        public int pot { get; set; }
        public int tempPot { get; set; }
        public int currentBet { get; set; }

        public Preference gamePreferences { get; set; }
        public Player[] players { get; set; }
        public List<SystemUser> spectators;

        public bool active { get; set; }

        public List<Card> flop { get; set; }
        public Card turn { get; set; }
        public Card river { get; set; }

        public int AvailableSeats
        {
            get
            {
                int ans = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i] == null)
                        ans++;
                }
                return ans;
            }
        }

        //public GameObserver playersChatObserver;
        //public GameObserver spectateChatObserver;
        //public GameObserver gameStatesObserver;

        // TODO: Gili - notice Gil decorator pattern and Aviv player.TokensInBet - you should use them in your logic
    }
}
