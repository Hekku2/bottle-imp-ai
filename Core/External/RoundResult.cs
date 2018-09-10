namespace Core.External
{
	public class RoundResult
	{
		public Card[] PlayedCards { get; }
		public int BottlePrice { get; }

		public RoundResult(int bottlePrice, Card[] playedCards)
		{
			BottlePrice = bottlePrice;
			PlayedCards = playedCards;
		}
	}
}
