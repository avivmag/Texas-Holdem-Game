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
        private cardType club;

        public enum cardType { club = 0, diamond = 1, heart = 2, spade = 3 };
        public int Value { get; }
        public cardType Type { get; }
    }
}
