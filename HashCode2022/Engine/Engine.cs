using HashCode2022.Entities;
using HashCode2022.Strategies;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HashCode2022.Engine
{
    public class Engine
    {
        private readonly string _context;
        private readonly IContributorStrategy _contributorStrategy;
        private readonly IProjectStrategy _projectStrategy;

        public List<Project> Run(FileData fileData)
        {
            return MatchProjectsAndContributors(fileData, fileData.Projects);
        }

        public List<Project> MatchProjectsAndContributors(FileData fileData, List<Project> projects)
        {
            List<Project> assignedList = new();
            List<Project> droppedList = new();
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
                    if (_context != null)
                    {
                        Console.WriteLine($"{_context}: Run {run}: {assignedList.Count} assigned ({assignedPrc}%), {notAssignedList.Count} not assigned, {droppedList.Count} dropped");
                    }
                }
                lastAssignedCount = 0;

                var previousNotAssignedList = notAssignedList;
                notAssignedList = new();

                foreach (var project in _projectStrategy.GetProjectOrder(previousNotAssignedList.AsQueryable()))
                {
                    var matchingContributors = ProjectCanBeAssigned(fileData, project);
                    if (matchingContributors != null)
                    {
                        if (AssignProject(project, matchingContributors))
                        {
                            lastAssignedCount++;
                            assignedList.Add(project);
                        }
                        else
                        {
                            droppedList.Add(project);
                        }
                    }
                    else
                    {
                        notAssignedList.Add(project);
                    }

                    if (_context != null && stopwatch.ElapsedMilliseconds >= nextProgress)
                    {
                        var processedPrc = Math.Round(100.0 * (assignedList.Count + notAssignedList.Count) / fileData.Projects.Count, 1);
                        var assignedPrc = Math.Round(100.0 * assignedList.Count / fileData.Projects.Count, 1);
                        Console.WriteLine($"{_context}: {stopwatch.ElapsedMilliseconds / 1000}s: {processedPrc}%, {assignedList.Count} assigned ({assignedPrc}%), {notAssignedList.Count} not assigned, {droppedList.Count} dropped");
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
            Skill ZeroSkill = new(projectSkill.Name, 0);

            foreach (var contributor in _contributorStrategy.GetContributorOrder(fileData.Contributors.AsQueryable(), projectSkill.Name, projectSkill.Level))
            {
                // skip if contributor already a match
                if (matchingContributors.Any(c => c.Contributor == contributor))
                {
                    continue;
                }

                var contrSkill = contributor.Skills.FirstOrDefault(s => s.Name == projectSkill.Name);

                if (contrSkill == null)
                {
                    if (projectSkill.Level > 1)
                    {
                        continue;
                    }
                    else
                    {
                        contrSkill = ZeroSkill;
                    }
                }

                // contributor must have at least project skill - 1
                if (contrSkill.Level < projectSkill.Level - 1)
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

        private bool AssignProject(Project project, List<MatchingContributor> matchingContributors)
        {
            // Sanity check...
            System.Diagnostics.Debug.Assert(matchingContributors.Count == project.Skills.Count);

            // First determine if the project is useful to be executed
            // We need start date first
            int startDate = 0;
            // bool projectHasMentors = false;
            for (int i = 0; i < matchingContributors.Count; i++)
            {
                var contributor = matchingContributors[i].Contributor;
                if (contributor.AvailableDate > startDate)
                {
                    startDate = contributor.AvailableDate;
                }

                // if (matchingContributors[i].IsMentor)
                // {
                //     projectHasMentors = true;
                // }
            }

            if (_projectStrategy.ShouldDropProject())
            {
                int overdue = startDate + project.Duration - project.BestBefore;
                // Score = startDate + duration 
                //if (overdue > 0 && overdue >= project.Score && !projectHasMentors)
                if (overdue > 0 && overdue >= project.Score)
                {
                    return false;
                }
            }

            for (int i = 0; i < matchingContributors.Count; i++)
            {
                var skillName = matchingContributors[i].SkillName;
                var contributor = matchingContributors[i].Contributor;
                var projectSkill = project.Skills[i];
                var isMentor = matchingContributors[i].IsMentor;

                // Sanity check...
                System.Diagnostics.Debug.Assert(skillName == projectSkill.Name);

                // Project can have dupplicate skills!
                var contributorSkill = contributor.Skills.FirstOrDefault(x => x.Name == skillName);
                if (contributorSkill == null)
                {
                    contributorSkill = new(skillName, 0);
                }

                System.Diagnostics.Debug.Assert(contributorSkill.Level >= projectSkill.Level - 1);

                // if skill level of contributor is same or 1 lower of project skill level --> increment contributor skill level
                // unless it is a mentor!
                if (!isMentor && (contributorSkill.Level == projectSkill.Level - 1 || contributorSkill.Level == projectSkill.Level))
                {
                    contributorSkill.IncrementLevel();
                }

                project.AssignedContributors.Add(contributor);
            }

            project.StartDate = startDate;

            UpdateAvailabilityDates(matchingContributors, startDate + project.Duration);
            return true;
        }


        private void UpdateAvailabilityDates(List<MatchingContributor> contributors, int newAvailability)
        {
            foreach (var contributor in contributors)
            {
                contributor.Contributor.AvailableDate = newAvailability;
            }
        }

        public Engine (string context, IContributorStrategy contributorStrategy, IProjectStrategy projectStrategy)
        {
            _context = context;
            _contributorStrategy = contributorStrategy;
            _projectStrategy = projectStrategy;
        }
    }
}
