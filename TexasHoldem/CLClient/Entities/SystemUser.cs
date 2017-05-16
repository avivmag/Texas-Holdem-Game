using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLClient.Entities
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

        public SystemUser(String name, String password, String email, String userImage, int money)
        {
            this.name       = name;
            this.password   = password;
            this.userImage  = userImage;
            this.money      = money;
            }
    }
}
