using HashCode2022.Engine;
using HashCode2022.Entities;
using HashCode2022.FileLogic;
using HashCode2022.Strategies;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HashCode2022
{
    internal abstract class Driver
    {
        private readonly string inputPath;
        private readonly string outputPath;
        private readonly int fileNo;

        private long RunSingleFile(string fileNameInput, string outputPath, IEnumerable<StrategySet> strategySets)
        {
            // foreach (StrategySet strategySet in strategies) 
            // { 
            //     long score = RunSingleFileWithSingleStrategySet(fileNameInput, outputPath, strategySet); 
            //     if (score > bestScore) 
            //     { 
            //         bestScore = score; 
            //     } 
            // }

            long bestScore = 0;

            List<Tuple<StrategySet, Task<long>>> tasks = new();
            foreach (StrategySet strategySet in strategySets)
            {

                tasks.Add(
                    new Tuple<StrategySet, Task<long>>(
                        strategySet, 
                        Task.Run(() => RunSingleFileWithSingleStrategySet(fileNameInput, outputPath, strategySet))));
            }

            Task.WaitAll(tasks.Select(s => s.Item2).ToArray());
            Console.WriteLine($"Summary for: {Path.GetFileName(fileNameInput)}:");
            foreach (var task in tasks)
            {
                var result = task.Item2.Result;
                Console.WriteLine($"{task.Item1.ToString()}: {result}");
                if (result > bestScore)
                {
                    bestScore = result;
                }
            }

            return bestScore;
        }

        private long RunSingleFileWithSingleStrategySet(string fileNameInput, string outputPath, StrategySet strategySet)
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

            return score;
        }

        protected abstract IEnumerable<StrategySet> GetStrategySets();

        public long Run()
        {
            var strategySets = GetStrategySets();

            var allFiles = Directory.GetFiles(inputPath, "*.txt").OrderBy(f => f);
            IEnumerable<string> files;
            if (fileNo == -1)
                files = allFiles;
            else
                files = allFiles.Skip(fileNo).Take(1);

            long overallScore = 0;
            foreach (var file in files)
            {
                overallScore += RunSingleFile(file, outputPath, strategySets);
            }

            return overallScore; 
        }

        public Driver(string inputPath, string outputPath, int fileNo = -1)
        {
            this.inputPath = inputPath;
            this.outputPath = outputPath;
            this.fileNo = fileNo;
        }
    }
}
