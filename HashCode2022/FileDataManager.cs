using HashCode2022.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2022
{
    public class FileDataManager
    {
        private readonly FileData _filedata;
        private int projectWithoutContrubutersCount;

        public void AssignAll()
        {
            MatchProjectsAndContributors(_filedata.Projects);
        }

        public List<Project> MatchProjectsAndContributors(List<Project> projects)
        {
            List<Project> assignedList = new();
            List<Project> notAssignedList = new();
            if (!projects.Any())
            {
                return projects;
            }

            bool projectAssigned = false;
            List<Project> tempList = new();

            SortProjects();

            foreach (var project in projects)
            {
                var matchingContributors = ProjectCanBeAssigned(project);
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

                return MatchProjectsAndContributors(notAssignedList);
            }

            return null;
        }






        private List<Tuple<string, Contributor>> ProjectCanBeAssigned(Project project)
        {
            int startDate = 0;
            List<Tuple<string, Contributor>> matchingContributors = new();

            foreach (var projectSkill in project.Skills)
            {
                Contributor contributor = FindBestContributor(project, projectSkill, matchingContributors.Select(c => c.Item2).ToList());

                // Check if project can be assigned
                if (contributor == null)
                    return null;

                matchingContributors.Add(new Tuple<string, Contributor>(projectSkill.Name, contributor));
            }

            return matchingContributors;
        }

        private Contributor FindBestContributor(Project project, Skill projectSkill, List<Contributor> matchingContributors)
        {
            foreach (var contributor in _filedata.Contributors
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
            System.Diagnostics.Debug.Assert(matchingContributors.Count == project.Skills.Count);

            int startDate = 0;
            for (int i = 0; i < matchingContributors.Count; i++)
            {
                var contributor = matchingContributors[i].Item2;
                var projectSkill = project.Skills[i];
                System.Diagnostics.Debug.Assert(matchingContributors[i].Item1 == projectSkill.Name);


                // var contributorSkill = project.Skills.SingleOrDefault(x => x.Name == matchingContributors[i].Item1);
                var contributorSkill = project.Skills.FirstOrDefault(x => x.Name == matchingContributors[i].Item1);
                // if skill level of contributor is same or 1 lower of project skill level --> increment contributor skill level
                if (contributorSkill.Level <= projectSkill.Level)
                {
                    contributorSkill.Level++;
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

        public void SortProjects()
        {
            // Switch around to optimise
            _filedata.Projects
                .OrderBy(p => p.Duration)
                .OrderBy(p => p.TotalSkillLevel)
                .OrderBy(p => p.HighestSkillLevel)
                ;
        }

        public FileDataManager(FileData fileData)
        {
            _filedata = fileData;
        }
    }
}
