using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ProjectStrategyE : IProjectStrategy
    {
        public IQueryable<Project> GetProjectOrder(IQueryable<Project> projects)
        {
            return projects
                .OrderBy(p => p.BestBefore)
                .ThenByDescending(p => p.Duration)
                ;
        }

        public bool ShouldDropProject()
        {
            return true;
        }
    }
}
