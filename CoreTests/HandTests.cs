using Core;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoreTests
{
	[TestFixture]
	public class HandTests
	{
		[Test]
		public void HasCard_ReturnTrueIfHandContainsCard()
		{
			var cards = new[]
			{
				new Card(CardType.Blue, 1, 0),
				new Card(CardType.Blue, 2, 0),
				new Card(CardType.Blue, 3, 0)
			};
			var hand = new Hand(cards);
			foreach (var card in cards)
			{
				Assert.IsTrue(hand.HasCard(card));
			}
		}

		[Test]
		public void HasCard_ReturnsFalseIfHandDoesntContainCard()
		{
			var cards = new[]
			{
				new Card(CardType.Blue, 1, 0),
				new Card(CardType.Blue, 2, 0)
			};

			var hand = new Hand(cards);
			Assert.IsFalse(hand.HasCard(new Card(CardType.Red, 1, 0)), "Different color");
			Assert.IsFalse(hand.HasCard(new Card(CardType.Blue, 3, 0)), "Different number");
			Assert.IsFalse(hand.HasCard(new Card(CardType.Red, 3, 0)), "Different color & number");
		}

		[Test]
		public void HasType_ReturnsFalseIfHandDoesntHaveColor()
		{
			var noYellow = new Hand(new[] { new Card(CardType.Blue, 1, 0), new Card(CardType.Red, 1, 0) });
			Assert.IsFalse(noYellow.HasType(CardType.Yellow));
			var noBlue = new Hand(new[] { new Card(CardType.Yellow, 1, 0), new Card(CardType.Red, 1, 0) });
			Assert.IsFalse(noBlue.HasType(CardType.Blue));
			var noRed = new Hand(new[] { new Card(CardType.Blue, 1, 0), new Card(CardType.Yellow, 1, 0) });
			Assert.IsFalse(noRed.HasType(CardType.Red));
		}

		[Test]
		public void HasType_ReturnsTrueIfHandHasColor()
		{
			var cards = new[]
			{
				new Card(CardType.Red, 1, 0),
				new Card(CardType.Blue, 2, 0),
				new Card(CardType.Yellow, 3, 0)
			};
			var hand = new Hand(cards);
			Assert.IsTrue(hand.HasType(CardType.Red));
			Assert.IsTrue(hand.HasType(CardType.Blue));
			Assert.IsTrue(hand.HasType(CardType.Yellow));
		}

		[Test]
		public void RemoveCard_ActuallyRemovesCard()
		{
			var removedCard = new Card(CardType.Blue, 1, 0);
			var cards = new[]
			{
				removedCard,
				new Card(CardType.Blue, 2, 0)
			};

			var hand = new Hand(cards);
			Assert.IsTrue(hand.HasCard(removedCard), "Hand should have this card");
			hand.RemoveCard(removedCard);
			Assert.IsFalse(hand.HasCard(removedCard), "Hand should no longer have this card");
		}

		[Test]
		public void RemoveCard_DoesntRemoveFromOriginal()
		{
			var removedCard = new Card(CardType.Blue, 1, 0);
			var cards = new List<Card>
			{
				removedCard,
				new Card(CardType.Blue, 2, 0)
			};
			var hand = new Hand(cards);
			hand.RemoveCard(removedCard);
			Assert.AreEqual(2, cards.Count, "Remove should not delete from original");
		}

		[Test]
		public void RemoveCard_ThrowsArgumentExceptionIfHandDoesntHaveCard()
		{
			var hand = new Hand(new Card[0]);
			Assert.Throws<ArgumentException>(() => hand.RemoveCard(new Card(CardType.Blue, 2, 0)));
		}
	}
}
