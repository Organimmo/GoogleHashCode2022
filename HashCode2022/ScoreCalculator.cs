using HashCode2022.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace HashCode2022
{
    public class ScoreCalculator
    {
        public long Calculate(List<Project> assignedProjects)
        {
            long score = 0;
            foreach (var project in assignedProjects)
            {
                int endDate = (project.StartDate ?? throw new InvalidOperationException()) + project.Duration;
                int overdue = endDate - project.BestBefore;
                int penalty = overdue > 0 ? overdue : 0;
                score += Math.Max(0, project.Score - penalty);
            }
            return score;
        }
    }
}