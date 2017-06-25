using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Domain
{
    public class SystemUser
    {
        public virtual int Id { get; set; }
        public virtual String UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Salt { get; set; }
        public virtual String Email { get; set; }
        public virtual String Image { get; set; }
        public virtual int Money { get; set; }
        public virtual int Rank { get; set; }
        public virtual int GamesPlayed { get; set; }
        public virtual int HighestCashInGame { get; set; }
        public virtual int TotalGrossProfit { get; set; }
    }
}
