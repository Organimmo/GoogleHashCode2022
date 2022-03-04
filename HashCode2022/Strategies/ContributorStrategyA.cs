using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ContributorStrategyA : IContributorStrategy
    {
        public IQueryable<Contributor> GetContributorOrder(IQueryable<Contributor> contributors, string targetSkillName, int targetSkillLevel)
        {
            return contributors;
        }

        public override string ToString()
        {
            return "CA";
        }
    }
}
