using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Internal
{
	public static class Dealer
	{
		private static Random rng = new Random();

		public static Hand[] Deal(Card[] cards, int playerCount)
		{
			var copy = cards.ToList();
			Shuffle(copy);

			var hands = new Hand[playerCount];
			var cardCount = copy.Count() / playerCount;
			for (int i = 0; i < playerCount; i++)
			{
				var chunk = copy.Skip(i*cardCount).Take(cardCount);
				var playerHand = new Hand(chunk);
				hands[i] = playerHand;
			}
			return hands;
		}

		private static void Shuffle<T>(IList<T> list)
		{
			int n = list.Count;
			
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}
	}
}
