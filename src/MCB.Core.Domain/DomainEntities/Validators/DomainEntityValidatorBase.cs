using MCB.Core.Domain.Abstractions.DomainEntities.Specifications;
using MCB.Core.Domain.Abstractions.DomainEntities.Validators;
using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;

namespace MCB.Core.Domain.DomainEntities.Validators;

public abstract class DomainEntityValidatorBase<TDomainEntity>
    : ValidatorBase<TDomainEntity>,
    IDomainEntityValidator<TDomainEntity>
    where TDomainEntity : IDomainEntity
{
    // Fields
    private readonly IDomainEntitySpecifications<TDomainEntity> _domainEntitySpecifications;

    // Constructors
    protected DomainEntityValidatorBase(
        IDomainEntitySpecifications<TDomainEntity> domainEntitySpecifications
    )
    {
        _domainEntitySpecifications = domainEntitySpecifications;
    }

    // Protected Methods
    protected void AddSpecificationsForCreation()
    {
        _domainEntitySpecifications.AddIdIsRequiredSpecification(FluentValidationValidatorWrapperInstance);
        _domainEntitySpecifications.AddTenantIdIsRequiredSpecification(FluentValidationValidatorWrapperInstance);
        _domainEntitySpecifications.AddCreationInfoIsRequiredSpecification(FluentValidationValidatorWrapperInstance);
        _domainEntitySpecifications.AddCreationInfoIsValidSpecification(FluentValidationValidatorWrapperInstance);
        _domainEntitySpecifications.AddRegistryVersionIsRequiredSpecification(FluentValidationValidatorWrapperInstance);
        _domainEntitySpecifications.AddRegistryVersionIsValidSpecification(FluentValidationValidatorWrapperInstance);
    }
    protected void AddSpecificationsForUpdate()
    {
        AddSpecificationsForCreation();

        _domainEntitySpecifications.AddUpdateInfoIsRequiredSpecification(FluentValidationValidatorWrapperInstance);
        _domainEntitySpecifications.AddUpdateInfoIsValidSpecification(FluentValidationValidatorWrapperInstance);
    }
}
