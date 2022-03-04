using System.Collections.Generic;
using System.Linq;
using HashCode2022.Entities;

namespace HashCode2022.Strategies
{
    public interface IProjectStrategy
    {
        IQueryable<Project> GetProjectOrder(IQueryable<Project> contributors);
        bool ShouldDropProject();
    }
}
