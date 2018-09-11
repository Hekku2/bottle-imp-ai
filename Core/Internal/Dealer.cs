using System.Linq;

namespace Core.Internal
{
	public static class Dealer
	{
		public static Hand[] Deal(Card[] cards, int playerCount)
		{
			// TODO Actually suffle cards

			var hands = new Hand[playerCount];
			var cardCount = cards.Count() / playerCount;
			for (int i = 0; i < playerCount; i++)
			{
				var chunk = cards.Skip(i*cardCount).Take(cardCount);
				var playerHand = new Hand(chunk);
				hands[i] = playerHand;
			}
			return hands;
		}
	}
}
