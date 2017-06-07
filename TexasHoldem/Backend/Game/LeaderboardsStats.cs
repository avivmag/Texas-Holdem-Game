using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Game
{
    public class LeaderboardsStats
    {
        public int highetsCashInAGame { get; set; }
        public int numberOfGamesPlayed { get; set; }
        public int totalGrossProfit { get; set; }

        public LeaderboardsStats()
        {
            highetsCashInAGame = 0;
            numberOfGamesPlayed = 0;
            totalGrossProfit = 0;
        }

        public int winRate()
        {
            return totalGrossProfit / numberOfGamesPlayed;
        }
    }
}