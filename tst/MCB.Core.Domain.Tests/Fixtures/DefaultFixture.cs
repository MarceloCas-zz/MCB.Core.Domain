using MCB.Core.Domain.Tests.DomainServicesTests;
using MCB.Core.Infra.CrossCutting.DateTime;
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
{
    // Properties
    public IServiceProvider ServiceProvider { get; }
    public DateTimeOffset InjectedUtcNow = new DateTimeOffset(DateTime.SpecifyKind(new DateTime(2022, 1, 1), DateTimeKind.Utc));

    // Constructors
    public DefaultFixture()
    {
        DateTimeProvider.GetDateCustomFunction = new Func<DateTimeOffset>(() => InjectedUtcNow);

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    // Private Methods
    private static void ConfigureServices(IServiceCollection services)
    {
        IoC.Bootstrapper.ConfigureServices(services);
        services.AddScoped<ICustomerDomainService, CustomerDomainService>();
    }
}
