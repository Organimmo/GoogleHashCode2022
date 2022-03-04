using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public interface IContributorStrategy
    {
        IQueryable<Contributor> GetContributorOrder(IQueryable<Contributor> contributors, string targetSkillName, int targetSkillLevel);
    }
}
