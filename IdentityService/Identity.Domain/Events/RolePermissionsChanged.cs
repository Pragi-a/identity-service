using Identity.Domain.Common;

namespace Identity.Domain.Events;

public sealed record RolePermissionsChanged(Guid EventId, DateTime OccuredAt, Guid RoleId) : IDomainEvent;