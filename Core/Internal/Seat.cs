namespace Core.Internal
{
	public class Seat
	{
		public int Index { get; }
		public IGameAi Player { get; }

		public Seat(int index, IGameAi player)
		{
			Index = index;
			Player = player;
		}
	}
}
