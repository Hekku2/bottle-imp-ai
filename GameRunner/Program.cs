using Core;
using System;

namespace GameRunner
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var ais = new[]
			{
				new AiCandidates.Random(),
				new AiCandidates.Random(),
				new AiCandidates.Random(),
				new AiCandidates.Random(),
			};
			var engine = new GameEngine(1, ais);
			var result = engine.Run();

			Console.WriteLine($"Result: {result[0]}, {result[1]}, {result[2]}, {result[3]}");
		}
	}
}
