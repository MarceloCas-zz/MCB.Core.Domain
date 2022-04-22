using FluentAssertions;
using MCB.Core.Domain.Abstractions.DomainNotifications.Enums;
using MCB.Core.Domain.Abstractions.DomainNotifications.Models;
using MCB.Core.Domain.DomainNotifications;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Abstractions.Observer;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Observer;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainNotificationsTests
{
    public class DomainNotificationHandlerBaseTest
    {
        [Fact]
        public async Task DomainNotificationHandler_Should_Handler_Domain_Events()
        {
            // Arrange
            var publisher = new Publisher();
            publisher.Subscribe<DomainNotificationHandler, DomainNotification>();
            var domainNotificationHandler = new DomainNotificationHandler();

            var domainNotificationToSendCollection = new List<DomainNotification>();
            for (int i = 0; i < 10; i++)
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
                domainNotificationToSendCollection.Add(
                    new DomainNotification(
                        DomainNotificationType.Information,
                        typeof(DomainNotification).FullName,
                        code: Guid.NewGuid().ToString(),
                        description: null
                    )
                );
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
#pragma warning restore CS8604 // Possible null reference argument.

                // Act
            foreach (var domainNotification in domainNotificationToSendCollection)
                await publisher.PublishAsync(
                    domainNotification,
                    cancellationToken: default
                );

            // Assert
            domainNotificationHandler.RaisedDomainNotificationsCollection.Should().BeEmpty();
            domainNotificationHandler.HasDomainNotifications().Should().BeFalse();
            domainNotificationToSendCollection.Should().HaveCount(DomainNotificationHandler.ReceivedDomainNotificationCollection.Count);
            for (int i = 0; i < domainNotificationToSendCollection.Count; i++)
            {
                var domainNotificationToSend = domainNotificationToSendCollection[i];
                var receivedDomainNotification = DomainNotificationHandler.ReceivedDomainNotificationCollection[i];

                domainNotificationToSend.Should().BeSameAs(receivedDomainNotification);
            }
        }
    }

    public class Publisher
        : PublisherBase
    {
        protected override ISubscriber<TSubject> InstanciateSubscriber<TSubject>(Type subscriberType)
        {
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return (ISubscriber<TSubject>)Activator.CreateInstance(subscriberType);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
        }
    }
    public class DomainNotificationHandler
        : DomainNotificationEventHandlerBase
    {
        public static List<DomainNotification> ReceivedDomainNotificationCollection { get; }

        static DomainNotificationHandler()
        {
            ReceivedDomainNotificationCollection = new List<DomainNotification>();
        }

        public override Task HandlerAsync(DomainNotification subject, CancellationToken cancellationToken)
        {
            ReceivedDomainNotificationCollection.Add(subject);
            return base.HandlerAsync(subject, cancellationToken);
        }
    }
}
