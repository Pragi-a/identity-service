using System.Diagnostics;
using Identity.Application.Common.Events;
using Identity.Domain.Events;
using MediatR;

namespace Identity.Application.Common.DomainEvents;

public sealed class RolePermissionsChangedHandler(IOutbox outbox) : INotificationHandler<RolePermissionsChanged>
{
    public async Task Handle(RolePermissionsChanged notification, CancellationToken cancellationToken)
    {
        var correlationId = Activity.Current?.TraceId.ToString() ?? Guid.NewGuid().ToString("N");

        var integrationEvent = new RolePermissionsChangedIntegrationEvent(notification.EventId, notification.OccuredAt,
            correlationId, notification.RoleId);
        
        await outbox.AddAsync(integrationEvent, cancellationToken);
    }
}