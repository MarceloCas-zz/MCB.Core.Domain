using MCB.Core.Domain.Abstractions.DomainEvents.Models;
using MCB.Core.Domain.Abstractions.DomainNotifications.Models;
using MCB.Core.Domain.DomainEvents;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Domain.DomainNotifications;
using MCB.Core.Domain.DomainNotifications.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MCB.Core.Domain.IoC
{
    public static class Bootstrapper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Domain Events
            services.AddScoped<IDomainEventHandler, DomainEventHandler>();
            services.AddScoped<IDomainEventPublisher, DomainEventPublisher>(serviceProvider => {
                var domainEventPublisher = new DomainEventPublisher(serviceProvider);
                domainEventPublisher.Subscribe<IDomainEventHandler, DomainEvent>();

                return domainEventPublisher;
            });

            // Domain Notifications
            services.AddScoped<IDomainNotificationHandler, DomainNotificationHandler>();
            services.AddScoped<IDomainNotificationPublisher, DomainNotificationPublisher>(serviceProvider => 
            {
                var domainEventPublisher = new DomainNotificationPublisher(serviceProvider);
                domainEventPublisher.Subscribe<IDomainNotificationHandler, DomainNotification>();

                return domainEventPublisher;
            });
        }
    }
}
