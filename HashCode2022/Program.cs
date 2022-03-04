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
    internal partial class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: ./HashCode2022 <input_path> <output_path>");
                return;
            }
            string inputPath = args[0];
            string outputPath = args[1];

            if (!Directory.Exists(outputPath))
            {
                Console.WriteLine($"Output directory {outputPath} does not exist, creating it");
                Directory.CreateDirectory(outputPath);
            }

            var strategySets = GetStrategySets();

            long overallScore = 0;
            foreach (var file in Directory.GetFiles(inputPath, "*.txt").OrderBy(f => f))
            {
                overallScore += RunSingleFile(file, outputPath, strategySets);
            }

            Console.WriteLine($"Overall score: {overallScore}");
        }

        class StrategySet
        {
            public IContributorStrategy ContributorStrategy { get; init; }
            public IProjectStrategy ProjectStrategy { get; init; }

            public StrategySet(IContributorStrategy contributorStrategy, IProjectStrategy projectStrategy)
            {
                ContributorStrategy = contributorStrategy ?? throw new ArgumentNullException(nameof(contributorStrategy));
                ProjectStrategy = projectStrategy ?? throw new ArgumentNullException(nameof(projectStrategy));
            }

            public override string ToString()
            {
                return $"{ContributorStrategy}-{ProjectStrategy}";
            }
        }

        readonly static StrategySet[] strategies = {
            // new StrategySet(new ContributorStrategyA(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyD(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyE(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyG(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyA()),
            // new StrategySet(new ContributorStrategyA(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyD(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyE(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyG(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyB()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyC()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyD()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyI(), new ProjectStrategyE()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyF()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyG(false)),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyG()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyG()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyG()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyH()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyH()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyH()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyH()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyI()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyI()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyI()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyI()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyJ()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyJ()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyJ()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyJ()),

            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyK()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyK()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyK()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyK()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyL()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyL()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyL()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyL()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyM()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyM()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyM()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyM()),
            // new StrategySet(new ContributorStrategyB(), new ProjectStrategyN()),
            // new StrategySet(new ContributorStrategyC(), new ProjectStrategyN()),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyN()),
            // new StrategySet(new ContributorStrategyH(), new ProjectStrategyN()),
            new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -4.0)), // best for file "c" and "e"
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -7)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -8)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -9)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -10)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -1)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -0.5)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 0.5)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 1)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -10)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -5)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 5)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 10)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -100)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -50)),
            new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 50)), // Best for file "b"
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, 100)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -80)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -60)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -40)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -20)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -45)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -55)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -65)),
            new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -75)), // best for file "a"/"d"/"f"
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -67)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -70)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -72)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -77)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -73)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -74)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -76)),
            // new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, -5.6, -78)),
        };

        static long RunSingleFile(string fileNameInput, string outputPath, IEnumerable<StrategySet> strategySets)
        {
            long bestScore = 0;

            // foreach (StrategySet strategySet in strategies)
            // {
            //     long score = RunSingleFileWithSingleStrategySet(fileNameInput, outputPath, strategySet);
            //     if (score > bestScore)
            //     {
            //         bestScore = score;
            //     }
            // }

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

        static long RunSingleFileWithSingleStrategySet(string fileNameInput, string outputPath, StrategySet strategySet)
        {
            string context = $"{Path.GetFileName(fileNameInput)}/{strategySet}";
            Console.WriteLine($"{context}: Processing file");

            FileReader fileReader = new();
            FileData fileData = fileReader.ReadFrom(fileNameInput);

            // Run the algorithm
            Engine engine = new(context, strategySet.ContributorStrategy, strategySet.ProjectStrategy);
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

        static IEnumerable<StrategySet> GetStrategySets()
        {
            return strategies.AsEnumerable();

            // int count = 4;
            // double start = -7.0;
            // double stop = -6.0;

            // // Try to minimize number cluttering
            // double stepRaw = (stop - start) / (count + 1);
            // int digits = -(int)Math.Round(Math.Log10(stepRaw));
            // double stepIncrement = Math.Pow(10, -digits);
            // double step = Math.Floor(stepRaw / stepIncrement) * stepIncrement;
            // double middle = Math.Round((start + stop) / 2 / stepIncrement) * stepIncrement;

            // double first;
            // if (count % 2 == 1) // e.g count = 5, start/stop/step = 3/4/0.2 --> middle = 3.5 --> first = 3.1
            //     first = middle - (count / 2) * step;
            // else // e.g count = 4, start/stop/step = 3/4/0.2 --> middle = 3.5 --> first = 3.1
            //     first = middle - step / 2 - ((count - 1) / 2) * step;

            // for (int i = 0; i < count; i++)
            // {
            //     double number = Math.Round(first + i * step, digits);
            //     yield return new StrategySet(new ContributorStrategyF(), new ProjectStrategyO(1.0, number));
            // }
        }
    }
}
