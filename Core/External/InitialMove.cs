namespace Core.External
{
	public class InitialMove
	{
		public Card ToLeft { get; }
		public Card ToRight { get; }
		public Card ToBottle { get; }

		public InitialMove(Card toLeft, Card toRight, Card toBottle)
		{
			ToLeft = toLeft;
			ToRight = toRight;
			ToBottle = toBottle;
		}
	}
}
