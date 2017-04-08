using System;
using System.Collections.Generic;

namespace BL.System
{
	public class GameCenter : Messages.Notification
	{
		private List<Game.TexasHoldemGame> texasHoldemGames;
		public Game.TexasHoldemGame createGame(int buyInPolicy, Game.Player player, Game.GamePreferences preferences)
		{
			return new Game.TexasHoldemGame(buyInPolicy, player, preferences);
		}

		public List<Game.TexasHoldemGame> findGameByCriteria(String str)
		{
			return null;
		}
	}
}
