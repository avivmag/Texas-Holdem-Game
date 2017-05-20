using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int gameCreatorUserId;
        public int availableSeats;

        public bool active { get; set; }

        public List<Card> flop { get; set; }
        public Card turn { get; set; }
        public Card river { get; set; }

        public Player[] players { get; set; }
    }
}
