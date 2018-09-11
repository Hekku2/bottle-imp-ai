using Core;
using Core.Internal;
using NUnit.Framework;
using System;
using System.Linq;

namespace CoreTests
{
	[TestFixture]
	public class DealerTests
	{
		[Test]
		public void Deal_GivesDifferentCardsToEachPlayer()
		{
			var cards = new Card[]
			{
				new Card(CardType.Blue, 1, 1),
				new Card(CardType.Blue, 2, 2),
				new Card(CardType.Blue, 3, 3)
			};
			var hands = Dealer.Deal(cards, 3);
			var allCards = hands.SelectMany(hand => hand.Cards());
			Assert.AreEqual(cards.Length, allCards.Count());

			foreach (Card card in cards)
			{
				Assert.IsTrue(allCards.Contains(card));
			}
		}
	}
}
