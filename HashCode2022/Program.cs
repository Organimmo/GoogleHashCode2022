using HashCode2022.Entities;
using HashCode2022.FileLogic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            foreach (var file in Directory.GetFiles(inputPath, "*.txt").OrderBy(f => f))
            {
                RunSingleFile(file, Path.Combine(outputPath, Path.GetFileName(file)));
            }
        }

        static void RunSingleFile(string fileNameInput, string fileNameOutput)
        {
            Console.WriteLine($"Processing file {Path.GetFileName(fileNameInput)}");

            FileReader fileReader = new();
            FileData fileData = fileReader.ReadFrom(fileNameInput);

            // Run the algorithm
            Engine engine = new();
            var assignedProjects = engine.Run(fileData);

            // Calculate score
            ScoreCalculator scoreCalculator = new();
            var score = scoreCalculator.Calculate(assignedProjects);

            Console.WriteLine($"Finished file {Path.GetFileName(fileNameInput)} with score {score}");

            // Write the output
            FileWriter fileWriter = new();
            fileWriter.WriteTo(fileNameOutput, assignedProjects, fileData);
        }
    }
}
