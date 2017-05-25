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
        public List<TexasHoldemGame> spectatingGame;

        private int tempRank;
        private int tempGames ;
        public bool newPlayer { get; set; }

        static int currentId = 0;
        static int getNextId()
        {
            return ++currentId;
        }

        public SystemUser(String name, String password, String email, String userImage, int money)
        {
            this.name = name;
            this.password = password;
            this.email = email;
            this.userImage = userImage;
            this.money = money;
            spectatingGame = new List<TexasHoldemGame> { };
            rank = -1;

            tempRank = 0;
            tempGames = 0;
            newPlayer = true;

            // NO MORE FUCKING RANDOM!!!
            // IT RUINES MY LIFE EVERY TIME!!!!!!
            //Random rnd = new Random();
            //this.id = rnd.Next(0, 999999);
            this.id = SystemUser.getNextId();
        }

		public void update(String str)
		{
			// writeln(str);
		}

        public void addSpectatingGame(TexasHoldemGame game)
        {
            if (!this.spectatingGame.Contains(game))
                this.spectatingGame.Add(game);
        }

        public void setTempGames()
        {
            tempGames++;
            if (tempGames >= 10)
                newPlayer = false;
            if (!newPlayer)
            {
                rank = tempRank;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SystemUser))
                return false;
            return name.Equals(((SystemUser)obj).name);
        }

        public void updateRank(int rankToUpdate)
        {
            if (newPlayer)
            {
                if (tempRank + rankToUpdate > 0)
                    tempRank += rankToUpdate;
                else
                {
                    tempRank = 0;
                }
            }
            else
            {
                if (rank + rankToUpdate > 0)
                {
                    rank += rankToUpdate;
                }
                else
                {
                    rank = 0;
                }
            }
        }
    }
}
