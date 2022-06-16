using FluentValidation;
using MCB.Core.Domain.Abstractions.DomainEntities.Specifications;
using MCB.Core.Domain.Entities;
using MCB.Core.Infra.CrossCutting.DateTime;

namespace MCB.Core.Domain.DomainEntities.Specifications;

public abstract class DomainEntitySpecificationsBase
{
    // Static Fields
    public static readonly Severity idIsRequiredErrorSeverity = Severity.Error;
    public static readonly string idIsRequiredErrorCode = "DOMAIN_ENTITY_ID_IS_REQUIRED";
    public static readonly string idIsRequiredErrorMessage = "DomainEntity.Id is required";

    public static readonly Severity tenantIdIsRequiredErrorSeverity = Severity.Error;
    public static readonly string tenantIdIsRequiredErrorCode = "DOMAIN_ENTITY_TENANT_ID_IS_REQUIRED";
    public static readonly string tenantIdIsRequiredErrorMessage = "DomainEntity.TenantId is required";

    public static readonly Severity creationInfoIsRequiredErrorSeverity = Severity.Error;
    public static readonly string creationInfoIsRequiredErrorCode = "DOMAIN_ENTITY_CREATION_INFO_IS_REQUIRED";
    public static readonly string creationInfoIsRequiredErrorMessage = "DomainEntity creation info is required";

    public static readonly Severity creationInfoShouldBeValidErrorSeverity = Severity.Error;
    public static readonly string creationInfoShouldBeValidErrorCode = "DOMAIN_ENTITY_CREATION_INFO_SHOULD_BE_VALID";
    public static readonly string creationInfoShouldBeValidErrorMessage = "DomainEntity creation info should be valid";

    public static readonly Severity updateInfoIsRequiredErrorSeverity = Severity.Error;
    public static readonly string updateInfoIsRequiredErrorCode = "DOMAIN_ENTITY_UPDATE_INFO_IS_REQUIRED";
    public static readonly string updateInfoIsRequiredErrorMessage = "DomainEntity update info is required";

    public static readonly Severity updateInfoShouldBeValidErrorSeverity = Severity.Error;
    public static readonly string updateInfoShouldBeValidErrorCode = "DOMAIN_ENTITY_UPDATE_INFO_SHOULD_BE_VALID";
    public static readonly string updateInfoShouldBeValidErrorMessage = "DomainEntity update info should be valid";

    public static readonly Severity registryVersionIsRequiredErrorSeverity = Severity.Error;
    public static readonly string registryVersionIsRequiredErrorCode = "DOMAIN_ENTITY_REGISTRY_VERSION_IS_REQUIRED";
    public static readonly string registryVersionIsRequiredErrorMessage = "DomainEntity.RegistryVersion is required";

    public static readonly Severity registryVersionShouldBeValidErrorSeverity = Severity.Error;
    public static readonly string registryVersionShouldBeValidErrorCode = "DOMAIN_ENTITY_REGISTRY_VERSION_SHOULD_BE_VALID";
    public static readonly string registryVersionShouldBeValidErrorMessage = "DomainEntity.RegistryVersion should be valid";
}

