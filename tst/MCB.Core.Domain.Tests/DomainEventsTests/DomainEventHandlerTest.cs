using FluentAssertions;
using MCB.Core.Domain.Abstractions.DomainEvents.Models;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Domain.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainEventsTests;

[Collection(nameof(DefaultFixture))]
public class DomainEventHandlerTest
{
    // Fields
    private readonly DefaultFixture _defaultFixture;

    // Constructors
    public DomainEventHandlerTest(DefaultFixture defaultFixture)
    {
        _defaultFixture = defaultFixture;
    }

    [Fact]
    public async Task DomainEventHandler_Should_Handler_Domain_Events()
    {
        // Arrange
        var dependencyInjectionContainer = _defaultFixture.CreateNewDependencyInjectionContainer();
        dependencyInjectionContainer.CreateNewScope();

        var domainEventPublisher = dependencyInjectionContainer.Resolve<IDomainEventPublisher>();
        var domainEventHandler = dependencyInjectionContainer.Resolve<IDomainEventHandler>();

        var domainEventToSendCollection = new List<DomainEvent>();
        for (int i = 0; i < 10; i++)
            domainEventToSendCollection.Add(new DomainEvent { CorrelationId = Guid.NewGuid() });

        // Act
        foreach (var domainEvent in domainEventToSendCollection)
            await domainEventPublisher.PublishAsync(
                domainEvent,
                cancellationToken: default
            );

        // Assert
        domainEventHandler.ReceivedDomainEventsCollection.Should().HaveCount(domainEventToSendCollection.Count);
        domainEventHandler.HasDomainEvents().Should().BeTrue();
        domainEventToSendCollection.Should().HaveCount(domainEventHandler.ReceivedDomainEventsCollection.Count());
        for (int i = 0; i < domainEventToSendCollection.Count; i++)
        {
            var domainEventToSend = domainEventToSendCollection[i];
            var receivedDomainEvent = domainEventHandler.ReceivedDomainEventsCollection.ToArray()[i];

            receivedDomainEvent.EventId.Should().NotBe(Guid.Empty);
            receivedDomainEvent.TimeStamp.Should().BeAfter(default);
            

            domainEventToSend.Should().BeSameAs(receivedDomainEvent);
        }
    }
}
