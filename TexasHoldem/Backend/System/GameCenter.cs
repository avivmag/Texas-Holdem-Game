using System;
using System.Collections.Generic;

namespace Backend.System
{
	public class GameCenter : Messages.Notification
	{
		private List<Game.TexasHoldemGame> texasHoldemGames;
		public Game.TexasHoldemGame createGame(int userId, Game.GamePreferences preferences)
		{
			return new Game.TexasHoldemGame(userId, preferences);
		}

		public List<Game.TexasHoldemGame> findGameByCriteria(String str)
		{
			return null;
		}
	}
}
