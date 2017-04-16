using System;
using System.Collections.Generic;

namespace BL.User
{
	public class SystemUser
	{
        public int id;
		private int money;
		private String name;
		private String password;
		private String userImage;
		private int rank;
		private List<Game.Spectator> spectators;

		public void update(string str)
		{
			// writeln(str);
		}

        public void addSpectatingGame(Game game)
        {
            if (!this.spectators.Contains(game))
                this.spectators.Add(game);
        }
	}
}
