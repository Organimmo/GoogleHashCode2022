using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ProjectStrategyO : IProjectStrategy
    {
        private readonly double scoreFactor;
        private readonly double durationFactor;
        private readonly double highestSkillLevelFactor;

        public IQueryable<Project> GetProjectOrder(IQueryable<Project> projects)
        {
            return projects
                .OrderBy(p => p.BestBefore - p.Score * scoreFactor - p.Duration * durationFactor - p.TotalSkillLevel * highestSkillLevelFactor)
                ;
        }

        public bool ShouldDropProject()
        {
            return true;
        }

        public ProjectStrategyO(double scoreFactor, double durationFactor = 0.0, double highestSkillLevelFactor = 0.0)
        {
            this.scoreFactor = scoreFactor;
            this.durationFactor = durationFactor;
            this.highestSkillLevelFactor = highestSkillLevelFactor;
        }

        public override string ToString()
        {
            return $"PO({scoreFactor}, {durationFactor}, {highestSkillLevelFactor})";
        }
    }
}
