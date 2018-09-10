using System;
using System.Collections.Generic;
using Core;
using Core.External;

namespace AiCandidates
{
	public class Random : IGameAi
	{
		public Random()
		{
		}

		public void GameStart(int numberOfPlayers, int numberOfRounds)
		{
			throw new NotImplementedException();
		}

		public Card Play(Card[] cardsPlayed)
		{
			throw new NotImplementedException();
		}

		public void ReceivedCards(Card fromLeft, Card fromRight)
		{
			throw new NotImplementedException();
		}

		public void RoundFinished(RoundResult result)
		{
			throw new NotImplementedException();
		}

		public InitialMove RoundStart(IEnumerable<Card> cards)
		{
			throw new NotImplementedException();
		}
	}
}
