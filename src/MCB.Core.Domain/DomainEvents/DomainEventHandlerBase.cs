using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Domain.Abstractions.DomainEvents.Models;

namespace MCB.Core.Domain.DomainEvents
{
    public abstract class DomainEventHandlerBase
        : IDomainEventHandler<DomainEvent>
    {
        // Fields
        private readonly List<DomainEvent> _raisedDomainEventsCollection;

        // Properties
        public IEnumerable<DomainEvent> RaisedDomainEventsCollection => _raisedDomainEventsCollection.AsReadOnly();

        // Constructors
        protected DomainEventHandlerBase()
        {
            _raisedDomainEventsCollection = new List<DomainEvent>();
        }

        // Public Methods
        public bool HasDomainEvents() => _raisedDomainEventsCollection.Count > 0;
        public virtual Task HandlerAsync(DomainEvent subject, CancellationToken cancellationToken)
        {
            _raisedDomainEventsCollection.Add(subject);
            return Task.CompletedTask;
        }
    }
}
