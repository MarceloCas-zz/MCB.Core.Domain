using MCB.Core.Domain.Abstractions.DomainEvents.Models;
using MCB.Core.Domain.Abstractions.DomainNotifications.Models;
using MCB.Core.Domain.DomainEvents;
using MCB.Core.Domain.DomainEvents.Interfaces;
using MCB.Core.Domain.DomainNotifications;
using MCB.Core.Domain.DomainNotifications.Interfaces;
using MCB.Core.Infra.CrossCutting.DependencyInjection.Abstractions.Interfaces;

namespace MCB.Core.Domain.IoC;

public static class Bootstrapper
{
    public static void ConfigureServices(IDependencyInjectionContainer dependencyInjectionContainer)
    {
        // Domain Events
        dependencyInjectionContainer.RegisterScoped<IDomainEventHandler, DomainEventHandler>();
        dependencyInjectionContainer.RegisterScoped<IDomainEventPublisher, DomainEventPublisher>(dependencyInjectionContainer => {
            var domainEventPublisher = new DomainEventPublisher(dependencyInjectionContainer);
            domainEventPublisher.Subscribe<IDomainEventHandler, DomainEvent>();

            return domainEventPublisher;
        });

        // Domain Notifications
        dependencyInjectionContainer.RegisterScoped<IDomainNotificationHandler, DomainNotificationHandler>();
        dependencyInjectionContainer.RegisterScoped<IDomainNotificationPublisher, DomainNotificationPublisher>(dependencyInjectionContainer => 
        {
            var domainEventPublisher = new DomainNotificationPublisher(dependencyInjectionContainer);
            domainEventPublisher.Subscribe<IDomainNotificationHandler, DomainNotification>();

            return domainEventPublisher;
        });
    }
}
