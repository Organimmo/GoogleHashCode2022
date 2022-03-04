using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ContributorStrategyF : IContributorStrategy
    {
        public virtual IQueryable<Contributor> GetContributorOrder(IQueryable<Contributor> contributors, string targetSkillName, int targetSkillLevel)
        {
            return contributors
                .OrderBy(c => c.AvailableDate)
                .ThenBy(c => Priority(c, targetSkillName, targetSkillLevel))
                ; 
            // Prio: targetSkill, targetSkill - 1, targetSkill + 1, targetSkill + 2, ...
        }

        protected int Priority(Contributor contributor, string targetSkillName, int targetSkillLevel)
        {
            Skill? skill = contributor.Skills.FirstOrDefault(s => s.Name == targetSkillName);
            if (skill == null)
            {
                if (targetSkillLevel == 1)
                {
                    return 1; // "second"
                }
                else
                {
                    return int.MaxValue; // "lowest prio"
                }
            }

            if (skill.Level == targetSkillLevel)
            {
                return 0; // "first"
            }
            else if (skill.Level == targetSkillLevel - 1)
            {
                return 1; // "second"
            }
            else if (skill.Level < targetSkillLevel - 1)
            {
                return int.MaxValue; // "lowest prio"
            }

            return targetSkillLevel - skill.Level + 2; // "third", "fourth", ...
        }


        public override string ToString()
        {
            return "CF";
        }
    }
}
