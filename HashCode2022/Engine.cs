using HashCode2022.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2022
{
    public class Engine
    {
        public List<Project> Run(FileData fileData)
        {
            SetContributorZeroSkills(fileData);

            return MatchProjectsAndContributors(fileData, fileData.Projects);
        }

        public List<Project> MatchProjectsAndContributors(FileData fileData, List<Project> projects)
        {
            List<Project> assignedList = new();
            List<Project> notAssignedList = fileData.Projects;

            int run = 0;
            int lastAssignedCount;
            do
            {
                Stopwatch stopwatch = new();
                stopwatch.Start();
                var nextProgress = 60000;

                {
                    var assignedPrc = Math.Round(100.0 * assignedList.Count / fileData.Projects.Count, 1);
                    Console.WriteLine($"Run {run}: {assignedList.Count} assigned ({assignedPrc}%), {notAssignedList.Count} not assigned");
                }
                lastAssignedCount = 0;

                var previousNotAssignedList = notAssignedList;
                notAssignedList = new();

                foreach (var project in SortProjects(previousNotAssignedList))
                {
                    var matchingContributors = ProjectCanBeAssigned(fileData, project);
                    if (matchingContributors != null)
                    {
                        lastAssignedCount++;
                        AssignProject(project, matchingContributors);
                        assignedList.Add(project);
                    }
                    else
                    {
                        notAssignedList.Add(project);
                    }

                    if (stopwatch.ElapsedMilliseconds >= nextProgress)
                    {
                        var processedPrc = Math.Round(100.0 * (assignedList.Count + notAssignedList.Count) / fileData.Projects.Count, 1);
                        var assignedPrc = Math.Round(100.0 * assignedList.Count / fileData.Projects.Count, 1);
                        Console.WriteLine($"{stopwatch.ElapsedMilliseconds / 1000}s: {processedPrc}%, {assignedList.Count} assigned ({assignedPrc}%), {notAssignedList.Count} not assigned");
                        nextProgress += 60000;
                    }
                }
                run++;
            } while (lastAssignedCount > 0);

            return assignedList;
        }

        private List<MatchingContributor>? ProjectCanBeAssigned(FileData fileData, Project project)
        {
            List<MatchingContributor> matchingContributors = new();

            foreach (var projectSkill in project.Skills)
            {
                Contributor? contributor = FindBestContributor(fileData, project, projectSkill, matchingContributors);

                // Check if contributor can be assigned
                if (contributor == null)
                    return null;

                matchingContributors.Add(new MatchingContributor(projectSkill.Name, contributor));
            }

            return matchingContributors;
        }

        private Contributor? FindBestContributor(FileData fileData, Project project, Skill projectSkill, List<MatchingContributor> matchingContributors)
        {
            foreach (var contributor in fileData.Contributors
                .OrderBy(d => d.AvailableDate)
                .OrderByDescending(d => d.HighestSkillLevel)
                )
            {
                // skip if contributor already a match
                if (matchingContributors.Any(c => c.Contributor == contributor))
                {
                    continue;
                }

                var contrSkill = contributor.Skills.FirstOrDefault(s => s.Name == projectSkill.Name);

                if (contrSkill == null)
                {
                    continue;
                }

                // contributor must have at least project skill - 1
                if (contrSkill.Level < projectSkill.Level - 1)
                // if (contrSkill.Level < projectSkill.Level)
                {
                    continue;
                }

                // check if there is a possible mentor
                if (contrSkill.Level == projectSkill.Level - 1)
                {
                    var potentialMentor = matchingContributors.FirstOrDefault(c => c.Contributor.Skills.Any(s => s.Name == projectSkill.Name && s.Level >= projectSkill.Level));

                    if (potentialMentor == null)
                    {
                        continue;
                    }

                    // Set mentor
                    potentialMentor.IsMentor = true;
                }

                return contributor;
            }

            // No contributor found
            return null;
        }

        private void AssignProject(Project project, List<MatchingContributor> matchingContributors)
        {
            // Sanity check...
            System.Diagnostics.Debug.Assert(matchingContributors.Count == project.Skills.Count);

            int startDate = 0;
            for (int i = 0; i < matchingContributors.Count; i++)
            {
                var skillName = matchingContributors[i].SkillName;
                var contributor = matchingContributors[i].Contributor;
                var projectSkill = project.Skills[i];
                var isMentor = matchingContributors[i].IsMentor;

                // Sanity check...
                System.Diagnostics.Debug.Assert(skillName == projectSkill.Name);

                // Project can have dupplicate skills!
                var contributorSkill = contributor.Skills.First(x => x.Name == skillName);

                System.Diagnostics.Debug.Assert(contributorSkill.Level >= projectSkill.Level - 1);

                // if skill level of contributor is same or 1 lower of project skill level --> increment contributor skill level
                // unless it is a mentor!
                if (!isMentor && (contributorSkill.Level == projectSkill.Level - 1 || contributorSkill.Level == projectSkill.Level))
                {
                    contributorSkill.IncrementLevel();
                }

                // startdata of project is last available
                if (contributor.AvailableDate > startDate)
                {
                    startDate = contributor.AvailableDate;
                }

                project.StartDate = startDate;
                project.AssignedContributors.Add(contributor);
            }

            UpdateAvailabilityDates(matchingContributors, startDate + project.Duration);
        }


        private void UpdateAvailabilityDates(List<MatchingContributor> contributors, int newAvailability)
        {
            foreach (var contributor in contributors)
            {
                contributor.Contributor.AvailableDate = newAvailability;
            }
        }

        private IEnumerable<Project> SortProjects(List<Project> projects)
        {
            // Switch around to optimise
            return projects
                .OrderBy(p => p.Duration)
                .OrderBy(p => p.TotalSkillLevel)
                .OrderBy(p => p.HighestSkillLevel)
                ;
        }

        private void SetContributorZeroSkills(FileData fileData)
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
