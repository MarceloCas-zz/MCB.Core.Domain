using MCB.Core.Domain.Abstractions.DomainEvents;

namespace MCB.Core.Domain.DomainEvents
{
    public class DomainEvent
        : IDomainEvent
    {
        // Properties
        public Guid EventId { get; set; }
        public Guid CorrelationId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
        public string? EventType { get; set; }
        public string EventSchema { get; set; }
        public Guid TenantId { get; set; }
        public string ExecutionUser { get; set; }
        public string SourcePlatform { get; set; }

        // Constructors
        public DomainEvent()
        {
            EventId = Guid.NewGuid();
            TimeStamp = DateTimeOffset.UtcNow;
            EventType = GetType().FullName;
            EventSchema = string.Empty;
            ExecutionUser = string.Empty;
            SourcePlatform = string.Empty;
        }
    }
}
