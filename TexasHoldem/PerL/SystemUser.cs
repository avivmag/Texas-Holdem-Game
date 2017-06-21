using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerL
{
    class SystemUser
    {
        public int id { get; set; }
        public int money { get; set; }
        public int rank { get; set; }
        public String name { get; set; }
        public String password { get; set; }
        public String email { get; set; }
        public String userImage { get; set; }
        //public List<TexasHoldemGame> spectatingGame;
        public bool newPlayer { get; set; }
        
        //public SystemUser(int id, String name, String email, String userImage, int money, int rank, int gamesPlayed)
        //{
        //    this.id = id;
        //    this.name = name;
        //    this.email = email;
        //    this.userImage = userImage;
        //    this.money = money;
        //    this.rank = rank;
        //    //spectatingGame = new List<TexasHoldemGame> { };
        //    newPlayer = gamesPlayed < 10;
        //}

        //public SystemUser(String name, String email, String userImage, int money, int rank, int gamesPlayed) : this(-1, name, email, userImage, money, rank, gamesPlayed) { }
        //public SystemUser(String name, String email, String userImage, int money, int rank) : this(name, email, userImage, money, rank, 0) { }

        //public void update(String str)
        //{
        //    // writeln(str);
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj.GetType() != typeof(SystemUser))
        //        return false;
        //    return name.Equals(((SystemUser)obj).name);
        //}
    }
}
