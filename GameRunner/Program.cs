using AiCandidates;
using Core;
using System;
using System.Linq;

namespace GameRunner
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var stats = new Stats();

			const int totalRounds = 200;
			var ais = new IGameAi[]
			{
				new FirstCandidate(stats),
				new AiCandidates.Random(),
				new AiCandidates.Random(),
				new AiCandidates.Random(),
			};
			var engine = new GameEngine(totalRounds, ais);
			var result = engine.Run();

			var total = result.Sum();
			var percents = result.Select(r => Percent(total, r)).ToList();

			Console.WriteLine($"Result: {percents[0]:0.00}%, {percents[1]:0.00}%, {percents[2]:0.00}%, {percents[3]:0.00}%");

			Console.WriteLine($"Round won {stats.RoundWins}, lost {stats.RoundLoses}, undecided paths {stats.UndecidedPath}");
		}

		private static double Percent(int total, int score)
		{
			return (score / (total*1.0)) * 100;
		}
	}
}
