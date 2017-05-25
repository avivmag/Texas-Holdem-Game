using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLClient
{
    public interface IObservable
    {
        /// <summary>
        /// Subscribes observer to the observable.
        /// </summary>
        /// <param name="obs">Observer to subscribe.</param>
        void Subscribe(IObserver obs);

        /// <summary>
        /// Unsubscribes observer from the observable.
        /// </summary>
        /// <param name="obs">Observer to unsubscribe.</param>
        void Unsubscribe(IObserver obs);

        void update(object obj);
    }
}
