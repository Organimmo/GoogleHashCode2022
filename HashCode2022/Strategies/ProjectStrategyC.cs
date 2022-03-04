using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ProjectStrategyC : IProjectStrategy
    {
        public IQueryable<Project> GetProjectOrder(IQueryable<Project> projects)
        {
            return projects
                .OrderBy(p => p.BestBefore)
                ;
        }

        public bool ShouldDropProject()
        {
            return true;
        }
    }
}
