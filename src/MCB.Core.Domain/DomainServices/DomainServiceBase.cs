using MCB.Core.Domain.Abstractions.DomainServices;
using MCB.Core.Domain.Entities.Abstractions;

namespace MCB.Core.Domain.DomainServices
{
    public abstract class DomainServiceBase<TAggregationRoot>
        : IDomainService<TAggregationRoot>
        where TAggregationRoot : IAggregationRoot
    {
    }
}