public abstract class DomainEntitySpecificationsBase<TDomainEntity>
    : DomainEntitySpecificationsBase,
    IDomainEntitySpecifications<TDomainEntity>
    where TDomainEntity : DomainEntityBase
{
    // Private Methods
    private static bool CheckCreationInfoIsRequired(TDomainEntity domainEntity)
    {
        return domainEntity.AuditableInfo.CreatedAt > default(DateTimeOffset)
            && !string.IsNullOrWhiteSpace(domainEntity.AuditableInfo.CreatedBy);
    }
    private static bool CheckUpdateInfoIsRequired(TDomainEntity domainEntity)
    {
        return domainEntity.AuditableInfo.LastUpdatedAt > default(DateTimeOffset)
            && !string.IsNullOrWhiteSpace(domainEntity.AuditableInfo.LastUpdatedBy);
    }
    private static bool CheckRegistryVersionIsRequired(DateTimeOffset registryVersion)
    {
        return registryVersion > default(DateTimeOffset);
    }

    // Public Methods
    public void AddIdIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
    {
        validator.RuleFor(domainEntity => domainEntity.Id)
            .NotEqual(Guid.Empty)
            .WithSeverity(idIsRequiredErrorSeverity)
            .WithErrorCode(idIsRequiredErrorCode)
            .WithMessage(idIsRequiredErrorMessage);
    }
    public void AddTenantIdIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
    {
        validator.RuleFor(domainEntity => domainEntity.TenantId)
            .NotEqual(Guid.Empty)
            .WithSeverity(tenantIdIsRequiredErrorSeverity)
            .WithErrorCode(tenantIdIsRequiredErrorCode)
            .WithMessage(tenantIdIsRequiredErrorMessage);
    }
    public void AddCreationInfoIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
    {
        validator.RuleFor(domainEntity => domainEntity)
            .Must(domainEntity => CheckCreationInfoIsRequired(domainEntity))
            .WithSeverity(creationInfoIsRequiredErrorSeverity)
            .WithErrorCode(creationInfoIsRequiredErrorCode)
            .WithMessage(creationInfoIsRequiredErrorMessage);
    }
    public void AddCreationInfoIsValidSpecification(AbstractValidator<TDomainEntity> validator)
    {
        validator.RuleFor(domainEntity => domainEntity)
            .Must(domainEntity =>
                domainEntity.AuditableInfo.CreatedAt <= DateTimeProvider.GetDate()
                && domainEntity.AuditableInfo.CreatedBy.Length <= 250
            )
            .When(domainEntity => CheckCreationInfoIsRequired(domainEntity))
            .WithSeverity(creationInfoShouldBeValidErrorSeverity)
            .WithErrorCode(creationInfoShouldBeValidErrorCode)
            .WithMessage(creationInfoShouldBeValidErrorMessage);
    }
    public void AddUpdateInfoIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
    {
        validator.RuleFor(domainEntity => domainEntity)
            .Must(domainEntity => CheckUpdateInfoIsRequired(domainEntity))
            .WithSeverity(updateInfoIsRequiredErrorSeverity)
            .WithErrorCode(updateInfoIsRequiredErrorCode)
            .WithMessage(updateInfoIsRequiredErrorMessage);
    }
    public void AddUpdateInfoIsValidSpecification(AbstractValidator<TDomainEntity> validator)
    {
        validator.RuleFor(domainEntity => domainEntity)
            .Must(domainEntity => 
                domainEntity.AuditableInfo.LastUpdatedAt >= domainEntity.AuditableInfo.CreatedAt
                && domainEntity.AuditableInfo.LastUpdatedAt <= DateTimeProvider.GetDate()
                && domainEntity.AuditableInfo.LastUpdatedBy.Length <= 250
            )
            .When(domainEntity => CheckUpdateInfoIsRequired(domainEntity))
            .WithSeverity(updateInfoShouldBeValidErrorSeverity)
            .WithErrorCode(updateInfoShouldBeValidErrorCode)
            .WithMessage(updateInfoShouldBeValidErrorMessage);
    }
    public void AddRegistryVersionIsRequiredSpecification(AbstractValidator<TDomainEntity> validator)
    {
        validator.RuleFor(domainEntity => domainEntity.RegistryVersion)
            .Must(registryVersion => CheckRegistryVersionIsRequired(registryVersion))
            .WithSeverity(registryVersionIsRequiredErrorSeverity)
            .WithErrorCode(registryVersionIsRequiredErrorCode)
            .WithMessage(registryVersionIsRequiredErrorMessage);
    }
    public void AddRegistryVersionIsValidSpecification(AbstractValidator<TDomainEntity> validator)
    {
        validator.RuleFor(domainEntity => domainEntity)
            .Must(domainEntity =>
                domainEntity.RegistryVersion <= DateTimeProvider.GetDate()
                && domainEntity.RegistryVersion >= (domainEntity.AuditableInfo.LastUpdatedAt ?? default)
            )
            .When(domainEntity => CheckRegistryVersionIsRequired(domainEntity.RegistryVersion))
            .WithSeverity(registryVersionShouldBeValidErrorSeverity)
            .WithErrorCode(registryVersionShouldBeValidErrorCode)
            .WithMessage(registryVersionShouldBeValidErrorMessage);
    }
}
