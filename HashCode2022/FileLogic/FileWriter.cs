using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.FileLogic
{
    internal class FileWriter
    {
        internal void WriteTo(string fileName, List<Project> assignedProjects, FileData fileData)
        {
            using (var sr = new StreamWriter(fileName))
            {
                // Line: <PlannedProjectCount>
                sr.WriteLine($"{assignedProjects.Count()}");

                // Foreach Project
                foreach (var project in assignedProjects)
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