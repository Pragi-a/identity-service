using Identity.Application.Common.Events;
using Identity.Application.Common.Events.Base;

namespace Identity.Application.Interfaces;

public interface IOutbox
{
    Task AddAsync(IIntegrationEvent integrationEvent,CancellationToken cancellationToken);
}