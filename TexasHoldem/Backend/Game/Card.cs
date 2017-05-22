using System;
using System.Collections.Generic;

namespace Backend.Game
{
	public class Card : IComparable<Card>
	{
		public const int NUMBER_OF_CARDS = 52;
		public const int LOWEST_CARD = 1;
		public const int HIGHEST_CARD = 13;
        
        public enum cardType { club = 0, diamond = 1, heart = 2, spade = 3 };
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

        public int CompareTo(Card other)
        {

            return this.Value - other.Value;
        }
    }
}