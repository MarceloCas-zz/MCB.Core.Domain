using FluentAssertions;
using MCB.Core.Domain.Abstractions.DomainNotifications.Enums;
using MCB.Core.Domain.Abstractions.DomainNotifications.Models;
using MCB.Core.Domain.DomainNotifications;
using MCB.Core.Domain.DomainNotifications.Interfaces;
using MCB.Core.Domain.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainNotificationsTests;

[Collection(nameof(DefaultFixture))]
public class DomainNotificationPublisherTest
{
    // Fields
    private readonly DefaultFixture _defaultFixture;

    // Constructors
    public DomainNotificationPublisherTest(DefaultFixture defaultFixture)
    {
        _defaultFixture = defaultFixture;
    }

    [Fact]
    public async Task DomainNotificationPublisher_Should_Publish()
    {
        // Arrange
        var dependencyInjectionContainer = _defaultFixture.CreateNewDependencyInjectionContainer();
        dependencyInjectionContainer.CreateNewScope();

        var domainNotificationPublisher = dependencyInjectionContainer.Resolve<IDomainNotificationPublisher>();
        var domainNotificationHandler = dependencyInjectionContainer.Resolve<IDomainNotificationHandler>();
        var domainNotification = new DomainNotification(
            domainNotificationType: DomainNotificationType.Information,
            domainEntityTypeFullName: default,
            code: default,
            description: default
        );

        // Act
        await domainNotificationPublisher.PublishAsync(domainNotification, cancellationToken: default);

        // Assert
        domainNotificationHandler.ReceivedDomainNotificationsCollection.Should().HaveCount(1);
        domainNotificationHandler.HasDomainNotifications().Should().BeTrue();
        var receivedDomainNotificationsCollection = domainNotificationHandler.ReceivedDomainNotificationsCollection.ToArray();
        receivedDomainNotificationsCollection[0].Should().BeSameAs(domainNotification);
    }

    [Fact]
    public async Task DomainNotificationPublisher_Should_Throw_Exception_If_No_Subscribe_Registered_In_IoC()
    {
        // Arrange
        var dependencyInjectionContainer = _defaultFixture.CreateNewDependencyInjectionContainer();
        dependencyInjectionContainer.CreateNewScope();

        var domainEventPublisher = dependencyInjectionContainer.Resolve<IDomainNotificationPublisher>();
        domainEventPublisher.Subscribe<UnregisteredDomainNotificationHandler, DomainNotification>();
        var domainNotification = new DomainNotification(
            domainNotificationType: DomainNotificationType.Information,
            domainEntityTypeFullName: default,
            code: default,
            description: default
        );

        var exceptionMessage = string.Empty;

        // Act
        try
        {
            await domainEventPublisher.PublishAsync(domainNotification, cancellationToken: default);
        }
        catch (InvalidOperationException ex)
        {
            exceptionMessage = ex.Message;
        }

        // Assert
        exceptionMessage.Should().Be(DomainNotificationPublisher.subscriberCanotBeInitializedErrorMessage);
    }
}

public class UnregisteredDomainNotificationHandler
    : IDomainNotificationHandler
{
    public IEnumerable<DomainNotification> ReceivedDomainNotificationsCollection => throw new NotImplementedException();

    public Task HandlerAsync(DomainNotification subject, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public bool HasDomainNotifications()
    {
        throw new NotImplementedException();
    }
}
