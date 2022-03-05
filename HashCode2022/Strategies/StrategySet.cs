using System;

namespace HashCode2022.Strategies
{
    class StrategySet
    {
        public IContributorStrategy ContributorStrategy { get; init; }
        public IProjectStrategy ProjectStrategy { get; init; }

        public StrategySet(IContributorStrategy contributorStrategy, IProjectStrategy projectStrategy)
        {
            ContributorStrategy = contributorStrategy ?? throw new ArgumentNullException(nameof(contributorStrategy));
            ProjectStrategy = projectStrategy ?? throw new ArgumentNullException(nameof(projectStrategy));
        }

        public override string ToString()
        {
            return $"{ContributorStrategy}-{ProjectStrategy}";
        }
    }
}
