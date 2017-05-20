namespace CLClient.Entities
{
    public class GamePreferences
    {
        public enum GameTypePolicy { Undef, Limit, No_Limit, Pot_Limit };
        public GameTypePolicy gamePolicy { get; set; }
        public int limit { get; set; } //related to the policy

        public bool? IsSpectatingAllowed { get; set; }

        public bool? isLeague { get; set; }
        public int minRank { get; set; }
        public int maxRank { get; set; }

        public int buyInPolicy { get; set; }

        public int startingChipsAmount { get; set; }

        public int minimalBet { get; set; }

        public int minPlayers { get; set; }

        public int maxPlayers { get; set; }
    }
}
