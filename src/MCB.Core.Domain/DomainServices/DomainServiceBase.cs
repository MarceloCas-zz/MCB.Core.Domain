using MCB.Core.Domain.Abstractions.DomainEvents.Models;
using MCB.Core.Domain.Abstractions.DomainServices;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;

namespace MCB.Core.Domain.DomainServices;
public abstract class DomainServiceBase<TAggregationRoot>
    : IDomainService<TAggregationRoot>
    where TAggregationRoot : IAggregationRoot
{
    // Fields
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly IJsonSerializer _jsonSerializer;

    // Constructors
    protected DomainServiceBase(
        IDomainEventPublisher domainEventPublisher,
        IJsonSerializer jsonSerializer
    )
    {
        _domainEventPublisher = domainEventPublisher;
        _jsonSerializer = jsonSerializer;
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
            EventDataSchema = _jsonSerializer.GenerateJsonSchema(eventDataType),
            EventData = _jsonSerializer.SerializeToJson(eventData),
            ExecutionUser = executionUser,
            SourcePlatform = sourcePlatform
        };

        await _domainEventPublisher.PublishAsync(
            domainEvent,
            cancellationToken
        ).ConfigureAwait(false);
    }
}
