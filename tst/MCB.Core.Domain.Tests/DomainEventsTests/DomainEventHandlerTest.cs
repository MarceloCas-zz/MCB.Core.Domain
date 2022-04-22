using FluentAssertions;
using MCB.Core.Domain.DomainEvents;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Observer;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Observer;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainEventsTests
{
    public class DomainEventHandlerTest
    {
        [Fact]
        public async Task DomainEventHandler_Should_Handler_Domain_Events()
        {
            // Arrange
            var publisher = new Publisher();
            publisher.Subscribe<DomainEventHandler, DomainEvent>();
            var domainEventHandler = new DomainEventHandler();

            var domainEventToSendCollection = new List<DomainEvent>();
            for (int i = 0; i < 10; i++)
                domainEventToSendCollection.Add(new DomainEvent { CorrelationId = Guid.NewGuid() });

            // Act
            foreach (var domainEvent in domainEventToSendCollection)
                await publisher.PublishAsync(
                    domainEvent,
                    cancellationToken: default
                );

            // Assert
            domainEventHandler.RaisedDomainEventsCollection.Should().BeEmpty();
            domainEventToSendCollection.Should().HaveCount(DomainEventHandler.ReceivedDomainEventCollection.Count);
            for (int i = 0; i < domainEventToSendCollection.Count; i++)
            {
                var domainEventToSend = domainEventToSendCollection[i];
                var receivedDomainEvent = DomainEventHandler.ReceivedDomainEventCollection[i];

                domainEventToSend.Should().BeSameAs(receivedDomainEvent);
            }
        }
    }

    public class Publisher
        : PublisherBase
    {
        protected override ISubscriber<TSubject> InstanciateSubscriber<TSubject>(Type subscriberType)
        {
            return (ISubscriber<TSubject>)Activator.CreateInstance(subscriberType);
        }
    }
    public class DomainEventHandler
        : DomainEventHandlerBase
    {
        public static List<DomainEvent> ReceivedDomainEventCollection { get; }

        static DomainEventHandler()
        {
            ReceivedDomainEventCollection = new List<DomainEvent>();
        }

        public override Task HandlerAsync(DomainEvent subject, CancellationToken cancellationToken)
        {
            ReceivedDomainEventCollection.Add(subject);
            return base.HandlerAsync(subject, cancellationToken);
        }
    }
}
