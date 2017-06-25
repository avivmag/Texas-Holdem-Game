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
        public byte[] userImageByteArray { get; set; }
        public List<TexasHoldemGame> spectatingGame;

        public bool newPlayer { get; set; }

        public SystemUser(int id, String name, String email, String userImage, int money, int rank, int gamesPlayed)
        {
            this.id = id;
            this.name = name;
            this.email = email;
            this.userImage = userImage;
            this.money = money;
            this.rank = rank;
            spectatingGame = new List<TexasHoldemGame> { };
            
            newPlayer = gamesPlayed < 10;
        }

        public SystemUser(String name, String email, String userImage, int money, int rank, int gamesPlayed) : this(-1, name, email, userImage, money, rank, gamesPlayed) { }
        public SystemUser(String name, String email, String userImage, int money, int rank) : this(name, email, userImage, money, rank, 0){}

        //public void addSpectatingGame(TexasHoldemGame game)
        //{
        //    if (!this.spectatingGame.Contains(game))
        //        this.spectatingGame.Add(game);
        //}

        //public void setTempGames()
        //{
        //    tempGames++;
        //    if (tempGames >= 10)
        //        newPlayer = false;
        //    if (!newPlayer)
        //    {
        //        rank = tempRank;
        //    }
        //}

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SystemUser))
                return false;
            return name.Equals(((SystemUser)obj).name) || id == ((SystemUser)obj).id;
        }

        //public void updateRank(int rankToUpdate)
        //{
        //    if (newPlayer)
        //    {
        //        if (tempRank + rankToUpdate > 0)
        //            tempRank += rankToUpdate;
        //        else
        //        {
        //            tempRank = 0;
        //        }
        //    }
        //    else
        //    {
        //        if (rank + rankToUpdate > 0)
        //        {
        //            rank += rankToUpdate;
        //        }
        //        else
        //        {
        //            rank = 0;
        //        }
        //    }
        //}
    }
}
