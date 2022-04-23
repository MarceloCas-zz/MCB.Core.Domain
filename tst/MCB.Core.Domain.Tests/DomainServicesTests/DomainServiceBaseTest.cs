using MCB.Core.Domain.Abstractions.DomainServices;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Domain.DomainServices;
using MCB.Core.Domain.Entities;
using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Domain.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using MCB.Core.Infra.CrossCutting.Serialization;
using FluentAssertions;

namespace MCB.Core.Domain.Tests.DomainServicesTests
{
    [Collection(nameof(DefaultFixture))]
    public class DomainServiceBaseTest
    {
        // Fields
        private readonly DefaultFixture _fixture;

        // Constructors
        public DomainServiceBaseTest(DefaultFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task DomainServiceBase_Should_Raise_DomainNotificationEvent_And_DomainEvents()
        {
            // Arrange
            var scopedServiceProvider = _fixture.ServiceProvider.CreateScope().ServiceProvider;
            var customerDomainService = scopedServiceProvider.GetService<ICustomerDomainService>();
            var domainEventHandler = scopedServiceProvider.GetService<IDomainEventHandler>();
            var tenantId = Guid.NewGuid();
            var correlationId = Guid.NewGuid();
            var executionUser = "marcelo.castelo@outlook.com";
            var sourcePlatform = "unitTest";
            var customer = new Customer().RegisterNewExposed(tenantId, executionUser, sourcePlatform);
            var newCustomerRegisteredEvent = new NewCustomerRegisteredEvent
            {
                Customer = customer
            };
            var newCustomerRegisteredEventJsonSchema = newCustomerRegisteredEvent.GenerateJsonSchema();


            // Act
            await customerDomainService.RaiseDomainEventAsyncExposed(
                tenantId,
                correlationId,
                executionUser,
                sourcePlatform,
                eventData: newCustomerRegisteredEvent,
                cancellationToken: default
            );

            // Assert
            domainEventHandler.ReceivedDomainEventsCollection.Should().HaveCount(1);
            domainEventHandler.HasDomainEvents().Should().BeTrue();

            var receivedDomainEventsCollection = domainEventHandler.ReceivedDomainEventsCollection.ToArray();
            receivedDomainEventsCollection[0].TenantId.Should().Be(tenantId);
            receivedDomainEventsCollection[0].EventId.Should().NotBe(correlationId);
            receivedDomainEventsCollection[0].CorrelationId.Should().Be(correlationId);
            receivedDomainEventsCollection[0].EventDataType.Should().Be(newCustomerRegisteredEvent.GetType().FullName);
            receivedDomainEventsCollection[0].EventDataSchema.Should().Be(newCustomerRegisteredEventJsonSchema);
            receivedDomainEventsCollection[0].EventData.Should().Be(newCustomerRegisteredEvent.SerializeToJson());
            receivedDomainEventsCollection[0].ExecutionUser.Should().Be(executionUser);
            receivedDomainEventsCollection[0].SourcePlatform.Should().Be(sourcePlatform);
        }
    }

    public class Customer
        : DomainEntityBase,
        IAggregationRoot
    {
        public const string REGISTER_NEW_MESSAGE_CODE = "REGISTER_NEW_MESSAGE_CODE";
        public const string REGISTER_NEW_MESSAGE_DESCRIPTION = "REGISTER_NEW_MESSAGE_DESCRIPTION";

        public Customer RegisterNewExposed(
            Guid tenantId,
            string executionUser,
            string sourcePlatform
        )
        {
            return (Customer)base.RegisterNew(
                tenantId,
                executionUser,
                sourcePlatform
            );
        }
    }
    public class NewCustomerRegisteredEvent
    {
        public Customer Customer { get; set; }
    }
    public interface ICustomerDomainService
        : IDomainService<Customer>
    {
        Task RaiseDomainEventAsyncExposed<TEventData>(Guid tenantId, Guid correlationId, string executionUser, string sourcePlatform, TEventData eventData, CancellationToken cancellationToken);
    }
    public class CustomerDomainService
        : DomainServiceBase<Customer>,
        ICustomerDomainService
    {
        public CustomerDomainService(
            IDomainEventPublisher domainEventPublisher
        ) : base(domainEventPublisher)
        {

        }

        public async Task RaiseDomainEventAsyncExposed<TEventData>(
            Guid tenantId,
            Guid correlationId,
            string executionUser,
            string sourcePlatform,
            TEventData eventData,
            CancellationToken cancellationToken
        )
        {
            await RaiseDomainEventAsync(tenantId, correlationId, executionUser, sourcePlatform, eventData, cancellationToken);
        }
    }
}
