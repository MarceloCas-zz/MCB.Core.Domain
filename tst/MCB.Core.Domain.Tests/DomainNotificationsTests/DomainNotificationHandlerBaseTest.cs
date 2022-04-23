using FluentAssertions;
using MCB.Core.Domain.Abstractions.DomainNotifications.Enums;
using MCB.Core.Domain.Abstractions.DomainNotifications.Models;
using MCB.Core.Domain.DomainNotifications.Interfaces;
using MCB.Core.Domain.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainNotificationsTests
{
    [Collection(nameof(DefaultFixture))]
    public class DomainNotificationHandlerBaseTest
    {
        // Fields
        private readonly DefaultFixture _defaultFixture;

        // Constructors
        public DomainNotificationHandlerBaseTest(DefaultFixture defaultFixture)
        {
            _defaultFixture = defaultFixture;
        }

        [Fact]
        public async Task DomainNotificationHandler_Should_Handler_Domain_Events()
        {
            // Arrange
            var scopedServiceProvider = _defaultFixture.ServiceProvider.CreateScope().ServiceProvider;
            var domainNotificationPublisher = scopedServiceProvider.GetService<IDomainNotificationPublisher>();
            var domainNotificationHandler = scopedServiceProvider.GetService<IDomainNotificationHandler>();

            var domainNotificationToSendCollection = new List<DomainNotification>();
            for (int i = 0; i < 10; i++)
                domainNotificationToSendCollection.Add(
                    new DomainNotification(
                        DomainNotificationType.Information,
                        typeof(DomainNotification).FullName,
                        code: Guid.NewGuid().ToString(),
                        description: null
                    )
                );

            // Act
            foreach (var domainNotification in domainNotificationToSendCollection)
                await domainNotificationPublisher.PublishAsync(
                    domainNotification,
                    cancellationToken: default
                );

            // Assert
            domainNotificationHandler.ReceivedDomainNotificationsCollection.Should().HaveCount(domainNotificationToSendCollection.Count);
            domainNotificationHandler.HasDomainNotifications().Should().BeTrue();
            domainNotificationToSendCollection.Should().HaveCount(domainNotificationHandler.ReceivedDomainNotificationsCollection.Count());
            for (int i = 0; i < domainNotificationToSendCollection.Count; i++)
            {
                var domainNotificationToSend = domainNotificationToSendCollection[i];
                var receivedDomainNotification = domainNotificationHandler.ReceivedDomainNotificationsCollection.ToArray()[i];

                domainNotificationToSend.Should().BeSameAs(receivedDomainNotification);
            }
        }
    }
}
