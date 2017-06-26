using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CLClient.Entities
{
    public class GameLog
    {
        // Game log meta data.
        public class GameLogMetaData
        {
            public string row { get; }
            public string gameId { get; }
            public string creatorName { get; }
            public string date { get; }

            public GameLogMetaData(int row, string logLine)
            {
                this.row = row.ToString();

                // Get the log meta data. remove first and last characters as in they are '[' and ']' respectively
                // Split the meta data log line by ][ to receive the meta data.
                var logMetaData = logLine.Substring(1, logLine.Length - 2)
                                         .Split(new string[] { "][" }, StringSplitOptions.None);
                if (logMetaData == null || logMetaData.Length == 4 || logMetaData[0] != "Game_Create")
                {
                    this.gameId = logMetaData[1];
                    this.creatorName = logMetaData[2];
                    this.date = logMetaData[3];
                }
                else
                {
                    this.gameId = "invalid game log data.";
                    this.creatorName = "invalid game logdata.";
                    this.date = "invalid game logdata.";
                }
            }
        }

        // The game log data.
        private string[] data;

        // The index of the next move.
        private int index = 0;

        public GameLog(string [] data)
        {
            this.data = data;
        }

        public GameLogMetaData getMetaData(int index)
        {
            return new GameLogMetaData(index, this.data[0]);
        }

        public string[] peekNextMoveAction()
        {
            if (index >= data.Length)
            {
                return null;
            }
            else if (index < 0)
            {
                // Indicate to not continue with data fetch.
                index = data.Length;
                return null;
            }
            else
            {
                var logLine = data[index];

                // Split the data in logLine by ][ to receive the data.
                var logData = logLine.Substring(1, logLine.Length - 2)
                                     .Split(new string[] { "][" }, StringSplitOptions.None);

                return logData;
            }
        }

        public string getNextMove()
        {
            if (index >= data.Length)
            {
                return null;
            }
            else if (index < 0)
            {
                // Indicate to not continue with data fetch.
                index = data.Length;
                return "Invalid log line. aborting replay.";
            }
            else
            {
                var logLine = data[index];

                // Split the data in logLine by ][ to receive the data.
                var logData = logLine.Substring(1, logLine.Length - 2)
                                     .Split(new string[] { "][" }, StringSplitOptions.None);

                var method = typeof(GameLog).GetMethod(logData[0], BindingFlags.NonPublic | BindingFlags.Static);

                if (method == null)
                {
                    index = logData.Length;
                    return "Invalid log line. aborting replay.";
                }
                index++;
                var returnedLine = (string)method.Invoke(null, new object[] { logData });
                return returnedLine;
            }            
        }

        private static string Pot_Changed(string[] logLine)
        {
            return String.Format("Pot changed! increased by {0} up to {1}", logLine[1], logLine[2]);
        }

        private static string River(string[] logLine)
        {
            return String.Format("River drawn! {0} of {1}!", logLine[2], logLine[1]);
        }

        private static string Turn(string[] logLine)
        {
            return String.Format("Turn drawn! {0} of {1}!", logLine[2], logLine[1]);
        }

        private static string Flop(string[] logLine)
        {
            return String.Format("Flop number {0} drawn! {1} of {2}!", logLine[1], logLine[3], logLine[2]);
        }

        private static string Spectate_Left(string[] logLine)
        {
            return String.Format("{0} has stopped spectating!", logLine[1]);
        }

        private static string Spectate_Join(string[] logLine)
        {
            return String.Format("{0} has started spectating!", logLine[1]);
        }

        private static string Player_Left(string[] logLine)
        {
            return String.Format("{0} has left the game!", logLine[1]);

        }

        private static string Player_Winner(string[] logLine)
        {
            return String.Format("{0} has won the round!", logLine[1]);
        }

        private static string Game_Create(string[] logLine)
        {
            return String.Format("game Id {0} was created by {1} at {2}!", logLine[1], logLine[2], logLine[3]);
        }

        private static string Player_Join(string[] logLine)
        {
            return String.Format("{0} has joined the game!", logLine[1]);
        }

        private static string Round_Start(string[] logLine)
        {
            return String.Format("A new round has started!", logLine[1]);
        }

        private static string Big_Blind(string[] logLine)
        {
            return String.Format("Big blind was put by {0}!", logLine[1]);
        }

        private static string Small_Blind(string[] logLine)
        {
            return String.Format("Small blind was put by {0}!", logLine[1]);
        }

        private static string Deal_Card(string[] logLine)
        {
            return String.Format("{0} of {1} was dealt to {2}!", logLine[3], logLine[2], logLine[4]);
        }

        private static string Action_Raise(string[] logLine)
        {
            return String.Format("{0} has raised by {1}!", logLine[1], logLine[2]);
        }

        private static string Action_Check(string[] logLine)
        {
            return String.Format("{0} has checked!", logLine[1]);
        }

        private static string Action_Fold(string[] logLine)
        {
            return String.Format("{0} has folded!", logLine[1]);
        }

        private static string Action_Call(string[] logLine)
        {
            return String.Format("{0} has called!", logLine[1]);
        }

        private static string Action_Bet(string[] logLine)
        {
            return String.Format("{0} has betted {1}!", logLine[1], logLine[2]);
        }
    }
}
