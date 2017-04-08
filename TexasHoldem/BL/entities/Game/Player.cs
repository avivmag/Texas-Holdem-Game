namespace BL.Game
{
	public class Player : Spectator
	{
		public int Tokens { get; set; }
		public Player(int tokens) : base()
		{
			Tokens = tokens;
		}
	}
}
