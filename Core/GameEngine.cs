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
		
		public void Run()
		{
			//Inform about other players
			foreach (var player in _players)
			{
				player.GameStart(_players.Count(), _rounds);
			}
			for (int x = 0; x < _rounds; x++)
			{
				var cards = CardCollection.AllCards();
				//suffle cards
				//Deal cards
				var cardCount = 36 / _players.Count();
				var playerInitialCards = new Hand[_players.Count()];
				for (int i = 0; i < _players.Count(); i++)
				{
					var playerHand = new Hand(cards.Take(cardCount));
					playerInitialCards[i] = playerHand;
				}

				//ask for initial move
				var initialMoves = playerInitialCards.Select((playerCards, i) => 
				{
					var initialMove = _players[i].RoundStart(playerCards.Cards());
					if (!InitialMoveRule.IsLegal(playerCards, initialMove))
					{
						throw new Exception("Illegal move");
					}
					return initialMove;
				}).ToArray();

				//Set bottle cards
				var bottleTax = initialMoves.Sum(move => move.ToBottle.Score);

				//Give player cards
				for (int i = 0; i < _players.Count(); i++)
				{
					var leftPlayerIndex = (i - 1) < 0 ? _players.Length : i-1;
					var rightPlayerIndex = (i + 1) % _players.Length;

					_players[i].ReceivedCards(initialMoves[leftPlayerIndex].ToRight, initialMoves[rightPlayerIndex].ToLeft);
				}

				var scoreCount = new ScoreCount(_players.Count());
				var bottlePrice = 19;
				int? bottleOwner = null;

				// Decide starter
				var startingPlayerIndex = 0;
				for (int i = 0; i < cardCount - 1; i++)
				{
					// Create player order
					var order = Enumerable.Range(0, _players.Count())
						.Select(index => 
						{
							var current = (startingPlayerIndex + index) % _players.Length;
							return new Seat(current, _players[current]);
						});

					// Ask move from players
					var playedCards = new List<Play>();
					foreach (var seat in order)
					{
						var card = seat.Player.Play(playedCards.Select(p => p.Card).ToArray());
						//Check rule validity
						//Remove card from player
						playedCards.Add(new Play(seat.Index, card));
					}

					// Decide winner
					// TODO Get index of winning player
					Play winning = new Play(startingPlayerIndex, null);
					foreach (var playedCard in playedCards)
					{
						if (IsWinningRule.Wins(bottlePrice, winning.Card, playedCard.Card))
						{
							winning = playedCard;
						}
					}
					if (winning.Card.Number < bottlePrice)
					{
						bottleOwner = winning.Index;
						bottlePrice = winning.Card.Number;
					}
					scoreCount.Add(winning.Index, playedCards.Select(p => p.Card).ToList());

					var result = new RoundResult(bottlePrice, playedCards.Select(p => p.Card).ToArray());
					foreach (var player in _players)
					{
						player.RoundFinished(result);
					}
				}
			}
		}
	}
}
