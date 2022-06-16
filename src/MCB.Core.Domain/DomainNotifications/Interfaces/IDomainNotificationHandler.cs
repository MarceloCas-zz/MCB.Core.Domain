using MCB.Core.Domain.Abstractions.DomainNotifications;
using MCB.Core.Domain.Abstractions.DomainNotifications.Models;

namespace MCB.Core.Domain.DomainNotifications.Interfaces;

public interface IDomainNotificationHandler
    : IDomainNotificationHandler<DomainNotification>
{
}
