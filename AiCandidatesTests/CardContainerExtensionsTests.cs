using NUnit.Framework;
using Core;

namespace AiCandidatesTests
{
    [TestFixture]
    public class CardContainerExtensionsTests
    {
        [Test]
        public void LowestCard_ReturnsCardWithLowestValue()
        {
            var cards = new Card[]
            {
                new Card(CardType.Blue, 3, 1),
                new Card(CardType.Blue, 2, 1),
                new Card(CardType.Blue, 6, 1)
            };
            var lowest = cards.LowestCard();
            Assert.AreEqual(cards[1].Number, lowest.Number);
        }

        [Test]
        public void LowestSafeCard_ReturnsLowestSafeCardWhenNoOtherCardsArePlayed()
        {
            var cards = new Card[]
            {
                new Card(CardType.Blue, 3, 1),
                new Card(CardType.Blue, 2, 1),
                new Card(CardType.Blue, 6, 1)
            };
            var lowest = cards.LowestSafeCard(2, new Card[0]);
            Assert.AreEqual(cards[0].Number, lowest.Number);
        }

        [Test]
        public void LowestSafeCard_ReturnsLowestSafeCardSomeElseIsInDanger()
        {
            var cards = new Card[]
            {
                new Card(CardType.Blue, 4, 1), //safe but not lowest
                new Card(CardType.Blue, 3, 1), // expected
                new Card(CardType.Blue, 11, 1), // not safe
                new Card(CardType.Blue, 13, 1) // safe but not lowest
            };
            var currentWinner = new Card(CardType.Blue, 10, 1);

            var lowest = cards.LowestSafeCard(12, new Card[] { currentWinner });
            Assert.AreEqual(cards[1].Number, lowest.Number);
        }

        [Test]
        public void LowestSafeCard_ReturnsLowestSafeCardMultipleAreInDanger()
        {
            var cards = new Card[]
            {
                new Card(CardType.Blue, 9, 1), //safe but not lowest
                new Card(CardType.Blue, 7, 1), // expected
                new Card(CardType.Blue, 11, 1), // not safe
                new Card(CardType.Blue, 13, 1) // safe but not lowest
            };
            var currentWinner = new Card(CardType.Blue, 10, 1);
            var redHerring = new Card(CardType.Blue, 8, 1);
            var wasted = new Card(CardType.Blue, 20, 1);

            var lowest = cards.LowestSafeCard(12, new Card[] { redHerring, currentWinner, wasted });
            Assert.AreEqual(cards[1].Number, lowest.Number);
        }

        [Test]
        public void LowestSafeCard_ReturnsNullIfThereAreNoSafeCardsWhenNoOtherCardsArePlayed()
        {
            var cards = new Card[]
            {
                new Card(CardType.Blue, 3, 1),
                new Card(CardType.Blue, 2, 1),
                new Card(CardType.Blue, 6, 1)
            };
            var lowest = cards.LowestSafeCard(7, new Card[0]);
            Assert.IsNull(lowest);
        }

        [Test]
        public void LowestWinningCard_ReturnsLowestWinningCard()
        {
            var cards = new Card[]
            {
                new Card(CardType.Blue, 2, 1),
                new Card(CardType.Blue, 3, 1),
                new Card(CardType.Blue, 6, 1)
            };
            var lowest = cards.LowestWinningCard(2);
            Assert.AreEqual(cards[1].Number, lowest.Number);
        }

        [Test]
        public void LowestLowestWinningCard_ReturnsNullIfThereAreNoWinningCards()
        {
            var cards = new Card[]
            {
                new Card(CardType.Blue, 3, 1),
                new Card(CardType.Blue, 2, 1),
                new Card(CardType.Blue, 6, 1)
            };
            var lowest = cards.LowestWinningCard(7);
            Assert.IsNull(lowest);
        }

        [Test]
        public void HighestCard_ReturnsCardWithLowestValue()
        {
            var cards = new Card[]
            {
                new Card(CardType.Blue, 3, 1),
                new Card(CardType.Blue, 8, 1),
                new Card(CardType.Blue, 6, 1)
            };
            var highes = cards.HighestCard();
            Assert.AreEqual(cards[1].Number, highes.Number);
        }
    }
}