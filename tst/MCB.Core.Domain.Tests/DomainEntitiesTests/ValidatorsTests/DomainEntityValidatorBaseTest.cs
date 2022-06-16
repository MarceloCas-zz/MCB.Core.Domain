using FluentAssertions;
using MCB.Core.Domain.Abstractions.DomainEntities.Specifications;
using MCB.Core.Domain.DomainEntities.Specifications;
using MCB.Core.Domain.DomainEntities.Validators;
using MCB.Core.Domain.Entities;
using MCB.Core.Infra.CrossCutting.DateTime;
using System;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainEntitiesTests.ValidatorsTests;

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
            createdAt: DateTimeProvider.GetDate(),
            sourcePlatform: "unitTest",
            registryVersion: DateTimeProvider.GetDate()
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
            createdAt: DateTimeProvider.GetDate(),
            updatedBy: "marcelo.castelo@outlook.com",
            updatedAt: DateTimeProvider.GetDate(),
            sourcePlatform: "unitTest",
            registryVersion: DateTimeProvider.GetDate()
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
        SetExistingInfoInternal<Customer>(
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

    protected override DomainEntityBase CreateInstanceForCloneInternal()
    {
        return new Customer();
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
