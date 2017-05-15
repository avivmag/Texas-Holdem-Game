using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLShared;
using SL;

namespace CLServer
{
    [Serializable]
    public class CLImpl : CLShared.CLInterface
    {
        private SLInterface sl;
        public CLImpl(SLInterface sl)
        {
            this.sl = sl;
        }
        
        ReturnMessage CLInterface.raiseBet(int gameId, int playerId, int coins)
        {
            return sl.raiseBet(gameId, playerId, coins);
        }
    }
}
