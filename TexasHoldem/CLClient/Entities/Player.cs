using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLClient.Entities
{
    public class Player
    {
        public int systemUserID { get; set; }
        public int Tokens { get; set; }
        //public int userRank { get; set; }
        public bool spectator { get; set; }
        public enum PlayerState { folded, in_round }
        public PlayerState playerState { get; set; }
    }
}
