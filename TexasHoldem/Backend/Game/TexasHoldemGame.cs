using System.Collections.Generic;
using Backend.User;
using System;

namespace Backend.Game
{
    public class TexasHoldemGame : Messages.Notification
    {
        private int availableSeats;
        public int currentDealer { get; set; }
        private int currentBig { get; set; }
        private int currentSmall { get; set; }
        public GamePreferences GamePreferences { get; }
        private Deck deck;
        public Player[] players { get; set; }
        public List<Spectator> spectators;
        private int gameCreatorUserId;
        public int id { get; set; }
        public int pot { get; set; }
        public bool active { get; set; }
        public int currentBlindBet { get; set; }

		public TexasHoldemGame(int gameCreatorUserId, GamePreferences gamePreferences)
		{
            this.gameCreatorUserId = gameCreatorUserId;
			this.GamePreferences = gamePreferences;
            pot = 0;
            active = true;
            deck = new Deck();
			spectators = new List<Spectator>();
            players = new Player[GamePreferences.MaxPlayers];

            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                players[i] = null;
            }
		}

		public virtual Message joinGame(Player p)
		{
			if (AvailableSeats == 0)
				return new Message(false,"There are no available seats.");

            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].systemUserID == p.systemUserID)
                    return new Message(false, "The player is already taking part in the wanted game.");
            }

            foreach (Spectator spec in spectators)
                if (spec.systemUserID == p.systemUserID)
                    return new Message(false, "Couldn't join the game because the user is already spectating the game.");

            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] == null)
                {
                    players[i] = p;
                    break;
                }
            }
			return new Message(true,"");
		}

        public int AvailableSeats
        {
            get
            {
                int ans = 0;
                for (int i = 0; i < GamePreferences.MaxPlayers; i++)
                {
                    if (players[i] == null)
                        ans++;
                }
                return ans;
            }
        }

        public Message joinSpectate(Spectator s)
        {
            if (!GamePreferences.IsSpectatingAllowed)
                return new Message(false, "Couldn't spectate the game because the game preferences is not alowing.");

            foreach (Player p in players)
                if (p != null)
                {
                    if (p.systemUserID == s.systemUserID)
                        return new Message(false, "Couldn't spectate the game because the user is already playing the game.");
                }
            foreach (Spectator spec in spectators)
                if (spec.systemUserID == s.systemUserID)
                    return new Message(false, "Couldn't spectate the game because the user is already spectating the game.");
            
            spectators.Add(s);
            return new Message(true,"");
        }

        public void leaveGame(Player p)
        {
            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null && players[i].id == p.id)
                {
                    players[i] = null;
                    break;
                }
            }
        }

        public void leaveGame(Spectator spec)
        {
            spectators.Remove(spec);
        }

        private void playGame()
        {
            deck.Shuffle();
            dealCards();
            currentSmall = setSmallBlind();
            currentBig = setBigBlind();
            
        }


        private void dealCards()
        {
            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null)
                {
                    List<Card> playerCards = new List<Card>();
                    playerCards.Add(deck.Top());
                    players[i].playerCards = playerCards;
                }
            }

            for (int i = 0; i < GamePreferences.MaxPlayers; i++)
            {
                if (players[i] != null)
                {
                    List<Card> playerCards = new List<Card>();
                    playerCards.Add(deck.Top());
                    players[i].playerCards = playerCards;
                }
            }
        }

        public int setSmallBlind()
        {
            int i = currentDealer;
            int j = (currentDealer + 1) % GamePreferences.MaxPlayers;
            while (i != j)
            {
                if (players[j] != null)
                    return j;
                j = (j + 1) % GamePreferences.MaxPlayers;
            }
            return -1;
        }

        public int setBigBlind()
        {
            int i = currentDealer;
            int j = (currentDealer + 1) % GamePreferences.MaxPlayers;
            while (i != j)
            {
                if (players[j] != null)
                    break;
                j = (j + 1) % GamePreferences.MaxPlayers;
            }
            j = (j + 1) % GamePreferences.MaxPlayers;
            while (i != j)
            {
                if (players[j] != null)
                    return j;
                j = (j + 1) % GamePreferences.MaxPlayers;
            }
            return -1;
        }

        public void betBlinds()
        {
            players[currentSmall].Tokens -= currentBlindBet / 2; 
            players[currentBig].Tokens -= currentBlindBet;
            pot += (currentBlindBet + (currentBlindBet / 2));
        }
    }
}
