using Core.External;
using Core.Rules;
using System;
using System.Linq;

namespace Core.Internal
{
    public class RoundEngine
    {
        private readonly IGameAi[] _players;

        private int PlayerCount { get { return _players.Length; } }

        public RoundEngine(IGameAi[] players)
        {
            _players = players;
        }

        public int[] PlayRound()
        {
            var cards = CardCollection.AllCards();

            //Deal cards
            var playerCards = Dealer.Deal(CardCollection.AllCards(), PlayerCount);
            var minHandSize = playerCards.Min(hand => hand.Cards().Count());
            var seats = Enumerable.Range(0, PlayerCount).Select(i =>
            {
                return new Seat(i, _players[i], playerCards[i]);
            }).ToArray();

            InitialMove[] initialMoves = DoInitialMoves(playerCards);
            GiveInitialCards(playerCards, initialMoves);

            var scoreCount = new ScoreCount(PlayerCount);
            TrickResult previousWinner = null;
            for (int i = 0; i < minHandSize - 1; i++)
            {
                previousWinner = new TrickEngine(seats).PlayTrick(previousWinner);
                scoreCount.Add(previousWinner.Winner.Index, previousWinner.WinnerPrice());
            }

            var bottleTax = initialMoves.Sum(move => move.ToBottle.Score);
            var scores = Scores(scoreCount, previousWinner.BottleOwner.Index, bottleTax);

            foreach (var player in _players)
            {
                player.RoundEnded(scores);
            }

            return scores;
        }

        private static int[] Scores(ScoreCount scoreCount, int bottleOwner, int bottleTax)
        {
            return Enumerable.Range(0, scoreCount.Players).Select(index =>
            {
                if (index == bottleOwner)
                {
                    return -bottleTax;
                }
                return scoreCount.Score(index);
            }).ToArray();
        }

        private void GiveInitialCards(Hand[] playerCards, InitialMove[] initialMoves)
        {
            for (int i = 0; i < PlayerCount; i++)
            {
                var leftPlayerIndex = (i - 1) < 0 ? PlayerCount - 1 : i;
                var rightPlayerIndex = (i + 1) % PlayerCount;

                var cardFromLeft = initialMoves[leftPlayerIndex].ToRight;
                var cardFromRight = initialMoves[rightPlayerIndex].ToLeft;
                _players[i].ReceivedCards(cardFromLeft, cardFromRight);
                playerCards[i].AddCard(cardFromLeft);
                playerCards[i].AddCard(cardFromRight);
            }
        }

        private InitialMove[] DoInitialMoves(Hand[] playerCards)
        {
            return playerCards.Select((hand, i) =>
            {
                var initialMove = _players[i].RoundStart(hand.Cards());
                if (!InitialMoveRule.IsLegal(hand, initialMove))
                {
                    throw new Exception("Illegal move");
                }
                hand.RemoveCard(initialMove.ToBottle);
                hand.RemoveCard(initialMove.ToLeft);
                hand.RemoveCard(initialMove.ToRight);
                return initialMove;
            }).ToArray();
        }
    }
}
