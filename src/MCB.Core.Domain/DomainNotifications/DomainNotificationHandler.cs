using MCB.Core.Domain.Abstractions.DomainNotifications.Models;
using MCB.Core.Domain.DomainNotifications.Interfaces;

namespace MCB.Core.Domain.DomainNotifications
{
    public class DomainNotificationHandler
        : IDomainNotificationHandler
    {
        // Fields
        private readonly List<DomainNotification> _receivedDomainNotificationsCollection;

        // Properties
        public IEnumerable<DomainNotification> ReceivedDomainNotificationsCollection => _receivedDomainNotificationsCollection.AsReadOnly();

        // Contructors
        public DomainNotificationHandler()
        {
            _receivedDomainNotificationsCollection = new List<DomainNotification>();
        }

        // Public Methods
        public bool HasDomainNotifications() => _receivedDomainNotificationsCollection.Count > 0;
        public virtual Task HandlerAsync(DomainNotification subject, CancellationToken cancellationToken)
        {
            _receivedDomainNotificationsCollection.Add(subject);
            return Task.CompletedTask;
        }
    }
}
