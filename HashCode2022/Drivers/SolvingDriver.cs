using HashCode2022.Strategies;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HashCode2022.Drivers
{
    internal class SolvingDriver : Driver
    {
        private struct Parameter<T>
        {
            public string Name { get; init; }
            public T Min { get; set; }
            public T Max { get; set; }
            public T Step { get; set; }
        }

        private readonly Parameter<double>[] parameters = new[]
        {
            new Parameter<double>
            {
                Name = "scoreFactor",
                Min = -10.0,
                Max = 10.0,
                Step = 0.2
            },
            new Parameter<double>
            {
                Name = "durationFactor",
                Min = -20.0,
                Max = 20.0,
                Step = 0.4
            },
            new Parameter<double>
            {
                Name = "highestSkillFactor",
                Min = -150.0,
                Max = 150.0,
                Step = 30,
            },
        };

        private readonly int divideCount = 4;

        public SolvingDriver(string inputPath, string outputPath, int fileNo = -1)
        : base(inputPath, outputPath, fileNo)
        {
        }

        protected override IEnumerable<StrategySet> GetStrategySets()
        {
            var p0 = GetParameterValues(0);
            var p1 = GetParameterValues(1);
            var p2 = GetParameterValues(2);

            foreach (var v0 in p0)
            foreach (var v1 in p1)
            foreach (var v2 in p2)
                yield return new StrategySet(
                    new ContributorStrategyF(),
                    new ProjectStrategyO(v0, v1, v2));
        }

        private double[] GetParameterValues(int parameterNo)
        {
            double[] values = new double[divideCount];
            for (int j = 0; j < divideCount; j++)
            {
                var parameter = parameters[parameterNo];
                double step = (parameter.Max - parameter.Min) / (divideCount + 1);
                values[j] =  parameter.Min + step * (j + 1);
                parameter.Step = step;
            }

            return values;
        }

        public override Result Run()
        {
            List<Result> results = new();

            long bestScore = long.MinValue;

            int countNotImproved = 0;
            const int maxCountNotImproved = 3;
            do
            {
                var result = base.Run();
                results.Add(result);

                if (result.Score > bestScore)
                {
                    bestScore = result.Score;
                    countNotImproved = 0;
                }
                else
                {
                    countNotImproved++;
                    if (countNotImproved >= maxCountNotImproved)
                    {
                        break;
                    }
                }

                Console.WriteLine($"Last run best score: {result.StrategySet} {result.Score}");
                Console.WriteLine($"Overall best score so far: {bestScore}");

                if (result.StrategySet?.ProjectStrategy is ProjectStrategyO projectStrategy)
                {
                    parameters[0].Min = projectStrategy.ScoreFactor - parameters[0].Step;
                    parameters[1].Min = projectStrategy.DurationFactor - parameters[1].Step;
                    parameters[2].Min = projectStrategy.HighestSkillLevelFactor - parameters[2].Step;
                    parameters[0].Max = projectStrategy.ScoreFactor + parameters[0].Step;
                    parameters[1].Max = projectStrategy.DurationFactor + parameters[1].Step;
                    parameters[2].Max = projectStrategy.HighestSkillLevelFactor + parameters[2].Step;
                }
            } while (true);

            var bestResult = results.OrderByDescending(x => x.Score).First();
            
            return new Result(bestResult.Score, bestResult.StrategySet ?? throw new InvalidOperationException(), results);
        }
    }
}
