using MCB.Core.Domain.Tests.DomainServicesTests;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace MCB.Core.Domain.Tests.Fixtures
{
    [CollectionDefinition(nameof(DefaultFixture))]
    public class DefaultFixtureCollection
        : ICollectionFixture<DefaultFixture>
    {

    }
    public class DefaultFixture
    {
        // Properties
        public IServiceProvider ServiceProvider { get; }

        // Constructors
        public DefaultFixture()
        {
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
}
