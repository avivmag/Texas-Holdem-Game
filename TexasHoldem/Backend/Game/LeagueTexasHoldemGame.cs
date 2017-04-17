using System.Collections.Generic;

namespace Backend.Game
{
	public class LeagueTexasHoldemGame : TexasHoldemGame
	{
		public int MinRank { get; }
		public int MaxRank { get; }

		public LeagueTexasHoldemGame(int gameCreatorUserId, GamePreferences gamePreferences, int MinRank, int MaxRank) :
				base(gameCreatorUserId, gamePreferences)
		{
			this.MinRank = MinRank;
			this.MaxRank = MaxRank;
		}

        public override bool joinGame(Player p)
        {
            if (AvailableSeats == 0 || p.userRank<MinRank || p.userRank>MaxRank )
                return false;

            players.Add(p);
            return true;
        }
    }
    

}
