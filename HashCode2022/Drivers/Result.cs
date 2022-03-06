using System.Collections.Generic;
using HashCode2022.Strategies;

namespace HashCode2022.Drivers
{
    internal class Result
    {
        public string? FileName { get; init; }
        public long Score { get; init; }
        public StrategySet? StrategySet { get; init; }
        public List<Result>? SubResults { get; init; }

        public Result(string fileName, long score, StrategySet strategySet)
        {
            FileName = fileName;
            Score = score;
            StrategySet = strategySet;
            SubResults = null;
        }

        public Result(long score, StrategySet strategySet, List<Result> subResults)
        {
            FileName = null;
            Score = score;
            StrategySet = strategySet;
            SubResults = subResults;
        }

        public Result(long score, List<Result> subResults)
        {
            FileName = null;
            Score = score;
            StrategySet = null;
            SubResults = subResults;
        }
    }
}
