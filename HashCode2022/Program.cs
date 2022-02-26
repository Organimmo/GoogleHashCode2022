using HashCode2022.Entities;
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
                Directory.CreateDirectory(outputPath);
            }

            foreach (var file in Directory.GetFiles(inputPath))
            {
                RunSingleFile(file, Path.Combine(outputPath, Path.GetFileName(file)));
            }
        }

        static void RunSingleFile(string fileNameInput, string fileNameOutput)
        {
            FileData fileData = ReadFile(fileNameInput);
            SetContributorZeroSkills(fileData);

            FileDataManager fileDataManager = new FileDataManager(fileData);
            fileDataManager.AssignAll();

            WriteFile(fileNameOutput, fileData);
        }

        static FileData ReadFile(string fileName)
        {
            var fileData = new FileData();
            using (var sr = new StreamReader(fileName))
            {
                string[] columns;

                // Line: <ContributorCount> <ProjectCount>
                columns = sr.ReadLine().Split();

                var contributorCount = int.Parse(columns[0]); // C
                var projectCount = int.Parse(columns[1]); // P

                fileData.Contributors = new();
                fileData.Projects = new();

                // C sections of Contributor
                for (var contributorNo = 0; contributorNo < contributorCount; contributorNo++)
                {
                    // Line: <ContributorName> <SkillCount>
                    columns = sr.ReadLine().Split();

                    string name = columns[0];
                    int skillCount = int.Parse(columns[1]); // N

                    var contributor = new Contributor()
                    {
                        Name = name,
                        Skills = new()
                    };

                    // N skills of contributor
                    for (var skillNo = 0; skillNo < skillCount; skillNo++)
                    {
                        // Line: <SkillName> <SkillLevel>
                        columns = sr.ReadLine().Split();
                        
                        var skill = new Skill()
                        {
                            Name = columns[0],
                            Level = int.Parse(columns[1])
                        };

                        contributor.Skills.Add(skill);
                    }

                    fileData.Contributors.Add(contributor);
                }

                // P sections of Project
                for (var projectNo = 0; projectNo < projectCount; projectNo++)
                {
                    // Line: <ProjectName> <DurationDays> <Score> <BestBefore> <RoleCount>
                    columns = sr.ReadLine().Split();

                    Project project = new Project()
                    {
                        Name = columns[0],
                        Duration = int.Parse(columns[1]),
                        Score = int.Parse(columns[2]),
                        BestBefore = int.Parse(columns[3]),
                        NumberOfRoles = int.Parse(columns[4]),
                        Skills = new()
                    };

                    for (var skillNo = 0; skillNo < project.NumberOfRoles; skillNo++)
                    {
                        // Line: <SkillName> <SkillRequiredLevel>
                        columns = sr.ReadLine().Split();

                        var skill = new Skill()
                        {
                            Name = columns[0],
                            Level = int.Parse(columns[1])
                        };

                        project.Skills.Add(skill);
                        if (!fileData.Skills.Contains(skill)) fileData.Skills.Add(skill);
                    }

                    fileData.Projects.Add(project);
                }
            }

            return fileData;
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
                        contributor.Skills.Add(
                            new Skill() 
                            { 
                                Name = skill, 
                                Level = 0
                            });
                    }
                }
            }
        }

        static void WriteFile(string fileName, FileData fileData)
        {
            Console.WriteLine("====================================");
            using (var sr = new StreamWriter(fileName))
            {
                var plannedProjects = fileData.Projects.Where(p => (p.AssignedContributors?.Count ?? 0) > 0);
                
                // Line: <PlannedProjectCount>
                sr.WriteLine($"{plannedProjects.Count()}");
                Console.WriteLine($"{plannedProjects.Count()}");

                // Foreach Project
                foreach (var project in plannedProjects)
                {
                    sr.WriteLine(project.Name);
                    Console.WriteLine(project.Name);

                    var contributors = string.Join(' ', project.AssignedContributors.Select(c => c.Name));
                    sr.WriteLine(contributors);
                    Console.WriteLine(contributors);
                }
            }
        }


        public int CalculateTotalScore(List<Project> projects)
        {
            int sum = 0;
            foreach (var project in projects)
            {
                sum += project.CalculateScore();
            }
            return sum;
        }

    }
}
