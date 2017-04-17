using Backend.Game;
using System;
using System.Collections.Generic;

namespace Backend.User
{
	public class SystemUser
	{
        public int id { get; set; }
		public int money { get; set; }
        public int rank { get; set; }
        private String name;
		private String password;
		private String userImage;
				private List<Game.Spectator> spectators;

        public SystemUser(string name, string password, int money)
        {
            this.name = name;
            this.password = password;
            this.money = money;
            spectators = new List<Spectator> { };
            rank = 0;
        }

		public void update(string str)
		{
			// writeln(str);
		}

        public void addSpectatingGame(Spectator spec)
        {
            if (!this.spectators.Contains(spec))
                this.spectators.Add(spec);
        }
	}
}
