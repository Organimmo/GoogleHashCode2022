using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2022.Entities
{
    public class Project
    {
        public string Name { get; init; }

        public List<Skill> Skills { get; init; } = new();

        // added in order of skills
        public List<Contributor> AssignedContributors { get; init; } = new();

        public int Duration { get; init; }

        public int Score { get; init; }

        public int BestBefore { get; init; }

        public int HighestSkillLevel => Skills.Select(x => x.Level).Max();

        public int TotalSkillLevel => Skills.Select(x => x.Level).Sum();

        public Project(string name, int duration, int score, int bestBefore)
        {
            Name = name;
            Duration = duration;
            Score = score;
            BestBefore = bestBefore;
        }
    }
}
