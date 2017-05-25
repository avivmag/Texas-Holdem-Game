using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obser
{
    public abstract class ObserverAbstract<T>
    {
        private List<T> subscribers;

        public void subscribe(T t)
        {
            subscribers.Add(t);
        }

        public void unsubscribe(T t)
        {
            subscribers.Remove(t);
        }

        public abstract void update(object obj);
    }
}
