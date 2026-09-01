using MediatR;

namespace Identity.Domain.Common;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    DateTime OccuredAt { get; }
}