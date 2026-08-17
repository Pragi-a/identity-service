namespace Identity.Infrastructure.Persistence.Outbox.Contracts;

public interface IOutboxRetryPolicy
{
    bool ShouldRetry(int retryCount);
    
    TimeSpan GetDelay(int retryCount);
}