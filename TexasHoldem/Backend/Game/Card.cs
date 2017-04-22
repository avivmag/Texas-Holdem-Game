using System;
using System.Collections.Generic;

namespace Backend.Game
{
	public class Card : Comparer<Card>
	{
		public const int NUMBER_OF_CARDS = 52;
		public const int LOWEST_CARD = 1;
		public const int HIGHEST_CARD = 13;

		public enum cardType { club, diamond, heart, spade };
		public int Value { get; }
		public cardType Type { get; }

		public Card(cardType type, int value)
		{
			this.Type = type;
			this.Value = value;
		}

        public override bool Equals(object obj)
        {
            if (!(obj.GetType().Equals(typeof(Card))))
                return false;

            Card card2 = (Card) obj;

            if (card2.Type == this.Type && card2.Value.Equals(this.Value))
                return true;
            return false;
        }

        public override string ToString()
        {
            return "Type: " + Type.ToString() + ",  Value: " + Value;
        }

        public override int Compare(Card x, Card y)
        {
            return x.Value - y.Value;
        }
    }
}