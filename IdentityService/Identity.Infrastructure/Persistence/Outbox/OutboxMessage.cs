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

    
    private OutboxMessage(){}
    public OutboxMessage(Guid id, string eventType,string payload, DateTime occurredAt, string correlationId)
    {
        Id = id;
        EventType = eventType;
        Payload = payload;
        OccurredAt = occurredAt;
        CorrelationId = correlationId;
        RetryCount = 0;
    }
}