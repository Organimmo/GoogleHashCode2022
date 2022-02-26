using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2022.Entities
{
    public class Project
    {
        public string Name { get; set; }

        public List<Skill> Skills { get; set; } = new();

        // added in order of skills
        public List<Contributor> AssignedContributors { get; set; } = new();

        public int Duration { get; set; }

        public int Score { get; set; }

        public int NumberOfRoles { get; set; }

        public int LastDayOfWork { get; set; }

        public int BestBefore { get; set; }

        public int HighestSkillLevel => Skills.Select(x => x.Level).Max();

        public int TotalSkillLevel => Skills.Select(x => x.Level).Sum();

        public int CalculateScore()
        {
            return -1; //score - (LastDayOfWork + 1 - BestBefore) > 0? score - (LastDayOfWork + 1 - BestBefore): 0;
        }

    }
}
