using MCB.Core.Domain.Abstractions.DomainEvents.Models;
using MCB.Core.Domain.DomainEvents.Interfaces;

namespace MCB.Core.Domain.DomainEvents
{
    public class DomainEventHandler
        : IDomainEventHandler
    {
        // Fields
        private readonly List<DomainEvent> _receivedDomainEventsCollection;

        // Properties
        public IEnumerable<DomainEvent> ReceivedDomainEventsCollection => _receivedDomainEventsCollection.AsReadOnly();

        // Constructors
        public DomainEventHandler()
        {
            _receivedDomainEventsCollection = new List<DomainEvent>();
        }

        // Public Methods
        public bool HasDomainEvents() => _receivedDomainEventsCollection.Count > 0;
        public virtual Task HandlerAsync(DomainEvent subject, CancellationToken cancellationToken)
        {
            _receivedDomainEventsCollection.Add(subject);
            return Task.CompletedTask;
        }
    }
}
