using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ProjectStrategyF : IProjectStrategy
    {
        public IQueryable<Project> GetProjectOrder(IQueryable<Project> projects)
        {
            return projects
                .OrderBy(p => p.BestBefore)
                .ThenByDescending(p => p.Score)
                .ThenByDescending(p => p.HighestSkillLevel)
                ;
        }

        public bool ShouldDropProject()
        {
            return true;
        }
    }
}
