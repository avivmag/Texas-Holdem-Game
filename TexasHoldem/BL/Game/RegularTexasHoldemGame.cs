using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Game
{
	class RegularTexasHoldemGame : TexasHoldemGame
	{
		public RegularTexasHoldemGame(int buyInPolicy, Player gameCreator, GamePreferences gamePreferences) :
				base(buyInPolicy, gameCreator, gamePreferences)
		{ }
	}
}
