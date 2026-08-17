using Identity.Infrastructure.Persistence.Outbox.Contracts;

namespace Identity.Infrastructure.Persistence.Outbox;

public sealed class OutboxProcessor(
    IOutboxRepository outboxRepository,
    IIntegrationEventPublisher publisher,
    IOutboxRetryPolicy retryPolicy)
    : IOutboxProcessor
{
    private const int BatchSize = 50;

    public async Task ProcessBatchAsync(CancellationToken cancellationToken)
    {
        var processorId = Environment.MachineName;

        var messages = await outboxRepository.ClaimBatchAsync(
            BatchSize,
            processorId,
            TimeSpan.FromMinutes(5),
            cancellationToken);

        foreach (var message in messages)
        {
            await ProcessMessageAsync(message, cancellationToken);
        }
    }

    private async Task ProcessMessageAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        try
        {
            await publisher.PublishAsync(message, cancellationToken);
            message.MarkPublished(DateTime.UtcNow);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            var nextRetryCount = message.RetryCount + 1;

            if (!retryPolicy.ShouldRetry(nextRetryCount))
            {
                message.MarkPermanentlyFailed(ex.Message, DateTime.UtcNow);
                return;
            }

            var delay = retryPolicy.GetDelay(nextRetryCount);
            message.MarkFailed(ex.Message, DateTime.UtcNow.Add(delay));
        }
        await outboxRepository.SaveAsync(cancellationToken);
    }
}