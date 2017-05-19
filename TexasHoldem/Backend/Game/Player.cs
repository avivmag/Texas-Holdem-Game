using System.Collections.Generic;

namespace Backend.Game
{
	public class Player
	{
        public int systemUserID { get; set; }
        public int Tokens { get; set; }
        //public int userRank { get; set; }
        public bool spectator { get; set; }
        public enum PlayerState { folded, in_round}
        public PlayerState playerState { get; set; }
        public List<Card> playerCards { get; set; }

        // a builder to the player
		public Player(int userId, int tokens, int userRank)
		{
            systemUserID = userId;
			Tokens = tokens;
            //this.userRank = userRank;
            spectator = false;
            playerCards = new List<Card>();
		}

        //a builder to a spectator
        public Player(int userId)
        {
            systemUserID = userId;
            spectator = true;
        }
    }
}
