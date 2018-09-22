namespace Core.Internal
{
	public class Seat
	{
		public int Index { get; }
		public Hand Hand { get; }
		public IGameAi Player { get; }

		public Seat(int index, IGameAi player, Hand hand)
		{
			Index = index;
			Player = player;
			Hand = hand;
		}
	}
}
