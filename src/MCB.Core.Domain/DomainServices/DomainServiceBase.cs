using MCB.Core.Domain.Abstractions.DomainEvents.Models;
using MCB.Core.Domain.Abstractions.DomainServices;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Infra.CrossCutting.Serialization;

namespace MCB.Core.Domain.DomainServices;
public abstract class DomainServiceBase<TAggregationRoot>
    : IDomainService<TAggregationRoot>
    where TAggregationRoot : IAggregationRoot
{
    // Fields
    private readonly IDomainEventPublisher _domainEventPublisher;

    // Constructors
    protected DomainServiceBase(
        IDomainEventPublisher domainEventPublisher
    )
    {
        _domainEventPublisher = domainEventPublisher;
    }

    // Private Methods
    private static void ValidateEventData<TEventData>(TEventData eventData)
    {
        if (eventData is null)
            throw new ArgumentNullException(nameof(eventData));
    }

    // Protected Methods
    protected async Task RaiseDomainEventAsync<TEventData>(
        Guid tenantId,
        Guid correlationId,
        string executionUser,
        string sourcePlatform,
        TEventData eventData,
        CancellationToken cancellationToken
    )
    {
        ValidateEventData(eventData);

        var eventDataType = eventData.GetType();
        var domainEvent = new DomainEvent
        {
            TenantId = tenantId,
            CorrelationId = correlationId,
            EventDataType = eventDataType.FullName,
            EventDataSchema = eventDataType.GenerateJsonSchema(),
            EventData = eventData.SerializeToJson(),
            ExecutionUser = executionUser,
            SourcePlatform = sourcePlatform
        };

        await _domainEventPublisher.PublishAsync(
            domainEvent,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
