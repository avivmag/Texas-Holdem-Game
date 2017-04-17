using System.Collections.Generic;
using Backend.User;

namespace Backend.Game
{
	public class TexasHoldemGame : Messages.Notification
	{
		public int AvailableSeats { get => GamePreferences.MaxPlayers - players.Count; }
		private Player currentDealer;
		private Player currentBig;
		private Player currentSmall;
		public GamePreferences GamePreferences { get; }
		private Deck deck;
		public List<Player> players;
		private List<Spectator> spectators;
        private int gameCreatorUserId;
        public int id { get; set; }

		public TexasHoldemGame(int gameCreatorUserId, GamePreferences gamePreferences)
		{
            this.gameCreatorUserId = gameCreatorUserId;
			this.GamePreferences = gamePreferences;
			deck = new Deck();
			players = new List<Player>();
			spectators = new List<Spectator>();
		}

		public virtual bool joinGame(Player p)
		{
			if (AvailableSeats == 0)
				return false;

            players.Add(p);
			return true;
		}

        public bool joinSpectate(Spectator s)
        {
            if (GamePreferences.IsSpectatingAllowed)
                return false;

            if (!spectators.Contains(s))
                spectators.Add(s);
            return true;
        }

        public void leaveGame(Player p)
        {
            players.Remove(p);
        }

        public void leaveGame(Spectator spec)
        {
            spectators.Remove(spec);
        }

	}
}
