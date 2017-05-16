using System.Collections.Generic;

namespace Backend.Game
{
	public class Player
	{
        public int systemUserID { get; set; }
        public int Tokens { get; set; }
        public int userRank { get; set; }
        public bool spectator { get; set; }
        public enum PlayerState { folded, in_round}
        public PlayerState playerState { get; set; }
        public List<Card> playerCards { get; set; }

		public Player(int userId, int tokens, int userRank)
		{
            this.systemUserID = userId;
			this.Tokens = tokens;
            this.userRank = userRank;
            this.spectator = false;
            playerCards = new List<Card>();
		}

        public Player(int userId)
        {
            this.systemUserID = userId;
            this.spectator = true;
        }
    }
}
