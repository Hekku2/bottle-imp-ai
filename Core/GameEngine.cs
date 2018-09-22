using Core.External;
using Core.Internal;
using Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
	public class GameEngine
	{
		private readonly int _rounds;
		private IGameAi[] _players;

		public GameEngine(int rounds, IEnumerable<IGameAi> players) {
			_rounds = rounds;
			_players = players.ToArray();
		}
		
		public int[] Run()
		{
			var finalScores = new int[_players.Count()];

			//Inform about other players
			foreach (var player in _players)
			{
				player.GameStart(_players.Count(), _rounds);
			}
			for (int roundNumber = 0; roundNumber < _rounds; roundNumber++)
			{
				var cards = CardCollection.AllCards();

				//Deal cards
				var playerCards = Dealer.Deal(CardCollection.AllCards(), _players.Count());
				var minHandSize = playerCards.Min(hand => hand.Cards().Count());
				var seats = Enumerable.Range(0, _players.Length).Select(i => {
					return new Seat(i, _players[i], playerCards[i]);
				}).ToArray();

				InitialMove[] initialMoves = DoInitialMoves(playerCards);
				GiveInitialCards(playerCards, initialMoves);

				//Set bottle cards
				var bottleTax = initialMoves.Sum(move => move.ToBottle.Score);

				var scoreCount = new ScoreCount(_players.Count());
				int? bottleOwner = null;

				TrickResult previousWinner = null;
				for (int i = 0; i < minHandSize - 1; i++)
				{
					previousWinner = TrickEngine.PlayTrick(previousWinner, seats);
					scoreCount.Add(previousWinner.Winner.Index, previousWinner.WinnerPrice);
				}

				for (int i = 0; i < _players.Count(); i++)
				{
					if (bottleOwner.HasValue && bottleOwner.Value == i)
					{
						finalScores[i] -= bottleTax;
					}
					else
					{
						finalScores[i] += scoreCount.Score(i);
					}
				}
			}
			return finalScores;
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

		private static List<Play> AskMoves(Hand[] playerCards, IEnumerable<Seat> order)
		{
			var playedCards = new List<Play>();
			foreach (var seat in order)
			{
				var card = seat.Player.Play(playedCards.Select(p => p.Card).ToArray());
				//Check rule validity
				var playerHand = playerCards[seat.Index];
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

		private void GiveInitialCards(Hand[] playerCards, InitialMove[] initialMoves)
		{
			for (int i = 0; i < _players.Count(); i++)
			{
				var leftPlayerIndex = (i - 1) < 0 ? _players.Length - 1 : i;
				var rightPlayerIndex = (i + 1) % _players.Length;

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
