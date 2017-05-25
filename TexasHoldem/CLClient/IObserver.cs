using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLClient
{
    public interface IObserver
    {
        /// <summary>
        /// Updates the observer with data as obj.
        /// </summary>
        void update(object obj);
    }
}
