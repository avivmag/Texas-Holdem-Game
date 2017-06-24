using System.Collections.Generic;

namespace Backend.Game
{
	public class Player
	{
        public int systemUserID { get; set; }
        public int Tokens { get; set; }
        public int TokensInBet { get; set; }
        public bool spectator { get; set; }
        public enum PlayerState { folded, in_round, not_in_round, my_turn, winner }
        public string imageUrl = "profile_pic";
        public PlayerState playerState { get; set; }
        public List<Card> playerCards { get; set; }
        public string name { get; set; }
        public byte[] userImage { get; set; }
        public TexasHoldemGame.HandsRanks handRank { get; set; }
        public int handRankCards { get; set; }

        // a builder to the player
        public Player(int userId, string name, int tokens, int userRank, byte[] userImage)
		{
            systemUserID = userId;
            this.name = name;
            Tokens = tokens;
            spectator = false;
            playerCards = new List<Card>();
            this.userImage = userImage;
		}

        //a builder to a spectator
        public Player(int userId, string name)
        {
            systemUserID = userId;
            spectator = true;
            this.name = name;
        }
    }
}
