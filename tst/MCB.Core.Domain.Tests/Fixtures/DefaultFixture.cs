using MCB.Core.Domain.Tests.DomainServicesTests;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Tests.Fixtures;
using Xunit;
using MCB.Core.Infra.CrossCutting.Abstractions.Serialization;
using MCB.Core.Infra.CrossCutting.Serialization;

namespace MCB.Core.Domain.Tests.Fixtures;

[CollectionDefinition(nameof(DefaultFixture))]
public class DefaultFixtureCollection
    : ICollectionFixture<DefaultFixture>
{

}
public class DefaultFixture
    : FixtureBase
{
    // Properties
    public IJsonSerializer JsonSerializer { get; }

    // Constructors
    public DefaultFixture()
    {
        JsonSerializer = new JsonSerializer();
    }

    // Protected Methods
    protected override IDependencyInjectionContainer CreateDependencyInjectionContainerInternal()
    {
        return new DependencyInjectionContainer();
    }
    protected override void BuildDependencyInjectionContainerInternal(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        ((DependencyInjectionContainer)dependencyInjectionContainer).Build();
    }
    protected override void ConfigureDependencyInjectionContainerInternal(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        dependencyInjectionContainer.RegisterScoped<ICustomerDomainService, CustomerDomainService>();

        MCB.Core.Infra.CrossCutting.IoC.Bootstrapper.ConfigureDependencyInjection(dependencyInjectionContainer);
        IoC.Bootstrapper.ConfigureServices(dependencyInjectionContainer);
    }

}