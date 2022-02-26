using System;
using System.IO;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.FileLogic
{
    internal class FileWriter
    {
        internal void WriteTo(string fileName, FileData fileData)
        {
            using (var sr = new StreamWriter(fileName))
            {
                var plannedProjects = fileData.Projects.Where(p => (p.AssignedContributors?.Count ?? 0) > 0);
                
                // Line: <PlannedProjectCount>
                sr.WriteLine($"{plannedProjects.Count()}");

                // Foreach Project
                foreach (var project in plannedProjects)
                {
                    // Line: <ProjectName>
                    sr.WriteLine(project.Name);


                    // Line: <ContributorName> <ContributorName> ...
                    var contributors = string.Join(' ', project.AssignedContributors.Select(c => c.Name));
                    sr.WriteLine(contributors);
                }
            }
        }
    }
}