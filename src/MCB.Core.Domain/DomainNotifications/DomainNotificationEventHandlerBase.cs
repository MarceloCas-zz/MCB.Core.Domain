using MCB.Core.Domain.Abstractions.DomainNotifications;
using MCB.Core.Domain.Abstractions.DomainNotifications.Models;

namespace MCB.Core.Domain.DomainNotifications
{
    public abstract class DomainNotificationEventHandlerBase
        : IDomainNotificationEventHandler<DomainNotification>
    {
        // Fields
        private readonly List<DomainNotification> _raisedDomainNotificationsCollection;

        // Properties
        public IEnumerable<DomainNotification> RaisedDomainNotificationsCollection => _raisedDomainNotificationsCollection.AsReadOnly();

        // Contructors
        protected DomainNotificationEventHandlerBase()
        {
            _raisedDomainNotificationsCollection = new List<DomainNotification>();
        }

        // Public Methods
        public bool HasDomainNotifications() => _raisedDomainNotificationsCollection.Count > 0;
        public virtual Task HandlerAsync(DomainNotification subject, CancellationToken cancellationToken)
        {
            _raisedDomainNotificationsCollection.Add(subject);
            return Task.CompletedTask;
        }
    }
}
