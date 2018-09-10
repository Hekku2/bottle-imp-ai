using Core;
using Core.Rules;
using NUnit.Framework;

namespace CoreTests.Rules
{
	[TestFixture]
	public class IsWinningRuleTests
	{
		[Test]
		public void Wins_ReturnsTrueIfCurrentWinningCardIsNotSet()
		{
			var challenger = new Card(CardType.Blue, 12, 0);

			Assert.IsTrue(IsWinningRule.Wins(1, null, challenger), "When there is no current winner, challenger wins by default");
		}

		[Test]
		public void Wins_CurrentWinnerHigherThanBottlePrice()
		{
			var current = new Card(CardType.Blue, 12, 0);
			Assert.IsTrue(IsWinningRule.Wins(10, current, new Card(CardType.Blue, 13, 0)), "Higher card should win");
			Assert.IsFalse(IsWinningRule.Wins(10, current, new Card(CardType.Blue, 11, 0)), "Lower card should lose");
			Assert.IsTrue(IsWinningRule.Wins(10, current, new Card(CardType.Blue, 9, 0)), "Lower than bottle price should");
		}

		[Test]
		public void Wins_CurrentWinnerLowerThanBottlePrice()
		{
			var current = new Card(CardType.Blue, 12, 0);
			Assert.IsTrue(IsWinningRule.Wins(15, current, new Card(CardType.Blue, 13, 0)), "Higher card should win");
			Assert.IsFalse(IsWinningRule.Wins(15, current, new Card(CardType.Blue, 16, 0)), "Higher than bottle price should lose");
			Assert.IsFalse(IsWinningRule.Wins(15, current, new Card(CardType.Blue, 11, 0)), "Lower card should lose");
		}
	}
}
