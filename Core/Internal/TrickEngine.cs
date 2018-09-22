using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.External;
using Core.Internal;
using Core.Rules;

public class TrickEngine
{
    public static TrickResult PlayTrick(TrickResult previousResult, Seat[] seats)
    {
        var bottlePrice = previousResult != null ? previousResult.BottlePrice : 19;
        var bottleOwner = previousResult != null ? previousResult.BottleOwner : null;
        // Create player order
        var order = Enumerable.Range(0, seats.Count())
            .Select(index =>
            {
                var startIndex = previousResult != null ? previousResult.Winner.Index : 0; 

                var current = (startIndex + index) % seats.Length;
                return seats[current];
            });

        List<Play> playedCards = AskMoves(order);

        // Decide winner
        Play winner = GetWinner(playedCards, bottlePrice);
        if (winner.Card.Number < bottlePrice)
        {
            bottleOwner = seats[winner.Index];
            bottlePrice = winner.Card.Number;
        }

        var result = new Core.External.TrickResult(bottlePrice, playedCards.Select(p => p.Card).ToArray());
        foreach (var seat in seats)
        {
            seat.Player.TrickFinished(result);
        }
        return new TrickResult()
        {
            Winner = seats[winner.Index],
            BottlePrice = bottlePrice,
            BottleOwner = bottleOwner,
            WinnerPrice = playedCards.Select(p => p.Card).ToArray()
        };
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

    private static List<Play> AskMoves(IEnumerable<Seat> order)
    {
        var playedCards = new List<Play>();
        foreach (var seat in order)
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