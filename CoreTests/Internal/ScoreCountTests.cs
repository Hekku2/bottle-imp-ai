using Core;
using Core.Internal;
using NUnit.Framework;

namespace CoreTests.Internal
{
    [TestFixture]
    public class ScoreCountTests
    {
        [Test]
        public void Players_ReturnsPlayerCount()
        {
            Assert.AreEqual(3, new ScoreCount(3).Players);
            Assert.AreEqual(4, new ScoreCount(4).Players);
        }

        [Test]
        public void Score_ReturnsInitialScoreOfZero()
        {
            var scores = new ScoreCount(4);
            for (int i = 0; i < 4; i++)
            {
                Assert.AreEqual(0, scores.Score(i));
            }
        }

        [Test]
        public void Add_AddsScoreForCorrectPlayer()
        {
            var scores = new ScoreCount(4);
            var cards = new[]
            {
                new Card(CardType.Blue, 1, 4),
                new Card(CardType.Blue, 1, 3)
            };
            scores.Add(2, cards);
            Assert.AreEqual(7, scores.Score(2));
            Assert.AreEqual(0, scores.Score(0));
            Assert.AreEqual(0, scores.Score(1));
            Assert.AreEqual(0, scores.Score(3));
        }

        [Test]
        public void Add_IsCumulative()
        {
            var scores = new ScoreCount(4);
            scores.Add(2, new[] { new Card(CardType.Blue, 1, 4) });
            scores.Add(2, new[] { new Card(CardType.Blue, 1, 3) });
            Assert.AreEqual(7, scores.Score(2));
            Assert.AreEqual(0, scores.Score(0));
            Assert.AreEqual(0, scores.Score(1));
            Assert.AreEqual(0, scores.Score(3));
        }
    }
}
