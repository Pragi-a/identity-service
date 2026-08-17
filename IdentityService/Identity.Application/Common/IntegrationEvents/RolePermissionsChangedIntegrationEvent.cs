using Identity.Application.Common.Events.Base;

namespace Identity.Application.Common.Events;

public sealed record RolePermissionsChangedIntegrationEvent(
    Guid EventId,
    DateTime OccurredAt,
    string CorrelationId,
    Guid RoleId)
    : IIntegrationEvent;