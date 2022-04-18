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
