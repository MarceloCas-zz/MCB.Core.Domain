using MCB.Core.Domain.Abstractions.Repositories;
using MCB.Core.Domain.Entities.Abstractions;

namespace MCB.Core.Domain.Repositories
{
    public abstract class RepositoryBase<TAggregationRoot>
        : IRepository<TAggregationRoot>
        where TAggregationRoot : IAggregationRoot
    {
    }
}
