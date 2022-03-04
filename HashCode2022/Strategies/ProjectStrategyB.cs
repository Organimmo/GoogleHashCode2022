using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ProjectStrategyB : IProjectStrategy
    {
        public IQueryable<Project> GetProjectOrder(IQueryable<Project> projects)
        {
            return projects
                .OrderBy(p => p.Duration)
                .ThenBy(p => p.TotalSkillLevel)
                .ThenBy(p => p.HighestSkillLevel)
                ;
        }

        public bool ShouldDropProject()
        {
            return true;
        }
    }
}
