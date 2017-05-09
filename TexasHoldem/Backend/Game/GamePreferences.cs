namespace Backend.Game
{
	public class GamePreferences
	{
		public enum GameTypePolicy { Undef, limit, no_limit, pot_limit };
		public GameTypePolicy GamePolicy { get; }
		public int BuyInPolicy { get; }
		public int StartingChipsAmount { get; }
		public int MinimalBet { get; }
		public int MinPlayers { get; }
		public int MaxPlayers { get; }
		public bool? IsSpectatingAllowed { get; }
        public bool isLeague { get; }
        public int MinRank { get; }
        public int MaxRank { get; }

        public GamePreferences(GameTypePolicy gamePolicy, int BuyInPolicy, int startingChipsAmount, int minimalBet, int minPlayers, int maxPlayers, bool? isSpectatingAllowed)
		{
			this.GamePolicy = gamePolicy;
			this.BuyInPolicy = BuyInPolicy;
			this.StartingChipsAmount = startingChipsAmount;
			this.MinimalBet = minimalBet;
			this.MinPlayers = minPlayers;
			this.MaxPlayers = maxPlayers;
			this.IsSpectatingAllowed = isSpectatingAllowed;
            this.isLeague = false;
            this.MinRank = 0;
            this.MaxRank = int.MaxValue;
		}

        public GamePreferences(GameTypePolicy gamePolicy, int BuyInPolicy, int startingChipsAmount, int minimalBet, int minPlayers,
                                int maxPlayers, bool? isSpectatingAllowed, int minRank, int maxRank)
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

            if (pref.GamePolicy != GameTypePolicy.Undef && GamePolicy != pref.GamePolicy)
                return false;

            if (pref.BuyInPolicy != -1 && BuyInPolicy != pref.BuyInPolicy)
                return false;

            if (pref.StartingChipsAmount != -1 && StartingChipsAmount != pref.StartingChipsAmount)
                return false;

            if (pref.MinimalBet != -1 && MinimalBet != pref.MinimalBet)
                return false;

            if (pref.MinPlayers != -1 && MinPlayers != pref.MinPlayers)
                return false;

            if (pref.MaxPlayers != -1 && MaxPlayers != pref.MaxPlayers)
                return false;

            if (pref.IsSpectatingAllowed.HasValue && IsSpectatingAllowed != pref.IsSpectatingAllowed)
                return false;

            if (isLeague != pref.isLeague)
                return false;

            if (pref.MinRank != -1 && MinRank != pref.MinRank)
                return false;

            if (pref.MaxRank != -1 && MaxRank != pref.MaxRank)
                return false;

            return true;
        }
    }
}
