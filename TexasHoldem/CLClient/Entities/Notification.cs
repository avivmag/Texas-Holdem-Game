using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLClient.Entities
{
    public class Notification
    {
        private string message;

        public Notification()
        {
            message = String.Empty;
        }

        public Notification(string m)
        {
            message = m;
        }

        public string getMessage()
        {
            return this.message;
        }
    }
}
