﻿using AiCandidates;
using Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace GameRunner
{
    public sealed class Program
    {
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<FirstCandidate>>();
            
            const int totalRounds = 1;
            var ais = new IGameAi[]
            {
                new FirstCandidate(logger),
                new AiCandidates.Random(),
                new AiCandidates.Random(),
                new AiCandidates.Random(),
            };
            var engine = new GameEngine(totalRounds, ais);
            var result = engine.Run();

            var total = result.Sum();
            var percents = result.Select(r => Percent(total, r)).ToList();

            Console.WriteLine($"Result: {percents[0]:0.00}%, {percents[1]:0.00}%, {percents[2]:0.00}%, {percents[3]:0.00}%");
            Console.WriteLine($"Result: {result[0]}, {result[1]}, {result[2]}, {result[3]}");
			Console.ReadKey();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(configure =>
            {
                configure.SetMinimumLevel(LogLevel.Trace);
                configure.AddConsole();
             });
        }

        private static double Percent(int total, int score)
        {
            return (score / (total * 1.0)) * 100;
        }
    }
}
