using Core.Internal;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class GameEngine
    {
        private readonly int _rounds;
        private IGameAi[] _players;

        public GameEngine(int rounds, IEnumerable<IGameAi> players)
        {
            _rounds = rounds;
            _players = players.ToArray();
        }

        public int[] Run()
        {
            var finalScores = new int[_players.Count()];

            //Inform about other players
            for (int seatIndex = 0; seatIndex < _players.Count(); seatIndex++)
            {
                _players[seatIndex].GameStart(_players.Count(), _rounds, seatIndex);
            }
            for (int roundNumber = 0; roundNumber < _rounds; roundNumber++)
            {
                var scores = new RoundEngine(_players).PlayRound();
				for (int i = 0; i < _players.Count(); i++)
                {
					finalScores[i] += scores[i];
				}
            }
            return finalScores;
        }
    }
}
