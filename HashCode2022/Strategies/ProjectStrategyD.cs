using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ProjectStrategyD : IProjectStrategy
    {
        public IQueryable<Project> GetProjectOrder(IQueryable<Project> projects)
        {
            return projects
                .OrderBy(p => p.BestBefore)
                .ThenBy(p => p.Duration)
                ;
        }

        public bool ShouldDropProject()
        {
            return true;
        }
    }
}
