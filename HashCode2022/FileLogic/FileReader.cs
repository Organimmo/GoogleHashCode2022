using System.IO;
using HashCode2022.Entities;

namespace HashCode2022.FileLogic
{
    internal class FileReader
    {
        internal FileData ReadFrom(string fileName)
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
    }
}