using System;
using System.Collections.Generic;
using System.IO;

namespace Backend.Game
{
	public class GameLog
	{
		private static GameLog instance;
		private Dictionary<string, StreamWriter> swList;
		private Dictionary<string, StreamReader> srList;
		protected GameLog() { }

		public static GameLog getInstance()
		{
			if (instance == null)
				instance = new GameLog();

			return instance;
		}
		
		public String readLine(String logName)
		{
			try
			{
				if (!srList.ContainsKey(logName))
					srList.Add(logName, File.OpenText(logName));

				return srList[logName].ReadLine();
			}
			catch
			{
				return null;
			}
		}

		public void writeLine(String logName, String line)
		{
			try
			{
				if (!swList.ContainsKey(logName))
					swList.Add(logName, File.AppendText(logName));

				swList[logName].WriteLine(line);
			}
			catch
			{
			}
		}
	}
}
