using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Game
{
	class GamePreferences
	{
		public enum GameTypePolicy { limit, no_limit, pot_limit };
		public GameTypePolicy GamePolicy { get; }
		public int JoinCost { get; }
		public int StartingChipsAmount { get; }
		public int MinimalBet { get; }
		public int MinPlayers { get; }
		public int MaxPlayers { get; }
		public bool IsSpectatingAllowed { get; }

		public GamePreferences(GameTypePolicy gamePolicy, int joinCost, int startingChipsAmount, int minimalBet, int minPlayers, int maxPlayers, bool isSpectatingAllowed)
		{
			this.GamePolicy = gamePolicy;
			this.JoinCost = joinCost;
			this.StartingChipsAmount = startingChipsAmount;
			this.MinimalBet = minimalBet;
			this.MinPlayers = minPlayers;
			this.MaxPlayers = maxPlayers;
			this.IsSpectatingAllowed = isSpectatingAllowed;
		}
	}
}
