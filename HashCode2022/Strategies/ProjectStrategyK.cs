using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ProjectStrategyK : IProjectStrategy
    {
        public IQueryable<Project> GetProjectOrder(IQueryable<Project> projects)
        {
            return projects
                .OrderByDescending(p => ((double)p.Score) / p.Duration)
                .ThenByDescending(p => p.TotalSkillLevel)
                .ThenBy(p => p.HighestSkillLevel)
                ;
        }

        public bool ShouldDropProject()
        {
            return true;
        }
    }
}
