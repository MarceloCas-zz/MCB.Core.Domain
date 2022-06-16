using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MCB.Core.Domain.DomainEntities.Specifications;
using MCB.Core.Domain.Entities;
using MCB.Core.Domain.Tests.Fixtures;
using MCB.Core.Infra.CrossCutting.DateTime;
using System;
using System.Linq;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainEntitiesTests.SpecificationsTests;

[Collection(nameof(DefaultFixture))]
public class DomainEntitySpecificationsBaseTest
{
    private readonly DefaultFixture _defaultFixture;

    public DomainEntitySpecificationsBaseTest(DefaultFixture defaultFixture)
    {
        _defaultFixture = defaultFixture;
    }

    [Fact]
    public void AddIdIsRequiredSpecification_Should_Pass()
    {
        // Arrange
        var customer = new Customer();
        customer.SetExistingInfoExposed(id: Guid.NewGuid());
        var customerValidator = new CustomerValidator();

        // Act
        var validationResult = customerValidator.Validate(customer);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.ID_IS_REQUIRED_ERROR_CODE
        ).Should().BeNull();
    }
    [Fact]
    public void AddIdIsRequiredSpecification_Should_Not_Pass()
    {
        // Arrange
        var customer = new Customer();
        customer.SetExistingInfoExposed(id: Guid.Empty);
        var customerValidator = new CustomerValidator();

        // Act
        var validationResult = customerValidator.Validate(customer);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.ID_IS_REQUIRED_ERROR_CODE
        ).Should().NotBeNull();
    }

    [Fact]
    public void AddTenantIdIsRequiredSpecification_Should_Pass()
    {
        // Arrange
        var customer = new Customer();
        customer.SetExistingInfoExposed(tenantId: Guid.NewGuid());
        var customerValidator = new CustomerValidator();

        // Act
        var validationResult = customerValidator.Validate(customer);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.TENANT_ID_IS_REQUIRED_ERROR_CODE
        ).Should().BeNull();
    }
    [Fact]
    public void AddTenantIdIsRequiredSpecification_Should_Not_Pass()
    {
        // Arrange
        var customer = new Customer();
        customer.SetExistingInfoExposed(tenantId: Guid.Empty);
        var customerValidator = new CustomerValidator();

        // Act
        var validationResult = customerValidator.Validate(customer);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.TENANT_ID_IS_REQUIRED_ERROR_CODE
        ).Should().NotBeNull();
    }

    [Fact]
    public void AddCreationInfoIsRequiredSpecification_Should_Pass()
    {
        // Arrange
        var customer = new Customer();
        customer.SetExistingInfoExposed(
            createdAt: DateTimeProvider.GetDate(),
            createdBy: "marcelo.castelo@outlook.com"
        );
        var customerValidator = new CustomerValidator();

        // Act
        var validationResult = customerValidator.Validate(customer);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.CREATION_INFO_IS_REQUIRED_ERROR_CODE
        ).Should().BeNull();
    }
    [Fact]
    public void AddCreationInfoIsRequiredSpecification_Should_Not_Pass()
    {
        // Arrange
        var customerA = new Customer();
        customerA.SetExistingInfoExposed(
            createdAt: DateTimeProvider.GetDate(),
            createdBy: null
        );
        var customerB = new Customer();
        customerB.SetExistingInfoExposed(
            createdAt: default,
            createdBy: "marcelo.castelo@outlook.com"
        );
        var customerValidator = new CustomerValidator();

        // Act
        var validationResultA = customerValidator.Validate(customerA);
        var validationResultB = customerValidator.Validate(customerB);

        // Assert
        validationResultA.Should().NotBeNull();
        validationResultA.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.CREATION_INFO_IS_REQUIRED_ERROR_CODE
        ).Should().NotBeNull();

        validationResultB.Should().NotBeNull();
        validationResultB.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.CREATION_INFO_IS_REQUIRED_ERROR_CODE
        ).Should().NotBeNull();
    }

    [Fact]
    public void AddCreationInfoIsValidSpecification_Should_Pass()
    {
        // Arrange
        var customer = new Customer();
        customer.SetExistingInfoExposed(
            createdAt: DateTimeProvider.GetDate(),
            createdBy: "marcelo.castelo@outlook.com"
        );
        var customerValidator = new CustomerValidator();

        // Act
        var validationResult = customerValidator.Validate(customer);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.CREATION_INFO_SHOULD_BE_VALID_ERROR_CODE
        ).Should().BeNull();
    }
    [Fact]
    public void AddCreationInfoIsValidSpecification_Should_Not_Pass()
    {
        // Arrange
        var customerA = new Customer();
        customerA.SetExistingInfoExposed(
            createdAt: DateTimeProvider.GetDate().AddDays(1),
            createdBy: "marcelo.castelo@outlook.com"
        );
        var customerB = new Customer();
        customerB.SetExistingInfoExposed(
            createdAt: DateTimeProvider.GetDate(),
            createdBy: new string('a', 251)
        );
        var customerValidator = new CustomerValidator();

        // Act
        var validationResultA = customerValidator.Validate(customerA);
        var validationResultB = customerValidator.Validate(customerB);

        // Assert
        validationResultA.Should().NotBeNull();
        validationResultA.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.CREATION_INFO_SHOULD_BE_VALID_ERROR_CODE
        ).Should().NotBeNull();

        validationResultB.Should().NotBeNull();
        validationResultB.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.CREATION_INFO_SHOULD_BE_VALID_ERROR_CODE
        ).Should().NotBeNull();
    }

    [Fact]
    public void AddUpdateInfoIsValidSpecification_Should_Pass()
    {
        // Arrange
        var customerValidator = new CustomerValidator();
        var referenceDate = DateTimeProvider.GetDate();

        /*
        domainEntity.AuditableInfo.UpdatedAt >= domainEntity.AuditableInfo.CreatedAt
        && domainEntity.AuditableInfo.UpdatedAt <= DateTimeProvider.GetDate()
        && domainEntity.AuditableInfo.UpdatedBy.Length <= 250
         */

        var customerCollection = new Customer[] {
            // domainEntity.AuditableInfo.UpdatedAt > domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt < DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length < 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(-2),
                updatedAt: referenceDate.AddDays(-1),
                updatedBy: "marcelo.castelo@outlook.com"
            ),
            // domainEntity.AuditableInfo.UpdatedAt > domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt < DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length == 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(-2),
                updatedAt: referenceDate.AddDays(-1),
                updatedBy: new string('a', 250)
            ),
            // domainEntity.AuditableInfo.UpdatedAt > domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt == DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length < 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(-2),
                updatedAt: referenceDate,
                updatedBy: "marcelo.castelo@outlook.com"
            ),
            // domainEntity.AuditableInfo.UpdatedAt > domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt == DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length == 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(-2),
                updatedAt: referenceDate,
                updatedBy: new string('a', 250)
            ),
            // domainEntity.AuditableInfo.UpdatedAt == domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt < DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length < 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(-1),
                updatedAt: referenceDate.AddDays(-1),
                updatedBy: "marcelo.castelo@outlook.com"
            ),
            // domainEntity.AuditableInfo.UpdatedAt == domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt < DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length == 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(-1),
                updatedAt: referenceDate.AddDays(-1),
                updatedBy: new string('a', 250)
            ),
            // domainEntity.AuditableInfo.UpdatedAt == domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt == DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length < 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate,
                updatedAt: referenceDate,
                updatedBy: "marcelo.castelo@outlook.com"
            ),
            // domainEntity.AuditableInfo.UpdatedAt == domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt == DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length == 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate,
                updatedAt: referenceDate,
                updatedBy: new string('a', 250)
            ),
        };
        var validationResultCollection = new ValidationResult[customerCollection.Length];

        // Act
        for (int i = 0; i < customerCollection.Length; i++)
            validationResultCollection[i] = customerValidator.Validate(customerCollection[i]);

        // Assert
        foreach (var validationResult in validationResultCollection)
        {
            validationResult.Should().NotBeNull();
            validationResult.Errors.FirstOrDefault(
                q => q.ErrorCode == DomainEntitySpecificationsBase.UPDATE_INFO_SHOULD_BE_VALID_ERROR_CODE
            ).Should().BeNull();
        }
    }
    [Fact]
    public void AddUpdateInfoIsValidSpecification_Should_Not_Pass()
    {
        // Arrange
        var customerValidator = new CustomerValidator();
        var referenceDate = DateTimeProvider.GetDate();

        /*
        domainEntity.AuditableInfo.UpdatedAt >= domainEntity.AuditableInfo.CreatedAt
        && domainEntity.AuditableInfo.UpdatedAt <= DateTimeProvider.GetDate()
        && domainEntity.AuditableInfo.UpdatedBy.Length <= 250
         */

        var customerCollection = new Customer[] {
            // domainEntity.AuditableInfo.UpdatedAt < domainEntity.AuditableInfo.CreatedAt
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate,
                updatedAt: referenceDate.AddDays(-1),
                updatedBy: "marcelo.castelo@outlook.com"
            ),
            // domainEntity.AuditableInfo.UpdatedAt > domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt > DateTimeProvider.GetDate()
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate,
                updatedAt: referenceDate.AddDays(1),
                updatedBy: "marcelo.castelo@outlook.com"
            ),
            // domainEntity.AuditableInfo.UpdatedAt == domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt > DateTimeProvider.GetDate()
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(1),
                updatedAt: referenceDate.AddDays(1),
                updatedBy: "marcelo.castelo@outlook.com"
            ),
            // domainEntity.AuditableInfo.UpdatedAt > domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt < DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length > 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(-2),
                updatedAt: referenceDate.AddDays(-1),
                updatedBy: new string('a', 251)
            ),
            // domainEntity.AuditableInfo.UpdatedAt > domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt == DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length > 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(-1),
                updatedAt: referenceDate,
                updatedBy: new string('a', 251)
            ),
            // domainEntity.AuditableInfo.UpdatedAt == domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt < DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length > 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate.AddDays(-1),
                updatedAt: referenceDate.AddDays(-1),
                updatedBy: new string('a', 251)
            ),
            // domainEntity.AuditableInfo.UpdatedAt == domainEntity.AuditableInfo.CreatedAt
            // && domainEntity.AuditableInfo.UpdatedAt == DateTimeProvider.GetDate()
            // && domainEntity.AuditableInfo.UpdatedBy.Length > 250
            new Customer().SetExistingInfoExposed(
                createdAt: referenceDate,
                updatedAt: referenceDate,
                updatedBy: new string('a', 251)
            ),
        };
        var validationResultCollection = new ValidationResult[customerCollection.Length];

        // Act
        for (int i = 0; i < customerCollection.Length; i++)
            validationResultCollection[i] = customerValidator.Validate(customerCollection[i]);

        // Assert
        foreach (var validationResult in validationResultCollection)
        {
            validationResult.Should().NotBeNull();
            validationResult.Errors.FirstOrDefault(
                q => q.ErrorCode == DomainEntitySpecificationsBase.UPDATE_INFO_SHOULD_BE_VALID_ERROR_CODE
            ).Should().NotBeNull();
        }
    }

    [Fact]
    public void AddRegistryVersionIsRequiredSpecification_Should_Pass()
    {
        // Arrange
        var customer = new Customer();
        customer.SetExistingInfoExposed(registryVersion: DateTimeProvider.GetDate());
        var customerValidator = new CustomerValidator();

        // Act
        var validationResult = customerValidator.Validate(customer);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.REGISTRY_VERSION_IS_REQUIRED_ERROR_CODE
        ).Should().BeNull();
    }
    [Fact]
    public void AddRegistryVersionIsRequiredSpecification_Should_Not_Pass()
    {
        // Arrange
        var customer = new Customer();
        customer.SetExistingInfoExposed(registryVersion: default);

        var customerValidator = new CustomerValidator();

        // Act
        var validationResult = customerValidator.Validate(customer);

        // Assert
        validationResult.Should().NotBeNull();
        validationResult.IsValid.Should().BeFalse();
        validationResult.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.REGISTRY_VERSION_IS_REQUIRED_ERROR_CODE
        ).Should().NotBeNull();
    }


    [Fact]
    public void AddRegistryVersionIsValidSpecification_Should_Pass()
    {
        // Arrange
        var customerValidator = new CustomerValidator();
        var customerA = new Customer();
        customerA.SetExistingInfoExposed(
            updatedAt: DateTimeProvider.GetDate().AddDays(-2),
            registryVersion: DateTimeProvider.GetDate().AddDays(-1)
        );
        var customerB = new Customer();
        customerB.SetExistingInfoExposed(
            updatedAt: null,
            registryVersion: DateTimeProvider.GetDate().AddDays(-1)
        );

        // Act
        var validationResultA = customerValidator.Validate(customerA);
        var validationResultB = customerValidator.Validate(customerB);

        // Assert
        validationResultB.Should().NotBeNull();
        validationResultB.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_CODE
        ).Should().BeNull();
    }
    [Fact]
    public void AddRegistryVersionIsValidSpecification_Should_Not_Pass()
    {
        // Arrange
        var customerA = new Customer();
        customerA.SetExistingInfoExposed(
            updatedAt: DateTimeProvider.GetDate().AddDays(-1),
            registryVersion: DateTimeProvider.GetDate().AddDays(-2)
        );

        var customerB = new Customer();
        customerB.SetExistingInfoExposed(
            registryVersion: DateTimeProvider.GetDate().AddDays(1)
        );

        var customerValidator = new CustomerValidator();

        // Act
        var validationResultA = customerValidator.Validate(customerA);
        var validationResultB = customerValidator.Validate(customerB);

        // Assert
        validationResultA.Should().NotBeNull();
        validationResultA.IsValid.Should().BeFalse();
        validationResultA.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_CODE
        ).Should().NotBeNull();

        validationResultB.Should().NotBeNull();
        validationResultB.IsValid.Should().BeFalse();
        validationResultB.Errors.FirstOrDefault(
            q => q.ErrorCode == DomainEntitySpecificationsBase.REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_CODE
        ).Should().NotBeNull();
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
            createdBy ?? string.Empty,
            createdAt,
            updatedBy ?? string.Empty,
            updatedAt,
            sourcePlatform ?? string.Empty,
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
public class CustomerValidator
    : AbstractValidator<Customer>
{
    public CustomerSpecifications CustomerSpecifications { get; }

    public CustomerValidator()
    {
        CustomerSpecifications = new CustomerSpecifications();

        CustomerSpecifications.AddIdIsRequiredSpecification(this);
        CustomerSpecifications.AddTenantIdIsRequiredSpecification(this);
        CustomerSpecifications.AddCreationInfoIsRequiredSpecification(this);

        CustomerSpecifications.AddCreationInfoIsValidSpecification(this);
        CustomerSpecifications.AddUpdateInfoIsRequiredSpecification(this);
        CustomerSpecifications.AddUpdateInfoIsValidSpecification(this);
        CustomerSpecifications.AddRegistryVersionIsRequiredSpecification(this);
        CustomerSpecifications.AddRegistryVersionIsValidSpecification(this);
    }
}
