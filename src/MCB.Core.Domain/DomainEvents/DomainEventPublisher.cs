using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Observer;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Observer;

namespace MCB.Core.Domain.DomainEvents;

public class DomainEventPublisher
    : PublisherBase,
    IDomainEventPublisher
{
    // Constants
    public static readonly string subscriberCanotBeInitializedErrorMessage = "Subscriber cannot be initialized";

    // Fields
    private readonly IDependencyInjectionContainer _dependencyInjectionContainer;

    // Constructors
    public DomainEventPublisher(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        _dependencyInjectionContainer = dependencyInjectionContainer;
    }

    // Protected Methods
    protected override ISubscriber<TSubject> InstanciateSubscriber<TSubject>(Type subscriberType)
    {
        var subscriber = _dependencyInjectionContainer.Resolve(subscriberType);

        if (subscriber is null)
            throw new InvalidOperationException(subscriberCanotBeInitializedErrorMessage);

        return (ISubscriber<TSubject>)subscriber;
    }
}
