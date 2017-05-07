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
        public String name { get; set; }
		public String password { get; set; }
        public String email { get; set; }
		public String userImage { get; set; }
        public List<Game.Player> spectators;
        public Guid leagueId;

        public SystemUser(String name, String password, String email, String userImage, int money)
        {
            this.name = name;
            this.password = password;
            this.email = email;
            this.userImage = userImage;
            this.money = money;
            spectators = new List<Game.Player> { };
            rank = 0;
        }

        public void setLeague(Guid leagueId)
        {
            this.leagueId = leagueId;
        }

		public void update(String str)
		{
			// writeln(str);
		}

        public void addSpectatingGame(Player spec)
        {
            if (!this.spectators.Contains(spec))
                this.spectators.Add(spec);
        }
	}
}
