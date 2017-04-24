using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Backend.Game
{
	public class GameLog
	{
		private static GameLog instance;
		private static Dictionary<int, String> gameIdFilePath = new Dictionary<int, String>();
        public enum Actions{
            Action_Bet,
            Action_Call,
            Action_Fold,
            Action_Check,
            Action_Raise,
            Deal_Card,
            Small_Blind,
            Big_Blind,
            Game_Start,
            Player_Join,
            Player_Left,
            Spectate_Join,
            Spectate_Left,
            Flop,
            Turn,
            River,
            Pot_Changed }
		protected GameLog() { }

        public static void setLog(int gameId, DateTime createDate)
        {
            String filePath = String.Format(@"logs\{0}_{1}.log", gameId.ToString(), createDate.ToString("yyyy-dd-M--HH-mm-ss"));

            if (!gameIdFilePath.ContainsKey(gameId))
            {
                gameIdFilePath.Add(gameId, filePath);
            }
        }

		public static GameLog getInstance()
		{
			if (instance == null)
				instance = new GameLog();

			return instance;
		}
		
		public static String readLine(int gameId)
		{
			try
			{
                if (!gameIdFilePath.ContainsKey(gameId))
                    throw new ArgumentException(String.Format("gameId {0} does not have a log file.", gameId));

                var stream = File.Open(gameIdFilePath[gameId], FileMode.Open, FileAccess.Read);

                StreamReader reader = new StreamReader(stream);
				var line = reader.ReadLine();
                reader.Dispose();
                return line;
			}
			catch
			{
				return null;
			}
		}

		public static void logLine(int gameId, Actions action, params string[] options)
		{
			try
			{
                if (!gameIdFilePath.ContainsKey(gameId))
                    throw new ArgumentException(String.Format("gameId {0} does not have a log file.", gameId));

                var optionsString = "[" + String.Join("][", options) + "]";

                var line = String.Format("[{0}]{1}", action, optionsString);

                FileStream stream = null;

                if (!File.Exists(gameIdFilePath[gameId]))
                {
                    stream = File.Create(gameIdFilePath[gameId]);
                }
                else
                {
                    stream = File.Open(gameIdFilePath[gameId], FileMode.Append, FileAccess.Write);
                }
                StreamWriter writer = new StreamWriter(stream);
                writer.WriteLine(line);
                writer.Dispose();
                stream.Dispose();
            }
			catch(Exception e)
			{
                Console.WriteLine(e);
			}
		}
	}
}
