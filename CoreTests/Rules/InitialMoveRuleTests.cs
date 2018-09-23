using Core;
using Core.External;
using Core.Rules;
using NUnit.Framework;

namespace CoreTests.Rules
{
	[TestFixture]
	public class InitialMoveRuleTests
	{
		private const int DefaultScore = 1;

		[Test]
		public void IsLegal_ReturnsFalseIfCardGivenIsNotInPlayersCards()
		{
			var cards = new Card[]
			{
				new Card(CardType.Blue, 12, DefaultScore),
				new Card(CardType.Blue, 13, DefaultScore),
				new Card(CardType.Blue, 14, DefaultScore)
			};

			var initialMove = new InitialMove(new Card(CardType.Red, 12, DefaultScore), new Card(CardType.Red, 13, DefaultScore), new Card(CardType.Red, 14, DefaultScore));

			Assert.False(InitialMoveRule.IsLegal(new Hand(cards), initialMove));
		}

		[Test]
		public void IsLegal_ReturnsFalseIfCardIsGivenMultipleTimes()
		{
			var cards = new Card[]
			{
				new Card(CardType.Blue, 12, DefaultScore),
				new Card(CardType.Blue, 13, DefaultScore),
				new Card(CardType.Blue, 14, DefaultScore)
			};

			var initialMove = new InitialMove(
				new Card(CardType.Blue, 12, DefaultScore),
				new Card(CardType.Blue, 12, DefaultScore),
				new Card(CardType.Blue, 13, DefaultScore));

			Assert.False(InitialMoveRule.IsLegal(new Hand(cards), initialMove));
		}

		[Test]
		public void IsLegal_ReturnsTrueIfPlayeHasCards()
		{
			var cards = new Card[]
			{
				new Card(CardType.Blue, 12, DefaultScore),
				new Card(CardType.Blue, 13, DefaultScore),
				new Card(CardType.Blue, 14, DefaultScore)
			};

			var initialMove = new InitialMove(new Card(CardType.Blue, 12, DefaultScore), new Card(CardType.Blue, 13, DefaultScore), new Card(CardType.Blue, 14, DefaultScore));

			Assert.IsTrue(InitialMoveRule.IsLegal(new Hand(cards), initialMove));
		}
	}
}
