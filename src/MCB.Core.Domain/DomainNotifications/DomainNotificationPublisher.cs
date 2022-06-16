﻿using MCB.Core.Domain.DomainNotifications.Interfaces;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Observer;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Observer;

namespace MCB.Core.Domain.DomainNotifications;

public class DomainNotificationPublisher
    : PublisherBase,
    IDomainNotificationPublisher
{
    // Constants
    public const string SUBSCRIBER_CANOT_BE_INITIALIZED_ERROR_MESSAGE = "Subscriber cannot be initialized";

    // Fields
    private readonly IServiceProvider _serviceProvider;

    // Constructors
    public DomainNotificationPublisher(IServiceProvider serviceProvider)
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
