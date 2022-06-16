using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Domain.Abstractions.DomainEvents.Models;

namespace MCB.Core.Domain.DomainEvents.Interfaces;

public interface IDomainEventHandler
    : IDomainEventHandler<DomainEvent>
{
}
