using System.Diagnostics.CodeAnalysis;

namespace Core.External
{
	public class TrickResult
	{
		public Card[] PlayedCards { get; }
		public int BottlePrice { get; }

		public TrickResult(int bottlePrice, Card[] playedCards)
		{
			BottlePrice = bottlePrice;
			PlayedCards = playedCards;
		}
	}
}
