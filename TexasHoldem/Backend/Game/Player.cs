using System.Collections.Generic;

namespace Backend.Game
{
	public class Player : Spectator
	{
        public int id { get; set; }
        public int Tokens { get; set; }
        public int userRank { get; set; }
        public enum PlayerState { folded, in_round}
        private PlayerState playerState { get; set; }
        public List<Card> playerCards { get; set; }

		public Player(int userId, int tokens, int userRank) : base(userId)
		{
			this.Tokens = tokens;
            this.userRank = userRank;
		}
	}
}
