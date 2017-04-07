using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Game
{
	class LeagueTexasHoldemGame : TexasHoldemGame
	{
		public int MinRank { get; }
		public int MaxRank { get; }

		public LeagueTexasHoldemGame(int buyInPolicy, Player gameCreator, GamePreferences gamePreferences, int MinRank, int MaxRank) :
				base(buyInPolicy, gameCreator, gamePreferences)
		{
			this.MinRank = MinRank;
			this.MaxRank = MaxRank;
		}
	}
}
