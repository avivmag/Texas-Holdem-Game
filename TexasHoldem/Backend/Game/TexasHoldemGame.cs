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
		public List<Spectator> spectators;
        private int gameCreatorUserId;
        public int id { get; set; }
        public int pot { get; set; }
        public bool active { get; set; }

		public TexasHoldemGame(int gameCreatorUserId, GamePreferences gamePreferences)
		{
            this.gameCreatorUserId = gameCreatorUserId;
			this.GamePreferences = gamePreferences;
            pot = 0;
            active = true;
            deck = new Deck();
			players = new List<Player>();
			spectators = new List<Spectator>();
		}

		public virtual Message joinGame(Player p)
		{
			if (AvailableSeats == 0)
				return new Message(false,"There are no available seats.");

            foreach (Player player in players)
                if (player.systemUserID == p.systemUserID)
                    return new Message(false, "The player is already taking part in the wanted game.");

            foreach (Spectator spec in spectators)
                if (spec.systemUserID == p.systemUserID)
                    return new Message(false, "Couldn't join the game because the user is already spectating the game.");

            players.Add(p);
			return new Message(true,"");
		}

        public Message joinSpectate(Spectator s)
        {
            if (!GamePreferences.IsSpectatingAllowed)
                return new Message(false, "Couldn't spectate the game because the game preferences is not alowing.");
            
            foreach (Player p in players)
                if (p.systemUserID == s.systemUserID)
                    return new Message(false, "Couldn't spectate the game because the user is already playing the game.");

            foreach (Spectator spec in spectators)
                if (spec.systemUserID == s.systemUserID)
                    return new Message(false, "Couldn't spectate the game because the user is already spectating the game.");
            
            spectators.Add(s);
            return new Message(true,"");
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
