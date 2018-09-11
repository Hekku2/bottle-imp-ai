using System.Collections.Generic;
using System.Linq;

namespace Core.Internal
{
	public class ScoreCount
	{
		private readonly List<ScorePile> _scores;

		public ScoreCount(int players)
		{
			_scores = Enumerable.Range(0, players).Select(i => new ScorePile()).ToList();
		}

		public void Add(int index, IEnumerable<Card> cards)
		{
			_scores[index].Add(cards);
		}

		public int Score(int index)
		{
			return _scores[index].Score;
		}

		private class ScorePile
		{
			private List<Card> _cards = new List<Card>();

			public int Score
			{
				get
				{
					return _cards.Sum(card => card.Score);
				}
			}

			public void Add(IEnumerable<Card> cards)
			{
				_cards.AddRange(cards);
			}
		}
	}
}
