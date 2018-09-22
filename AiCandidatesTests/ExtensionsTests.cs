using NUnit.Framework;
using Core;

namespace AiCandidatesTests
{
    [TestFixture]
    public class ExtensionsTests
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