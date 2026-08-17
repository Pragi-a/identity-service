namespace Identity.Infrastructure.Persistence.Outbox.Contracts;

public interface IIntegrationEventPublisher
{
    Task PublishAsync(OutboxMessage message, CancellationToken cancellationToken);
}