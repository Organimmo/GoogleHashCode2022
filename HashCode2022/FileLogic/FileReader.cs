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
                columns = ReadLine(sr);

                var contributorCount = int.Parse(columns[0]); // C
                var projectCount = int.Parse(columns[1]); // P

                fileData.Contributors = new();
                fileData.Projects = new();

                // C sections of Contributor
                for (var contributorNo = 0; contributorNo < contributorCount; contributorNo++)
                {
                    // Line: <ContributorName> <SkillCount>
                    columns = ReadLine(sr);

                    string name = columns[0];
                    int skillCount = int.Parse(columns[1]); // N

                    var contributor = new Contributor(name);

                    // N skills of contributor
                    for (var skillNo = 0; skillNo < skillCount; skillNo++)
                    {
                        // Line: <SkillName> <SkillLevel>
                        columns = ReadLine(sr);
                        
                        var skill = new Skill(columns[0], int.Parse(columns[1]));
                        contributor.Skills.Add(skill);
                    }

                    fileData.Contributors.Add(contributor);
                }

                // P sections of Project
                for (var projectNo = 0; projectNo < projectCount; projectNo++)
                {
                    // Line: <ProjectName> <DurationDays> <Score> <BestBefore> <RoleCount>
                    columns = ReadLine(sr);

                    Project project = new Project(
                        name: columns[0],
                        duration: int.Parse(columns[1]),
                        score: int.Parse(columns[2]),
                        bestBefore: int.Parse(columns[3]));

                    int numberOfRoles = int.Parse(columns[4]);

                    for (var skillNo = 0; skillNo < numberOfRoles; skillNo++)
                    {
                        // Line: <SkillName> <SkillRequiredLevel>
                        columns = ReadLine(sr);

                        var skill = new Skill(columns[0], int.Parse(columns[1]));

                        project.Skills.Add(skill);
                        if (!fileData.Skills.Contains(skill)) fileData.Skills.Add(skill);
                    }

                    fileData.Projects.Add(project);
                }
            }

            return fileData;
        }

        private string[] ReadLine(StreamReader sr)
        {
            string? line = sr.ReadLine();
            if (line == null)
            {
                throw new InvalidDataException("File is not in correct format");
            }

            return line.Split();
        }
    }
}