using Identity.Infrastructure.Persistence.Outbox.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Outbox;

public sealed class EfCoreOutboxRepository(IdentityDbContext identityDbContext) : IOutboxRepository
{
    public async Task<IReadOnlyCollection<OutboxMessage>> ClaimBatchAsync(int batchSize, string processorId,
        TimeSpan leaseDuration, CancellationToken cancellationToken)
    {
        await using var transaction = await identityDbContext.Database.BeginTransactionAsync(cancellationToken);
        var now = DateTime.UtcNow;
        var leaseThreshold = now - leaseDuration;

        var messages = await identityDbContext.OutboxMessages.FromSqlInterpolated($"""
             SELECT * FROM auth.outbox_messages
             WHERE published_at IS NULL
             AND failed_at  IS NULL
             AND (
                 
                 processing_started_at IS NULL
                 OR processing_started_at < {leaseThreshold}
             )
             AND(
                 next_attempt_at IS NULL
                 OR next_attempt_at < {now}
             )
             ORDER BY occurred_at
             LIMIT {batchSize}
             FOR UPDATE SKIP LOCKED
             """).ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            message.Claim(processorId, now);
        }

        await identityDbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);
        return messages;
    }

    public Task SaveAsync(CancellationToken cancellationToken)
    {
        return identityDbContext.SaveChangesAsync(cancellationToken);
    }
}