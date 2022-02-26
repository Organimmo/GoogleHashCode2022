using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HashCode2022.Entities
{
    public class Contributor
    {
        public string Name { get; set; }

        public List<Skill> Skills { get; set; } = new();

        public int AvailableDate { get; set; }

        public int HighestSkillLevel => Skills.Select(x => x.Level).Max();
    }


    public class ContributorOccupation
    {
        public List<Contributor> Skills { get; set; } = new();

        public int Day { get; set; }

    }
}
