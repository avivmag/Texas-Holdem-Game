using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Game
{
	class Card
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


	}
}
