using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public class ProjectStrategyO : IProjectStrategy
    {
        public double ScoreFactor { get; init; }
        public double DurationFactor { get; init; }
        public double HighestSkillLevelFactor { get; init; }

        public IQueryable<Project> GetProjectOrder(IQueryable<Project> projects)
        {
            return projects
                .OrderBy(p => p.BestBefore - p.Score * ScoreFactor - p.Duration * DurationFactor - p.TotalSkillLevel * HighestSkillLevelFactor)
                ;
        }

        public bool ShouldDropProject()
        {
            return true;
        }

        public ProjectStrategyO(double scoreFactor, double durationFactor = 0.0, double highestSkillLevelFactor = 0.0)
        {
            this.ScoreFactor = scoreFactor;
            this.DurationFactor = durationFactor;
            this.HighestSkillLevelFactor = highestSkillLevelFactor;
        }

        public override string ToString()
        {
            return $"PO({ScoreFactor}, {DurationFactor}, {HighestSkillLevelFactor})";
        }
    }
}
