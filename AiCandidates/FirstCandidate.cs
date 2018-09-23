using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using Core.External;
using Microsoft.Extensions.Logging;

namespace AiCandidates
{
	/// <summary>
	/// 1. Win a hand if you can win without taking bottle
	/// 2. Give lowest possible card away if you can't win
	/// 3. If possible, remove lowers value card without taking bottle
	/// 4. Avoid bottle if possible
	/// </summary>
	public class FirstCandidate : IGameAi
	{
		private readonly ILogger _logger;

		private int _seatAtTable;
		private int _numberOfPlayers;
		private int _bottlePrice = 19;

		private Hand _currentHand;

		private readonly Stats _stats;

		public FirstCandidate(ILogger<FirstCandidate> logger, Stats stats)
		{
			_logger = logger;
			_stats = stats;
		}

		public void GameStart(int numberOfPlayers, int numberOfRounds, int seatAtTable)
		{
			_logger.LogInformation("Game started: {0} players, {1} rounds, seat at table: {2}", numberOfPlayers, numberOfRounds, seatAtTable);
			_seatAtTable = seatAtTable;
			_numberOfPlayers = numberOfPlayers;
		}

		public Card Play(Card[] cardsPlayed)
		{
			var startingCard = cardsPlayed.FirstOrDefault();
			var playableCards = GetPlayableCards(startingCard);
			if (startingCard == null)
			{
				_stats.RoundWins += 1;
				return _currentHand.RemoveCard(playableCards.HighestCard());
			}
			_stats.RoundLoses += 1;

			var winningCard = WinningCard(cardsPlayed);
			if (winningCard.Number < _bottlePrice)
			{
				var myLowest = playableCards.LowestCard();
				if (myLowest != null && myLowest.Number < winningCard.Number)
				{
					return _currentHand.RemoveCard(myLowest);
				}

				var lowestSafeCard = playableCards.LowestSafeCard(_bottlePrice);
				if (lowestSafeCard != null)
				{
					return _currentHand.RemoveCard(lowestSafeCard);
				}

				var highestDangerCard = playableCards.HighestCard();
				return _currentHand.RemoveCard(highestDangerCard);
			}

			if (OthersHavePlayed(cardsPlayed))
			{
				var biggest = cardsPlayed.Max(card => card.Number);
				var lowestWinningCard = playableCards.LowestWinningCard(biggest);

				if (lowestWinningCard != null)
				{
					return _currentHand.RemoveCard(lowestWinningCard);
				}

				var lowestSafeCard = playableCards.LowestSafeCard(_bottlePrice);
				if (lowestSafeCard != null)
				{
					return _currentHand.RemoveCard(lowestSafeCard);
				}
			}

			_stats.UndecidedPath += 1;
			return _currentHand.RemoveCard(playableCards.HighestCard());
		}

		private Card WinningCard(Card[] cardsPlayed)
		{
			var bottlePriceContender = cardsPlayed
				.Where(card => card.Number < _bottlePrice)
				.OrderByDescending(card => card.Number)
				.FirstOrDefault();

			if (bottlePriceContender != null)
			{
				return bottlePriceContender;
			}

			return cardsPlayed
				.OrderByDescending(card => card.Number)
				.FirstOrDefault();
		}

		private bool OthersHavePlayed(Card[] cardsPlayed)
		{
			return cardsPlayed.Length == _numberOfPlayers - 1;
		}

		private IEnumerable<Card> GetPlayableCards(Card startingCard)
		{
			if (startingCard == null)
			{
				return _currentHand.Cards();
			}

			var sameTypeCards = _currentHand.Cards().Where(c => c.Type == startingCard.Type);
			if (sameTypeCards.Any())
			{
				return sameTypeCards;
			}
			else
			{
				return _currentHand.Cards();
			}
		}

		public void ReceivedCards(Card fromLeft, Card fromRight)
		{
			_currentHand.AddCard(fromLeft);
			_currentHand.AddCard(fromRight);
		}

		public void TrickFinished(Core.External.TrickResult result)
		{
			var winningCard = WinningCard(result.PlayedCards);
			var myCard = result.PlayedCards[_seatAtTable];
			_logger.LogInformation("Round finished. Winning card {0}, my card {1}, bottle price {2}", winningCard, myCard, result.BottlePrice);
			_bottlePrice = result.BottlePrice;
		}

		public InitialMove RoundStart(IEnumerable<Card> cards)
		{
			_currentHand = new Hand(cards);

			var left = _currentHand.Cards()[0];
			var right = _currentHand.Cards()[1];
			var bottle = _currentHand.Cards()[2];
			_currentHand.RemoveCard(left);
			_currentHand.RemoveCard(right);
			_currentHand.RemoveCard(bottle);
			return new InitialMove(left, right, bottle);
		}
	}
}
