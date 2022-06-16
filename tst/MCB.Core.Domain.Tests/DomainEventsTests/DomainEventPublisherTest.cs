using FluentAssertions;
using MCB.Core.Domain.Abstractions.DomainEvents;
using MCB.Core.Domain.Abstractions.DomainEvents.Models;
using MCB.Core.Domain.DomainEvents;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Domain.Tests.Fixtures;
using MCB.Core.Infra.CrossCutting.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainEventsTests;

[Collection(nameof(DefaultFixture))]
public class DomainEventPublisherTest
{
    // Fields
    private readonly DefaultFixture _fixture;

    // Constructors
    public DomainEventPublisherTest(DefaultFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task DomainEventPublisher_Should_Publish()
    {
        // Arrange
        var scopedServiceProvider = _fixture.ServiceProvider.CreateScope().ServiceProvider;
        var domainEventPublisher = scopedServiceProvider.GetService<IDomainEventPublisher>();
        var domainEventHandler = scopedServiceProvider.GetService<IDomainEventHandler>();
        var domainEvent = new DomainEvent
        {
            EventData = new DummyDomainEvent().SerializeToJson()
        };

        // Act
        await domainEventPublisher.PublishAsync(domainEvent, cancellationToken: default);

        // Assert
        domainEventHandler.ReceivedDomainEventsCollection.Should().HaveCount(1);
        domainEventHandler.HasDomainEvents().Should().BeTrue();
        var receivedDomainEventsCollection = domainEventHandler.ReceivedDomainEventsCollection.ToArray();
        receivedDomainEventsCollection[0].Should().BeSameAs(domainEvent);
    }

    [Fact]
    public async Task DomainEventPublisher_Should_Throw_Exception_If_No_Subscribe_Registered_In_IoC()
    {
        // Arrange
        var scopedServiceProvider = _fixture.ServiceProvider.CreateScope().ServiceProvider;
        var domainEventPublisher = scopedServiceProvider.GetService<IDomainEventPublisher>();
        domainEventPublisher.Subscribe<UnregisteredDomainEventHandler, DummyDomainEvent>();

        var domainEvent = new DummyDomainEvent();
        var exceptionMessage = string.Empty;

        // Act
        try
        {
            await domainEventPublisher.PublishAsync(domainEvent, cancellationToken: default);
        }
        catch (InvalidOperationException ex)
        {
            exceptionMessage = ex.Message;
        }

        // Assert
        exceptionMessage.Should().Be(DomainEventPublisher.subscriberCanotBeInitializedErrorMessage);
    }
}

public record DummyDomainEvent
    : DomainEvent
{
    public Guid Id { get; set; }

    public DummyDomainEvent()
    {
        Id = Guid.NewGuid();
    }
}
public class UnregisteredDomainEventHandler
    : IDomainEventHandler<DummyDomainEvent>
{
    public IEnumerable<DummyDomainEvent> ReceivedDomainEventsCollection => throw new NotImplementedException();

    public Task HandlerAsync(DummyDomainEvent subject, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public bool HasDomainEvents()
    {
        throw new NotImplementedException();
    }
}
