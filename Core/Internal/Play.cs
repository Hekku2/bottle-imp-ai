namespace Core.Internal
{
	internal class Play
	{
		public int Index { get; }
		public Card Card { get; }

		public Play(int index, Card card)
		{
			Index = index;
			Card = card;
		}
	}
}
