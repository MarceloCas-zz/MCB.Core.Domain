using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Observer;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Observer;

namespace MCB.Core.Domain.DomainEvents;

public class DomainEventPublisher
    : PublisherBase,
    IDomainEventPublisher
{
    // Constants
    public const string SUBSCRIBER_CANOT_BE_INITIALIZED_ERROR_MESSAGE = "Subscriber cannot be initialized";

    // Fields
    private readonly IServiceProvider _serviceProvider;

    // Constructors
    public DomainEventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    // Protected Methods
    protected override ISubscriber<TSubject> InstanciateSubscriber<TSubject>(Type subscriberType)
    {
        var subscriber = _serviceProvider.GetService(subscriberType);

        if (subscriber is null)
            throw new InvalidOperationException(SUBSCRIBER_CANOT_BE_INITIALIZED_ERROR_MESSAGE);

        return (ISubscriber<TSubject>)subscriber;
    }
}
