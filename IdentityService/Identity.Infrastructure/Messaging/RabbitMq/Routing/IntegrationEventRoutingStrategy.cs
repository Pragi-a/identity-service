using Identity.Application.Common.IntegrationEvents;
using Identity.Infrastructure.Messaging.RabbitMq.Contracts.Routing;

namespace Identity.Infrastructure.Messaging.RabbitMq.Routing;

public sealed class IntegrationEventRoutingStrategy : IIntegrationEventRoutingStrategy
{
    private const string RolePermissionsChanged = nameof(RolePermissionsChangedIntegrationEvent);

    public string GetRoutingKey(string eventType)
    {
        return eventType switch
        {
            RolePermissionsChanged => "role.permissions.changed",

            _ => throw new InvalidOperationException(
                $"No Routing Key configured for the event type '{eventType}'."
            )
        };
    }
}