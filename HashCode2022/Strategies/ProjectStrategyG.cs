using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ProjectStrategyG : IProjectStrategy
    {
        private readonly bool shouldDropProject;

        public ProjectStrategyG(bool shouldDropProject = true)
        {
            this.shouldDropProject = shouldDropProject;
        }

        public IQueryable<Project> GetProjectOrder(IQueryable<Project> projects)
        {
            return projects
                .OrderByDescending(p => ((double)p.Score) / p.Duration)
                ;
        }

        public bool ShouldDropProject()
        {
            return shouldDropProject;
        }

        public override string ToString()
        {
            return "PG";
        }
    }
}
