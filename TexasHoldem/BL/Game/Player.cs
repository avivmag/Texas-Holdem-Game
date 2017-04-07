using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Game
{
	class Player : Spectator
	{
		public int Tokens { get; set; }
		public Player(int tokens) : base()
		{
			Tokens = tokens;
		}
	}
}
