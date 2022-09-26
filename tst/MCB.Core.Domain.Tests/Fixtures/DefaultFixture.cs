using MCB.Core.Domain.Tests.DomainServicesTests;
using MCB.Core.Infra.CrossCutting.DateTime;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;
using MCB.Core.Infra.CrossCutting.DependencyInjection;
using MCB.Tests.Fixtures;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MCB.Core.Domain.Tests.Fixtures;

[CollectionDefinition(nameof(DefaultFixture))]
public class DefaultFixtureCollection
    : ICollectionFixture<DefaultFixture>
{

}
public class DefaultFixture
    : FixtureBase
{
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

        IoC.Bootstrapper.ConfigureServices(dependencyInjectionContainer);
    }

}