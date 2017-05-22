using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLClient.Entities
{
    public class Card
    {
        public const int NUMBER_OF_CARDS = 52;
        public const int LOWEST_CARD = 1;
        public const int HIGHEST_CARD = 13;
        
        public enum cardType { club = 0, diamond = 1, heart = 2, spade = 3, unknown = 4 };
        public int Value { get; set; }
        public cardType Type { get; set; }

        public Card(cardType Type, int Value)
        {
            this.Type = Type;
            this.Value = Value;
        }
    }
}
