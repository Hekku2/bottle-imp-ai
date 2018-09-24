using Core.Internal;
using Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

public class TrickEngine
{
    private readonly Seat[] _seats;

    private int PlayerCount { get { return _seats.Length; } }

    public TrickEngine(Seat[] seats)
    {
        _seats = seats;
    }

	public TrickResult PlayTrick(TrickResult previousResult)
	{
		var bottlePrice = previousResult != null ? previousResult.BottlePrice : 19;
		var bottleOwner = previousResult?.BottleOwner;
		// Create player order
		var order = Enumerable.Range(0, PlayerCount)
			.Select(index =>
			{
				var startIndex = previousResult != null ? previousResult.Winner.Index : 0;

				var current = (startIndex + index) % PlayerCount;
				return _seats[current];
			});

		List<Play> playedCards = AskMoves();

		// Decide winner
		Play winner = GetWinner(playedCards, bottlePrice);
		if (winner.Card.Number < bottlePrice)
		{
			bottleOwner = _seats[winner.Index];
			bottlePrice = winner.Card.Number;
		}

        var cardsBySeat = playedCards.OrderBy(play => play.Index).Select(play => play.Card);
		var result = new Core.External.TrickResult(bottlePrice, cardsBySeat.ToArray());
		foreach (var seat in _seats)
		{
			seat.Player.TrickFinished(result);
		}
		return new TrickResult(bottlePrice, _seats[winner.Index], bottleOwner, cardsBySeat.ToArray());
	}

	private static Play GetWinner(List<Play> playedCards, int bottlePrice)
	{
		Play winning = playedCards.First();
		foreach (var playedCard in playedCards.Skip(1))
		{
			if (IsWinningRule.Wins(bottlePrice, winning.Card ?? null, playedCard.Card))
			{
				winning = playedCard;
			}
		}
		return winning;
	}

	private List<Play> AskMoves()
	{
		var playedCards = new List<Play>();
		foreach (var seat in _seats)
		{
			var card = seat.Player.Play(playedCards.Select(p => p.Card).ToArray());
			//Check rule validity
			var playerHand = seat.Hand;
			if (!PlayRule.IsLegal(playerHand, card, playedCards.Select(p => p.Card).FirstOrDefault()))
			{
				throw new Exception("Illegal move");
			}
			//Remove card from player
			playerHand.RemoveCard(card);
			playedCards.Add(new Play(seat.Index, card));
		}

		return playedCards;
	}
}