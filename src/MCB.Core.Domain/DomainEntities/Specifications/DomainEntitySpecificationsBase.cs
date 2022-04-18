﻿using FluentValidation;
using MCB.Core.Domain.Abstractions.DomainEntities.Specifications;
using MCB.Core.Domain.Entities;

namespace MCB.Core.Domain.DomainEntities.Specifications
{
    public abstract class DomainEntitySpecificationsBase<TDomainEntity>
        : IDomainEntitySpecifications<TDomainEntity>
        where TDomainEntity : DomainEntityBase
    {
        // Constants
        public const Severity ID_IS_REQUIRED_ERROR_SEVERITY = Severity.Error;
        public const string ID_IS_REQUIRED_ERROR_CODE = "DOMAIN_ENTITY_ID_IS_REQUIRED";
        public const string ID_IS_REQUIRED_ERROR_MESSAGE = "DomainEntity.Id is required";

        public const Severity TENANT_ID_IS_REQUIRED_ERROR_SEVERITY = Severity.Error;
        public const string TENANT_ID_IS_REQUIRED_ERROR_CODE = "DOMAIN_ENTITY_TENANT_ID_IS_REQUIRED";
        public const string TENANT_ID_IS_REQUIRED_ERROR_MESSAGE = "DomainEntity.TenantId is required";

        public const Severity CREATION_INFO_IS_REQUIRED_ERROR_SEVERITY = Severity.Error;
        public const string CREATION_INFO_IS_REQUIRED_ERROR_CODE = "DOMAIN_ENTITY_CREATION_INFO_IS_REQUIRED";
        public const string CREATION_INFO_IS_REQUIRED_ERROR_MESSAGE = "DomainEntity creation info is required";

        public const Severity CREATION_INFO_SHOULD_BE_VALID_ERROR_SEVERITY = Severity.Error;
        public const string CREATION_INFO_SHOULD_BE_VALID_ERROR_CODE = "DOMAIN_ENTITY_CREATION_INFO_SHOULD_BE_VALID";
        public const string CREATION_INFO_SHOULD_BE_VALID_ERROR_MESSAGE = "DomainEntity creation info should be valid";

        public const Severity UPDATE_INFO_IS_REQUIRED_ERROR_SEVERITY = Severity.Error;
        public const string UPDATE_INFO_IS_REQUIRED_ERROR_CODE = "DOMAIN_ENTITY_UPDATE_INFO_IS_REQUIRED";
        public const string UPDATE_INFO_IS_REQUIRED_ERROR_MESSAGE = "DomainEntity update info is required";

        public const Severity UPDATE_INFO_SHOULD_BE_VALID_ERROR_SEVERITY = Severity.Error;
        public const string UPDATE_INFO_SHOULD_BE_VALID_ERROR_CODE = "DOMAIN_ENTITY_UPDATE_INFO_SHOULD_BE_VALID";
        public const string UPDATE_INFO_SHOULD_BE_VALID_ERROR_MESSAGE = "DomainEntity update info should be valid";

        public const Severity REGISTRY_VERSION_IS_REQUIRED_ERROR_SEVERITY = Severity.Error;
        public const string REGISTRY_VERSION_IS_REQUIRED_ERROR_CODE = "DOMAIN_ENTITY_REGISTRY_VERSION_IS_REQUIRED";
        public const string REGISTRY_VERSION_IS_REQUIRED_ERROR_MESSAGE = "DomainEntity.RegistryVersion is required";

        public const Severity REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_SEVERITY = Severity.Error;
        public const string REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_CODE = "DOMAIN_ENTITY_REGISTRY_VERSION_SHOULD_BE_VALID";
        public const string REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_MESSAGE = "DomainEntity.RegistryVersion should be valid";

        // Public Methods
        public void AddIdIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
        {
            validator.RuleFor(domainEntity => domainEntity.Id)
                .NotEqual(Guid.Empty)
                .WithSeverity(ID_IS_REQUIRED_ERROR_SEVERITY)
                .WithErrorCode(ID_IS_REQUIRED_ERROR_CODE)
                .WithMessage(ID_IS_REQUIRED_ERROR_MESSAGE);
        }
        public void AddTenantIdIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
        {
            validator.RuleFor(domainEntity => domainEntity.TenantId)
                .NotEqual(Guid.Empty)
                .WithSeverity(TENANT_ID_IS_REQUIRED_ERROR_SEVERITY)
                .WithErrorCode(TENANT_ID_IS_REQUIRED_ERROR_CODE)
                .WithMessage(TENANT_ID_IS_REQUIRED_ERROR_MESSAGE);
        }
        public void AddCreationInfoIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
        {
            validator.RuleFor(domainEntity => domainEntity)
                .Must(domainEntity =>
                    domainEntity != null
                    && domainEntity.AuditableInfo.CreatedAt > default(DateTimeOffset)
                    && !string.IsNullOrWhiteSpace(domainEntity.AuditableInfo.CreatedBy)
                )
                .WithSeverity(CREATION_INFO_IS_REQUIRED_ERROR_SEVERITY)
                .WithErrorCode(CREATION_INFO_IS_REQUIRED_ERROR_CODE)
                .WithMessage(CREATION_INFO_IS_REQUIRED_ERROR_MESSAGE);
        }
        public void AddCreationInfoIsValidSpecification(AbstractValidator<TDomainEntity> validator)
        {
            validator.RuleFor(domainEntity => domainEntity)
                .Must(domainEntity =>
                    domainEntity.AuditableInfo.CreatedAt <= DateTimeOffset.UtcNow
                    && domainEntity.AuditableInfo.CreatedBy.Length <= 250
                )
                .When(domainEntity =>
                    domainEntity != null
                    && domainEntity.AuditableInfo.CreatedAt > default(DateTimeOffset)
                    && !string.IsNullOrWhiteSpace(domainEntity.AuditableInfo.CreatedBy)
                )
                .WithSeverity(CREATION_INFO_SHOULD_BE_VALID_ERROR_SEVERITY)
                .WithErrorCode(CREATION_INFO_SHOULD_BE_VALID_ERROR_CODE)
                .WithMessage(CREATION_INFO_SHOULD_BE_VALID_ERROR_MESSAGE);
        }
        public void AddUpdateInfoIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
        {
            validator.RuleFor(domainEntity => domainEntity)
                .Must(domainEntity =>
                    domainEntity != null
                    && domainEntity.AuditableInfo.UpdatedAt > default(DateTimeOffset)
                    && !string.IsNullOrWhiteSpace(domainEntity.AuditableInfo.UpdatedBy)
                )
                .WithSeverity(UPDATE_INFO_IS_REQUIRED_ERROR_SEVERITY)
                .WithErrorCode(UPDATE_INFO_IS_REQUIRED_ERROR_CODE)
                .WithMessage(UPDATE_INFO_IS_REQUIRED_ERROR_MESSAGE);
        }
        public void AddUpdateInfoIsValidSpecification(AbstractValidator<TDomainEntity> validator)
        {
            validator.RuleFor(domainEntity => domainEntity)
                .Must(domainEntity =>
                    domainEntity != null
                    && domainEntity.AuditableInfo.UpdatedAt >= domainEntity.AuditableInfo.CreatedAt
                    && !string.IsNullOrWhiteSpace(domainEntity.AuditableInfo.UpdatedBy)
                )
                .When(domainEntity =>
                    domainEntity != null
                    && domainEntity.AuditableInfo.UpdatedAt > default(DateTimeOffset)
                    && !string.IsNullOrWhiteSpace(domainEntity.AuditableInfo.UpdatedBy)
                )
                .WithSeverity(UPDATE_INFO_IS_REQUIRED_ERROR_SEVERITY)
                .WithErrorCode(UPDATE_INFO_IS_REQUIRED_ERROR_CODE)
                .WithMessage(UPDATE_INFO_IS_REQUIRED_ERROR_MESSAGE);
        }
        public void AddRegistryVersionIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
        {
            validator.RuleFor(domainEntity => domainEntity.RegistryVersion)
                .Must(registryVersion => registryVersion > default(DateTimeOffset))
                .WithSeverity(REGISTRY_VERSION_IS_REQUIRED_ERROR_SEVERITY)
                .WithErrorCode(REGISTRY_VERSION_IS_REQUIRED_ERROR_CODE)
                .WithMessage(REGISTRY_VERSION_IS_REQUIRED_ERROR_MESSAGE);
        }
        public void AddRegistryVersionIsValidSpecification(AbstractValidator<TDomainEntity> validator)
        {
            validator.RuleFor(domainEntity => domainEntity)
                .Must(domainEntity =>
                    domainEntity.RegistryVersion <= DateTimeOffset.UtcNow
                    && domainEntity.RegistryVersion >= domainEntity.AuditableInfo.UpdatedAt
                )
                .When(domainEntity => domainEntity.RegistryVersion > default(DateTimeOffset))
                .WithSeverity(REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_SEVERITY)
                .WithErrorCode(REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_CODE)
                .WithMessage(REGISTRY_VERSION_SHOULD_BE_VALID_ERROR_MESSAGE);
        }
    }
}