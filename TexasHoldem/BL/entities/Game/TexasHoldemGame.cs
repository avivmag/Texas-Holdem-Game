using System.Collections.Generic;

namespace BL.Game
{
	public class TexasHoldemGame : Messages.Notification
	{
		public int AvailableSeats { get => GamePreferences.MaxPlayers - players.Count; }
		private Player currentDealer;
		private Player currentBig;
		private Player currentSmall;
		public GamePreferences GamePreferences { get; }
		private Deck deck;
		private List<Player> players;
		private List<Spectator> spectators;
        private int id;

		public TexasHoldemGame(Player gameCreator, GamePreferences gamePreferences)
		{
			GamePreferences = gamePreferences;
			deck = new Deck();
			players = new List<Player>();
			spectators = new List<Spectator>();
		}

		public bool joinGame(Player p)
		{
			if (AvailableSeats == 0)
				return false;

            players.Add(p);
			return true;
		}

        public bool canSpectate()
        {
            return GamePreferences.IsSpectatingAllowed();
        }

        public void addSpectator(Spectator s)
        {
            if (!spectators.Contains(s))
                spectators.Add(s);
        }

        public void leaveGame(Player p)
        {
            players.Remove(p);
        }

        public void leaveGame(Spectator spec)
        {
            players.Remove(spec);
        }

	}
}
