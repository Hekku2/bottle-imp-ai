using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.External;

namespace AiCandidates
{
	public class Random : IGameAi
	{
		private Hand _currentHand;

		public Random()
		{
		}

		public void GameStart(int numberOfPlayers, int numberOfRounds)
		{
			// Random doesnt care about this
		}

		public Card Play(Card[] cardsPlayed)
		{
			var startingCard = cardsPlayed.FirstOrDefault();
			if (startingCard == null)
			{
				return _currentHand.RemoveCard(_currentHand.Cards().FirstOrDefault());
			}

			var sameTypeCard = _currentHand.Cards().FirstOrDefault(c => c.Type == startingCard.Type);
			if (sameTypeCard != null)
			{
				_currentHand.RemoveCard(sameTypeCard);
				return sameTypeCard;
			}
			
			return _currentHand.RemoveCard(_currentHand.Cards().FirstOrDefault());
		}

		public void ReceivedCards(Card fromLeft, Card fromRight)
		{
			_currentHand.AddCard(fromLeft);
			_currentHand.AddCard(fromRight);
		}

		public void RoundFinished(RoundResult result)
		{
			//
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
