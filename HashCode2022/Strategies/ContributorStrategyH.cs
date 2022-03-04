using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ContributorStrategyH : ContributorStrategyF
    {
        public override IQueryable<Contributor> GetContributorOrder(IQueryable<Contributor> contributors, string targetSkillName, int targetSkillLevel)
        {
            return contributors
                .OrderBy(c => c.AvailableDate)
                .ThenBy(c => Priority(c, targetSkillName, targetSkillLevel))
                .ThenBy(c => c.HighestSkillLevel)
                ; 
            // Prio: targetSkill, targetSkill - 1, targetSkill + 1, targetSkill + 2, ...
        }


        public override string ToString()
        {
            return "CH";
        }
    }
}
