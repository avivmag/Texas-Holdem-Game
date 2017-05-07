namespace Backend.Game
{
	public class GamePreferences
	{
		public enum GameTypePolicy { limit, no_limit, pot_limit };
		public GameTypePolicy GamePolicy { get; }
		public int BuyInPolicy { get; }
		public int StartingChipsAmount { get; }
		public int MinimalBet { get; }
		public int MinPlayers { get; }
		public int MaxPlayers { get; }
		public bool IsSpectatingAllowed { get; }
        public bool isLeague { get; }
        public int MinRank { get; }
        public int MaxRank { get; }

        public GamePreferences(GameTypePolicy gamePolicy, int BuyInPolicy, int startingChipsAmount, int minimalBet, int minPlayers, int maxPlayers, bool isSpectatingAllowed)
		{
			this.GamePolicy = gamePolicy;
			this.BuyInPolicy = BuyInPolicy;
			this.StartingChipsAmount = startingChipsAmount;
			this.MinimalBet = minimalBet;
			this.MinPlayers = minPlayers;
			this.MaxPlayers = maxPlayers;
			this.IsSpectatingAllowed = isSpectatingAllowed;
            this.isLeague = false;
            this.MinRank = -1;
            this.MaxRank = -1;
		}

        public GamePreferences(GameTypePolicy gamePolicy, int BuyInPolicy, int startingChipsAmount, int minimalBet, int minPlayers, int maxPlayers, bool isSpectatingAllowed, int minRank, int maxRank)
        {
            this.GamePolicy = gamePolicy;
            this.BuyInPolicy = BuyInPolicy;
            this.StartingChipsAmount = startingChipsAmount;
            this.MinimalBet = minimalBet;
            this.MinPlayers = minPlayers;
            this.MaxPlayers = maxPlayers;
            this.IsSpectatingAllowed = isSpectatingAllowed;
            this.isLeague = true;
            this.MinRank = minRank;
            this.MaxRank = maxRank;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(GamePreferences))
                return false;
            GamePreferences pref = (GamePreferences)obj;
            return (GamePolicy == pref.GamePolicy && BuyInPolicy == pref.BuyInPolicy && StartingChipsAmount == pref.StartingChipsAmount && MinimalBet == pref.MinimalBet
                && MinPlayers == pref.MinPlayers && MaxPlayers == pref.MaxPlayers && IsSpectatingAllowed == pref.IsSpectatingAllowed && isLeague == pref.isLeague && MinRank == pref.MinRank
                && MaxRank == pref.MaxRank);
        }
    }
}
