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

        public override Message joinGame(Player p)
        {
            if (AvailableSeats == 0)
                return new Message(false, "There are no available seats.");
            if (p.userRank < MinRank || p.userRank > MaxRank)
                return new Message(false, "The rank of the user is not standing in the league game policy.");
            foreach (Player player in players)
                if (player.systemUserID == p.systemUserID)
                    return new Message(false, "The player is already taking part in the wanted game.");
            foreach (Spectator spec in spectators)
                if (spec.systemUserID == p.systemUserID)
                    return new Message(false, "Couldn't join the game because the user is already spectating the game.");

            players.Add(p);
            return new Message(true,"");
        }
    }
    

}
