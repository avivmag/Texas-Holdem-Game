using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLClient.Entities.DecoratorPreferences;

namespace CLClient.Entities
{
    public class TexasHoldemGame : IObservable
    {
        public enum HandsRanks { HighCard, Pair, TwoPairs, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush }
        public enum BetAction { fold, bet, call, check, raise }
        private List<IObserver> gameObservers = new List<IObserver>();
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

        public bool gameOnChips { get; set; }
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

        public void Subscribe(IObserver obs)
        {
            if (!gameObservers.Contains(obs))
            {
                this.gameObservers.Add(obs);
            }
        }

        public void Unsubscribe(IObserver obs)
        {
            if (gameObservers.Contains(obs))
            {
                this.gameObservers.Remove(obs);
            }
        }

        public void update(object obj)
        {
            foreach (var obs in gameObservers)
            {
                obs.update(obj);
            }
        }
    }
}
