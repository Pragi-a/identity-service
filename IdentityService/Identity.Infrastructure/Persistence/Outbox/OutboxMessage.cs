namespace Identity.Infrastructure.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }

    public string EventType { get; private set; } = string.Empty;

    public string Payload { get; private set; } = string.Empty;

    public DateTime OccurredAt { get; private set; }

    public string CorrelationId { get; private set; }

    public DateTime? PublishedAt { get; private set; }

    public int RetryCount { get; private set; }

    public string? Error { get; private set; }

    public DateTime? ProcessingStartedAt { get; private set; }

    public string? ProcessingBy { get; private set; }

    public DateTime? NextAttemptAt { get; private set; }

    public DateTime? FailedAt { get; private set; }

    private OutboxMessage()
    {
    }

    public OutboxMessage(Guid id, string eventType, string payload, DateTime occurredAt, string correlationId)
    {
        Id = id;
        EventType = eventType;
        Payload = payload;
        OccurredAt = occurredAt;
        CorrelationId = correlationId;
        RetryCount = 0;
    }

    public void Claim(string processorId, DateTime processingStartedAt)
    {
        ProcessingBy = processorId;
        ProcessingStartedAt = processingStartedAt;
    }

    public void MarkPublished(DateTime publishedAt)
    {
        PublishedAt = publishedAt;
        ProcessingStartedAt = null;
        ProcessingBy = null;
        Error = null;
    }

    public void MarkFailed(string error, DateTime nextAttemptAt)
    {
        RetryCount++;
        Error = error;
        ProcessingStartedAt = null;
        ProcessingBy = null;

        NextAttemptAt = nextAttemptAt;
    }

    public void MarkPermanentlyFailed(string error, DateTime failedAt)
    {
        RetryCount++;
        Error = error;
        FailedAt = failedAt;
        NextAttemptAt = null;
        ProcessingBy = null;
        ProcessingStartedAt = null;
    }
}