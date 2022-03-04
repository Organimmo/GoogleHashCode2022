using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ContributorStrategyC : IContributorStrategy
    {
        public IQueryable<Contributor> GetContributorOrder(IQueryable<Contributor> contributors, string targetSkillName, int targetSkillLevel)
        {
            return contributors
                .OrderBy(d => d.AvailableDate)
                .ThenBy(d => d.HighestSkillLevel)
                ;
        }

        public override string ToString()
        {
            return "CC";
        }
    }
}
