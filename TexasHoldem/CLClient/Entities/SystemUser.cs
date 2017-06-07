using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLClient.Entities
{
    public class SystemUser : IObservable
    {
        public int id { get; set; }
        public int money { get; set; }
        public int rank { get; set; }
        public String name { get; set; }
        public String password { get; set; }
        public String email { get; set; }
        public String userImage { get; set; }
        private List<IObserver> subscribers = new List<IObserver>();

        public bool newPlayer { get; set; }

        public SystemUser(String name, String password, String email, String userImage, int money)
        {
            this.name       = name;
            this.password   = password;
            this.userImage  = userImage;
            this.email      = email;
            this.money      = money;
            this.newPlayer  = true;
            }

        public void Subscribe(IObserver obs)
        {
            if (!subscribers.Contains(obs))
            {
                subscribers.Add(obs);
            }
        }

        public void Unsubscribe(IObserver obs)
        {
            if (!subscribers.Contains(obs))
            {
                subscribers.Remove(obs);
            }
        }

        public void update(object obj)
        {
            foreach (var obs in subscribers)
            {
                obs.update(obj);
            }
        }
    }
}
