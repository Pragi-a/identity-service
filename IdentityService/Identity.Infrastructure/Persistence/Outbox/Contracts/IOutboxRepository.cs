namespace Identity.Infrastructure.Persistence.Outbox.Contracts;

public interface IOutboxRepository
{
    Task<IReadOnlyCollection<OutboxMessage>> ClaimBatchAsync(
            int batchSize,
            string processorId,
            TimeSpan leaseDuration,
            CancellationToken cancellationToken
        );
    
    Task SaveAsync(CancellationToken cancellationToken);
}