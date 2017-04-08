using System.Collections.Generic;

namespace BL.Game
{
	public class TexasHoldemGame : Messages.Notification
	{
		public int AvailableSeats { get => GamePreferences.MaxPlayers - players.Count; }
		public int BuyInPolicy { get; }
		private Player currentDealer;
		private Player currentBig;
		private Player currentSmall;
		public GamePreferences GamePreferences { get; }
		private Deck deck;
		private List<Player> players;
		private List<Spectator> spectators;

		public TexasHoldemGame(int buyInPolicy, Player gameCreator, GamePreferences gamePreferences)
		{
			BuyInPolicy = buyInPolicy;
			GamePreferences = gamePreferences;
			deck = new Deck();
			players = new List<Player>();
			spectators = new List<Spectator>();
		}

		public bool joinGame(Player player)
		{
			if (AvailableSeats == 0)
				return false;

			players.Add(player);
			return true;
		}
	}
}
