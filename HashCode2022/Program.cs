using HashCode2022.Drivers;
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

            Driver driver = new FixedDriver(inputPath, outputPath);
            // Driver driver = new FixedDriver(inputPath, outputPath, 1);
            // Driver driver = new SolvingDriver(inputPath, outputPath, 5);
            Result result = driver.Run();

            Console.WriteLine($"Overall best score: {result.StrategySet} {result.Score}");
        }
    }
}
