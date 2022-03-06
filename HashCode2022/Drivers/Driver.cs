using HashCode2022.Engine;
using HashCode2022.Entities;
using HashCode2022.FileLogic;
using HashCode2022.Strategies;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HashCode2022.Drivers
{
    internal abstract class Driver
    {
        private readonly string inputPath;
        private readonly string outputPath;
        private readonly int fileNo;

        // private long RunSingleFile(string fileNameInput, string outputPath, IEnumerable<StrategySet> strategySets)
        // {
        //     // foreach (StrategySet strategySet in strategies) 
        //     // { 
        //     //     long score = RunSingleFileWithSingleStrategySet(fileNameInput, outputPath, strategySet); 
        //     //     if (score > bestScore) 
        //     //     { 
        //     //         bestScore = score; 
        //     //     } 
        //     // }

        //     long bestScore = 0;

        //     List<Tuple<StrategySet, Task<long>>> tasks = new();
        //     foreach (StrategySet strategySet in strategySets)
        //     {

        //         tasks.Add(
        //             new Tuple<StrategySet, Task<long>>(
        //                 strategySet, 
        //                 Task.Run(() => RunSingleFileWithSingleStrategySet(fileNameInput, outputPath, strategySet))));
        //     }

        //     Task.WaitAll(tasks.Select(s => s.Item2).ToArray());
        //     Console.WriteLine($"Summary for: {Path.GetFileName(fileNameInput)}:");
        //     foreach (var task in tasks)
        //     {
        //         var result = task.Item2.Result;
        //         Console.WriteLine($"{task.Item1.ToString()}: {result}");
        //         if (result > bestScore)
        //         {
        //             bestScore = result;
        //         }
        //     }

        //     return bestScore;
        // }

        private Result RunSingleFileWithSingleStrategySet(string fileNameInput, string outputPath, StrategySet strategySet)
        {
            string context = $"{Path.GetFileName(fileNameInput)}/{strategySet}";
            Console.WriteLine($"{context}: Processing file");

            FileReader fileReader = new();
            FileData fileData = fileReader.ReadFrom(fileNameInput);

            // Run the algorithm
            Engine.Engine engine = new(context, strategySet.ContributorStrategy, strategySet.ProjectStrategy);
            var assignedProjects = engine.Run(fileData);

            // Calculate score
            ScoreCalculator scoreCalculator = new();
            var score = scoreCalculator.Calculate(assignedProjects);

            Console.WriteLine($"{context}: ==> Finished with score {score}");

            // Write the output
            FileWriter fileWriter = new();
            var outputPath2 = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(fileNameInput), strategySet.ToString());
            if (!Directory.Exists(outputPath2))
            {
                Directory.CreateDirectory(outputPath2);
            }
            fileWriter.WriteTo(Path.Combine(outputPath2, "out.txt"), assignedProjects, fileData);

            return new Result(Path.GetFileName(fileNameInput), score, strategySet);
        }

        protected abstract IEnumerable<StrategySet> GetStrategySets();

        public virtual Result RunSingleStrategySet(StrategySet strategySet)
        {
            var allFiles = Directory.GetFiles(inputPath, "*.txt").OrderBy(f => f);
            IEnumerable<string> files;
            if (fileNo == -1)
                files = allFiles;
            else
                files = allFiles.Skip(fileNo).Take(1);

            List<Result> results = new();
            foreach (var file in files)
            {
                var result = RunSingleFileWithSingleStrategySet(file, outputPath, strategySet);
                results.Add(result);
            }

            var overallScore = results.Sum(r => r.Score);
            return new Result(overallScore, strategySet, results); 
        }

        public virtual Result Run()
        {
            var strategySets = GetStrategySets();

            ConcurrentQueue<StrategySet> strategySetQueue = new(strategySets);
            ConcurrentBag<Result> results = new();

            List<Task> tasks = new();
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                tasks.Add(
                    Task.Run(() =>
                    {
                        while (strategySetQueue.TryDequeue(out StrategySet? strategySet) && strategySet != null)
                        {
                            Result result = RunSingleStrategySet(strategySet);
                            results.Add(result);
                        }
                    }));
            }

            Task.WaitAll(tasks.ToArray());

            foreach (var result in results)
            {
                Console.WriteLine($"{result.StrategySet?.ToString()}: {result.Score}");
            }

            var bestResult = results.OrderByDescending(r => r.Score).First();

            return new Result(bestResult.Score, bestResult.StrategySet ?? throw new InvalidOperationException(), results.ToList());
        }

        public Driver(string inputPath, string outputPath, int fileNo = -1)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
            this.fileNo = fileNo;
        }
    }
}
