using HashCode2022.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2022
{
    public class Engine
    {
        public void Run(FileData fileData)
        {
            SetContributorZeroSkills(fileData);

            MatchProjectsAndContributors(fileData, fileData.Projects);
        }

        public List<Project>? MatchProjectsAndContributors(FileData fileData, List<Project> projects)
        {
            List<Project> assignedList = new();
            List<Project> notAssignedList = new();

            foreach (var project in SortProjects(fileData.Projects))
            {
                var matchingContributors = ProjectCanBeAssigned(fileData, project);
                if (matchingContributors != null)
                {
                    AssignProject(project, matchingContributors);
                    assignedList.Add(project);

                }
                else
                {
                    notAssignedList.Add(project);
                }
            }

            if (assignedList.Any())            {

                return MatchProjectsAndContributors(fileData, notAssignedList);
            }

            return null;
        }

        private List<Tuple<string, Contributor>>? ProjectCanBeAssigned(FileData fileData, Project project)
        {
            List<Tuple<string, Contributor>> matchingContributors = new();

            foreach (var projectSkill in project.Skills)
            {
                Contributor? contributor = FindBestContributor(fileData, project, projectSkill, matchingContributors.Select(c => c.Item2).ToList());

                // Check if project can be assigned
                if (contributor == null)
                    return null;

                matchingContributors.Add(new Tuple<string, Contributor>(projectSkill.Name, contributor));
            }

            return matchingContributors;
        }

        private Contributor? FindBestContributor(FileData fileData, Project project, Skill projectSkill, List<Contributor> matchingContributors)
        {
            foreach (var contributor in fileData.Contributors
                .OrderBy(d => d.AvailableDate)
                .OrderByDescending(d => d.HighestSkillLevel))
            {
                // skip if contributor already a match
                if (matchingContributors.Contains(contributor))
                {
                    continue;
                }

                var contrSkill = contributor.Skills.FirstOrDefault(s => s.Name == projectSkill.Name);

                if (contrSkill == null)
                {
                    // TODO somewhere Add skill with level 0
                    continue;
                }

                // contributor must have at least project skill - 1
                // if (contrSkill.Level < projectSkill.Level - 1)
                if (contrSkill.Level < projectSkill.Level)
                {
                    continue;
                }

                // check if there is a possible mentor
                if (contrSkill.Level == projectSkill.Level - 1 && matchingContributors.Any())
                {
                    var potentialMentor = matchingContributors.FirstOrDefault(c => c.Skills.Any(s => s.Name == projectSkill.Name && s.Level >= projectSkill.Level));

                    if (potentialMentor == null)
                    {
                        continue;
                    }
                }

                return contributor;
            }

            // No contributor found
            return null;
        }

        private void AssignProject(Project project, List<Tuple<string, Contributor>> matchingContributors)
        {
            // Sanity check...
            System.Diagnostics.Debug.Assert(matchingContributors.Count == project.Skills.Count);

            int startDate = 0;
            for (int i = 0; i < matchingContributors.Count; i++)
            {
                var contributor = matchingContributors[i].Item2;
                var projectSkill = project.Skills[i];

                // Sanity check...
                System.Diagnostics.Debug.Assert(matchingContributors[i].Item1 == projectSkill.Name);

                // var contributorSkill = project.Skills.SingleOrDefault(x => x.Name == matchingContributors[i].Item1);
                var contributorSkill = project.Skills.First(x => x.Name == matchingContributors[i].Item1);
                // if skill level of contributor is same or 1 lower of project skill level --> increment contributor skill level
                if (contributorSkill.Level <= projectSkill.Level)
                {
                    contributorSkill.IncrementLevel();
                }

                // startdata of project is last available
                if (contributor.AvailableDate > startDate)
                {
                    startDate = contributor.AvailableDate;
                }

                project.AssignedContributors.Add(contributor);
            }

            UpdateAvailabilityDates(matchingContributors, startDate + project.Duration);
        }


        public void UpdateAvailabilityDates(List<Tuple<string, Contributor>> contributors, int newAvailability)
        {
            foreach (var contributor1 in contributors)
            {
                contributor1.Item2.AvailableDate = newAvailability;
            }
        }

        public IEnumerable<Project> SortProjects(List<Project> projects)
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
