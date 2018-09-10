using Core;
using Core.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreTests.Rules
{
	[TestFixture]
	public class PlayRuleTests
	{
		[Test]
		public void IsLegal_ReturnsFalseIfCardDoesntExist()
		{
			var cards = new[]
			{
				new Card(CardType.Blue, 1, 0),
				new Card(CardType.Blue, 2, 0)
			};
			var hand = new Hand(cards);

			Assert.IsFalse(PlayRule.IsLegal(hand, new Card(CardType.Red, 1, 0), null), "Different color");
			Assert.IsFalse(PlayRule.IsLegal(hand, new Card(CardType.Blue, 3, 0), null), "Different number");
			Assert.IsFalse(PlayRule.IsLegal(hand, new Card(CardType.Red, 3, 0), null), "Different color & number");
		}

		[Test]
		public void IsLegal_ReturnsFalseIfUserHasCorrectColorButPlaysDifferent()
		{
			var start = new Card(CardType.Blue, 6, 0);
			var cards = new[]
			{
				new Card(CardType.Blue, 1, 0),
				new Card(CardType.Blue, 2, 0),
				new Card(CardType.Red, 3, 0)
			};
			var hand = new Hand(cards);

			Assert.IsFalse(PlayRule.IsLegal(hand, cards[2], start), "User lied");
		}

		[Test]
		public void IsLegal_ReturnTrueIfThereIsNoStartCard()
		{
			var cards = new[]
{
				new Card(CardType.Blue, 1, 0),
				new Card(CardType.Blue, 2, 0),
				new Card(CardType.Red, 3, 0)
			};
			var hand = new Hand(cards);

			Assert.IsTrue(PlayRule.IsLegal(hand, cards[2], null), "All cards should be allowed");
		}
	}
}
