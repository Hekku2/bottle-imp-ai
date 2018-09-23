using System.Linq;
using Core;
using Core.Internal;
using NSubstitute;
using NUnit.Framework;

namespace CoreTests.Internal
{
	[TestFixture]
	public class TrickEngineTests
	{
		private const int StartingBottlePrice = 19;

		[Test]
		public void PlayTrick_StartsFromFirstPlayerIfThereIsNoPreviousResult()
		{
			var seats = Enumerable.Range(0, 3).Select(index => {
				var card = new Card(CardType.Blue, index + 1, 1);
				var hand = new Hand(new[] { card });
				var player = Substitute.For<IGameAi>();
				player.Play(Arg.Any<Card[]>()).Returns(card);
				var seat = new Seat(index, player, hand);
				return seat;
			}).ToArray();

			var result = TrickEngine.PlayTrick(null, seats);
			Assert.NotNull(result);
			Assert.AreEqual(seats[2], result.BottleOwner);
			Assert.AreEqual(3, result.BottlePrice);
			Assert.AreEqual(seats[2], result.Winner);
			Assert.AreEqual(3, result.WinnerPrice().Count());
		}

		[Test]
		public void PlayTrick_SetsCorrectWinnerWhenNoCardIsUnderBottlePrice()
		{
			var seats = Enumerable.Range(0, 3).Select(index => {
				var card = new Card(CardType.Blue, index + StartingBottlePrice + 1, 1);
				var hand = new Hand(new[] { card });
				var player = Substitute.For<IGameAi>();
				player.Play(Arg.Any<Card[]>()).Returns(card);
				var seat = new Seat(index, player, hand);
				return seat;
			}).ToArray();
			var result = TrickEngine.PlayTrick(null, seats);
			Assert.IsNull(result.BottleOwner);
			Assert.AreEqual(StartingBottlePrice, result.BottlePrice);
			Assert.AreEqual(seats[2], result.Winner);
			Assert.AreEqual(3, result.WinnerPrice().Count());
		}
	}
}