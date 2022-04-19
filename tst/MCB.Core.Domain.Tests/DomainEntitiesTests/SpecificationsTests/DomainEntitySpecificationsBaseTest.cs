using FluentAssertions;
using FluentValidation;
using MCB.Core.Domain.DomainEntities.Specifications;
using MCB.Core.Domain.Entities;
using System;
using System.Linq;
using Xunit;

namespace MCB.Core.Domain.Tests.DomainEntitiesTests.SpecificationsTests
{
    public class DomainEntitySpecificationsBaseTest
    {
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
                createdAt: DateTime.UtcNow,
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
                createdAt: DateTime.UtcNow,
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
                createdAt: DateTime.UtcNow,
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
                createdAt: DateTimeOffset.UtcNow.AddDays(1),
                createdBy: "marcelo.castelo@outlook.com"
            );
            var customerB = new Customer();
            customerB.SetExistingInfoExposed(
                createdAt: DateTimeOffset.UtcNow,
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
            var customer = new Customer();
            customer.SetExistingInfoExposed(
                updatedAt: DateTime.UtcNow,
                updatedBy: "marcelo.castelo@outlook.com"
            );
            var customerValidator = new CustomerValidator();

            // Act
            var validationResult = customerValidator.Validate(customer);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.Errors.FirstOrDefault(
                q => q.ErrorCode == DomainEntitySpecificationsBase.UPDATE_INFO_SHOULD_BE_VALID_ERROR_CODE
            ).Should().BeNull();
        }
        [Fact]
        public void AddUpdateInfoIsValidSpecification_Should_Not_Pass()
        {
            // Arrange
            var customerA = new Customer();
            customerA.SetExistingInfoExposed(
                createdAt: DateTimeOffset.UtcNow.AddDays(-1),
                updatedAt: DateTimeOffset.UtcNow.AddDays(-2),
                updatedBy: "marcelo.castelo@outlook.com"
            );
            var customerB = new Customer();
            customerB.SetExistingInfoExposed(
                createdAt: DateTimeOffset.UtcNow.AddDays(-1),
                updatedAt: DateTimeOffset.UtcNow.AddDays(1),
                updatedBy: "marcelo.castelo@outlook.com"
            );
            var customerC = new Customer();
            customerC.SetExistingInfoExposed(
                createdAt: DateTimeOffset.UtcNow.AddDays(-2),
                updatedAt: DateTimeOffset.UtcNow.AddDays(-1),
                updatedBy: new string('a', 251)
            );
            var customerValidator = new CustomerValidator();

            // Act
            var validationResultA = customerValidator.Validate(customerA);
            var validationResultB = customerValidator.Validate(customerB);
            var validationResultC = customerValidator.Validate(customerC);

            // Assert
            validationResultA.Should().NotBeNull();
            validationResultA.Errors.FirstOrDefault(
                q => q.ErrorCode == DomainEntitySpecificationsBase.UPDATE_INFO_SHOULD_BE_VALID_ERROR_CODE
            ).Should().NotBeNull();

            validationResultB.Should().NotBeNull();
            validationResultB.Errors.FirstOrDefault(
                q => q.ErrorCode == DomainEntitySpecificationsBase.UPDATE_INFO_SHOULD_BE_VALID_ERROR_CODE
            ).Should().NotBeNull();

            validationResultC.Should().NotBeNull();
            validationResultC.Errors.FirstOrDefault(
                q => q.ErrorCode == DomainEntitySpecificationsBase.UPDATE_INFO_SHOULD_BE_VALID_ERROR_CODE
            ).Should().NotBeNull();
        }

        [Fact]
        public void AddRegistryVersionIsRequiredSpecification_Should_Pass()
        {
            // Arrange
            var customer = new Customer();
            customer.SetExistingInfoExposed(registryVersion: DateTimeOffset.UtcNow);
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
            var customer = new Customer();
            customer.SetExistingInfoExposed(
                updatedAt: DateTimeOffset.UtcNow.AddDays(-2),
                registryVersion: DateTimeOffset.UtcNow.AddDays(-1)
            );
            var customerValidator = new CustomerValidator();

            // Act
            var validationResult = customerValidator.Validate(customer);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.Errors.FirstOrDefault(
                q => q.ErrorCode == DomainEntitySpecificationsBase.REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_CODE
            ).Should().BeNull();
        }
        [Fact]
        public void AddRegistryVersionIsValidSpecification_Should_Not_Pass()
        {
            // Arrange
            var customerA = new Customer();
            customerA.SetExistingInfoExposed(
                updatedAt: DateTimeOffset.UtcNow.AddDays(-1),
                registryVersion: DateTimeOffset.UtcNow.AddDays(-2)
            );

            var customerB = new Customer();
            customerB.SetExistingInfoExposed(
                registryVersion: DateTimeOffset.UtcNow.AddDays(1)
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
        public void SetExistingInfoExposed(
            Guid id = default, 
            Guid tenantId = default, 
            string? createdBy = default, 
            DateTimeOffset createdAt = default, 
            string? updatedBy = default, 
            DateTimeOffset? updatedAt = default, 
            string? sourcePlatform = default, 
            DateTimeOffset registryVersion = default
        )
        {
            SetExistingInfo(
                id,
                tenantId,
                createdBy ?? string.Empty,
                createdAt,
                updatedBy ?? string.Empty,
                updatedAt,
                sourcePlatform ?? string.Empty,
                registryVersion
            );
        }
    }
    public class CustomerSpecifications
        : DomainEntitySpecificationsBase<Customer>
    {

    }
    public class CustomerValidator
        : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            var customerSpecifications = new CustomerSpecifications();

            customerSpecifications.AddIdIsRequiredSpecification(this);
            customerSpecifications.AddTenantIdIsRequiredSpecification(this);
            customerSpecifications.AddCreationInfoIsRequiredSpecification(this);

            customerSpecifications.AddCreationInfoIsValidSpecification(this);
            customerSpecifications.AddUpdateInfoIsRequiredSpecification(this);
            customerSpecifications.AddUpdateInfoIsValidSpecification(this);
            customerSpecifications.AddRegistryVersionIsRequiredSpecification(this);
            customerSpecifications.AddRegistryVersionIsValidSpecification(this);
        }
    }
}
