using System;
using HashCode2022.Entities;

namespace HashCode2022.Engine
{
    public class MatchingContributor
    {
        public string SkillName { get; init; }
        public Contributor Contributor { get; init; }
        public bool IsMentor { get; set; }

        public MatchingContributor(string skillName, Contributor contributor)
        {
            SkillName = skillName;
            Contributor = contributor;
            IsMentor = false;
        }
    }
}
