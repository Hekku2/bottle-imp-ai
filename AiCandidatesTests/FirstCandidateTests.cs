using AiCandidates;
using Core;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;

[TestFixture]
public class FirstCandidateTests
{

    private FirstCandidate _ai;

    [SetUp]
    public void Setup()
    {
        var mockLogger = Substitute.For<ILogger<FirstCandidate>>();
        _ai = new FirstCandidate(mockLogger);
    }

    [Test]
    public void Play_ReturnsWinningCardForClearWin()
    {
        _ai.GameStart(3, 1, 0);

        var playerHand = new []
        {
            new Card(CardType.Blue, 1, 1), // left
            new Card(CardType.Blue, 2, 1), // right
            new Card(CardType.Blue, 3, 1), // bottle
            new Card(CardType.Blue, 36, 1)
        };
        var initial = _ai.RoundStart(playerHand);
        _ai.ReceivedCards(playerHand[1], playerHand[2]);

        var currentPlay = new []
        {
            new Card(CardType.Blue, 34, 1),
            new Card(CardType.Blue, 33, 1)
        };
        var result = _ai.Play(currentPlay);
        Assert.AreEqual(playerHand[3], result, "Only one winning option");
    }

    [Test]
    public void Play_ShouldDiscardLowCardWhenPossible()
    {
        _ai.GameStart(3, 1, 0);

        var playerHand = new []
        {
            new Card(CardType.Blue, 1, 1), // left
            new Card(CardType.Blue, 2, 1), // right
            new Card(CardType.Blue, 20, 1), // bottle
            new Card(CardType.Blue, 36, 1)
        };
        var initial = _ai.RoundStart(playerHand);
        _ai.ReceivedCards(playerHand[1], playerHand[2]);

        var currentPlay = new []
        {
            new Card(CardType.Blue, 4, 1),
            new Card(CardType.Blue, 33, 1)
        };
        var result = _ai.Play(currentPlay);
        Assert.AreEqual(playerHand[1], result, "Play should be low card because it's possible");
    }

	[Test]
	public void Play_ShouldDiscardLowestSafeCardWhenPossible()
	{
		_ai.GameStart(3, 1, 0);

		var playerHand = new[]
		{
			new Card(CardType.Blue, 1, 1), // left
            new Card(CardType.Blue, 2, 1), // right
            new Card(CardType.Blue, 20, 1), // bottle
			new Card(CardType.Blue, 25, 1)
		};
		var initial = _ai.RoundStart(playerHand);
		_ai.ReceivedCards(playerHand[1], playerHand[2]);

		var currentPlay = new[]
		{
			new Card(CardType.Blue, 21, 1),
			new Card(CardType.Blue, 37, 1)
		};
		var result = _ai.Play(currentPlay);
		Assert.AreEqual(playerHand[2], result, "Play should be low card because it's possible");
	}
}