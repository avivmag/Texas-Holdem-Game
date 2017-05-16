using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLShared
{
    public interface CLInterface
    {
        #region GameRegion
        ReturnMessage raiseBet(int gameId, int playerId, int coins);
        #endregion


    }
}
