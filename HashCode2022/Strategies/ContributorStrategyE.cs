using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ContributorStrategyE : IContributorStrategy
    {
        public IQueryable<Contributor> GetContributorOrder(IQueryable<Contributor> contributors, string targetSkillName, int targetSkillLevel)
        {
            return contributors
                .OrderBy(d => d.HighestSkillLevel)
                .ThenBy(d => d.AvailableDate)
                ;
        }

        public override string ToString()
        {
            return "CE";
        }
    }
}
