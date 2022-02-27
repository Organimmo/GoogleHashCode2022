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
            Console.WriteLine("Processing file " + Path.GetFileName(fileNameInput));

            FileReader fileReader = new();
            FileData fileData = fileReader.ReadFrom(fileNameInput);

            SetContributorZeroSkills(fileData);

            // Run the algorithm
            FileDataManager fileDataManager = new(fileData);
            fileDataManager.AssignAll();

            FileWriter fileWriter = new();
            fileWriter.WriteTo(fileNameOutput, fileData);
        }

        static void SetContributorZeroSkills(FileData fileData)
        {
            List<string> allSkills = new();
            foreach (var project in fileData.Projects)
            {
                foreach (var skill in project.Skills)
                {
                    allSkills.Add(skill.Name);
                }
            }

            allSkills = allSkills.Distinct().ToList();

            foreach (var contributor in  fileData.Contributors)
            {
                foreach (var skill in allSkills)
                {
                    if (!contributor.Skills.Any(s => s.Name == skill))
                    {
                        contributor.Skills.Add(new Skill(skill));
                    }
                }
            }
        }
    }
}
