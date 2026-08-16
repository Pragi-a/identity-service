namespace Identity.Application.Common.Events;

public interface IOutbox
{
    Task AddAsync(IIntegrationEvent integrationEvent,CancellationToken cancellationToken);
}