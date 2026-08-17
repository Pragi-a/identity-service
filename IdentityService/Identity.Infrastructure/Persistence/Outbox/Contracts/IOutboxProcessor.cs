namespace Identity.Infrastructure.Persistence.Outbox.Contracts;

public interface IOutboxProcessor
{
    Task ProcessBatchAsync(CancellationToken cancellationToken);
}