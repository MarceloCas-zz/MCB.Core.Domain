using FluentAssertions;
using MCB.Core.Domain.Abstractions.DomainEntities.Specifications;
using MCB.Core.Domain.DomainEntities.Specifications;
using MCB.Core.Domain.DomainEntities.Validators;
using MCB.Core.Domain.Entities;
using System;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainEntitiesTests.ValidatorsTests
{
    public class DomainEntityValidatorBaseTest
    {
        [Fact]
        public void DomainEntitySpecificationsBase_Should_Validate_Creation_Info()
        {
            // Arrange
            var customer = new Customer().SetExistingInfoExposed(
                id: Guid.NewGuid(),
                tenantId: Guid.NewGuid(),
                createdBy: "marcelo.castelo@outlook.com",
                createdAt: DateTimeOffset.UtcNow,
                sourcePlatform: "unitTest",
                registryVersion: DateTimeOffset.UtcNow
            );
            var customerValidatorisValidToCreate = new CustomerValidatorisValidToCreate(new CustomerSpecifications());

            // Act
            var validationResult = customerValidatorisValidToCreate.Validate(customer);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
        [Fact]
        public void DomainEntitySpecificationsBase_Should_Validate_Update_Info()
        {
            // Arrange
            var customer = new Customer().SetExistingInfoExposed(
                id: Guid.NewGuid(),
                tenantId: Guid.NewGuid(),
                createdBy: "marcelo.castelo@outlook.com",
                createdAt: DateTimeOffset.UtcNow,
                updatedBy: "marcelo.castelo@outlook.com",
                updatedAt: DateTimeOffset.UtcNow,
                sourcePlatform: "unitTest",
                registryVersion: DateTimeOffset.UtcNow
            );
            var customerValidatorisValidToCreate = new CustomerValidatorisValidToUpdate(new CustomerSpecifications());

            // Act
            var validationResult = customerValidatorisValidToCreate.Validate(customer);

            // Assert
            validationResult.IsValid.Should().BeTrue();
        }
    }

    public class Customer
        : DomainEntityBase
    {
        public Customer SetExistingInfoExposed(
            Guid id = default,
            Guid tenantId = default,
            string createdBy = default,
            DateTimeOffset createdAt = default,
            string updatedBy = default,
            DateTimeOffset? updatedAt = default,
            string sourcePlatform = default,
            DateTimeOffset registryVersion = default
        )
        {
            SetExistingInfo(
                id,
                tenantId,
                createdBy,
                createdAt,
                updatedBy,
                updatedAt,
                sourcePlatform,
                registryVersion
            );

            return this;
        }
    }
    public class CustomerSpecifications
        : DomainEntitySpecificationsBase<Customer>
    {

    }
    public class CustomerValidatorisValidToCreate
        : DomainEntityValidatorBase<Customer>
    {
        public CustomerValidatorisValidToCreate(
            IDomainEntitySpecifications<Customer> domainEntitySpecifications
        ) : base(domainEntitySpecifications)
        {

        }

        protected override void ConfigureFluentValidationConcreteValidator(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
        {
            AddSpecificationsForCreation();
        }
    }
    public class CustomerValidatorisValidToUpdate
        : DomainEntityValidatorBase<Customer>
    {
        public CustomerValidatorisValidToUpdate(
            IDomainEntitySpecifications<Customer> domainEntitySpecifications
        ) : base(domainEntitySpecifications)
        {

        }

        protected override void ConfigureFluentValidationConcreteValidator(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
        {
            AddSpecificationsForUpdate();
        }
    }
}
