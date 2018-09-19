using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core;
using Core.External;

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
		private int _numberOfPlayers;
		private int _bottlePrice = 19;

		private Hand _currentHand;

		private readonly Stats _stats;

		public FirstCandidate(Stats stats)
		{
			_stats = stats;
		}

		public void GameStart(int numberOfPlayers, int numberOfRounds)
		{
			_numberOfPlayers = numberOfPlayers;
		}

		public Card Play(Card[] cardsPlayed)
		{
			var startingCard = cardsPlayed.FirstOrDefault();
			var playableCards = GetPlayableCards(startingCard);
			if (startingCard == null)
			{
				_stats.RoundWins += 1;

				var highestCard = playableCards
					.OrderByDescending(card => card.Number)
					.First();
				return _currentHand.RemoveCard(highestCard);
			}
			_stats.RoundLoses += 1;

			var winningCard = WinningCard(cardsPlayed);
			if (winningCard.Number < _bottlePrice)
			{
				var myLowest = playableCards.OrderByDescending(card => card.Number).FirstOrDefault();
				if (myLowest != null && myLowest.Number < winningCard.Number)
				{
					return _currentHand.RemoveCard(myLowest);
				}

				var lowestSafeCard = playableCards
					.Where(card => card.Number > _bottlePrice)
					.OrderBy(card => card.Number)
					.FirstOrDefault();
				if (lowestSafeCard != null)
				{
					return _currentHand.RemoveCard(lowestSafeCard);
				}

				var highestDangerCard = playableCards.OrderByDescending(card => card.Number).First();
				return _currentHand.RemoveCard(highestDangerCard);
			}

			if (OthersHavePlayed(cardsPlayed))
			{
				var biggest = cardsPlayed.Max(card => card.Number);
				var lowestWinningCard = playableCards
					.Where(card => card.Number > biggest)
					.OrderBy(card => card.Number)
					.FirstOrDefault();

				if (lowestWinningCard != null)
				{
					return _currentHand.RemoveCard(lowestWinningCard);
				}

				var lowestSafeCard = playableCards
					.Where(card => card.Number > _bottlePrice)
					.OrderBy(card => card.Number)
					.FirstOrDefault();
				if (lowestSafeCard != null)
				{
					return _currentHand.RemoveCard(lowestSafeCard);
				}
			}

			_stats.UndecidedPath += 1;
			return _currentHand.RemoveCard(playableCards.OrderByDescending(card => card.Number).First());
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

		public void RoundFinished(RoundResult result)
		{
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
