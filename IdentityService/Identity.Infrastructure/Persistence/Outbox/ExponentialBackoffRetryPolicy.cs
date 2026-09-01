using Identity.Infrastructure.Persistence.Outbox.Contracts;

namespace Identity.Infrastructure.Persistence.Outbox;

public sealed class ExponentialBackoffRetryPolicy : IOutboxRetryPolicy
{
    private const int MaxRetries = 10;

    private static readonly TimeSpan InitialDelay = TimeSpan.FromSeconds(5);

    private static readonly TimeSpan MaxDelay = TimeSpan.FromHours(1);


    public bool ShouldRetry(int retryCount)
    {
        return retryCount < MaxRetries;
    }

    public TimeSpan GetDelay(int retryCount)
    {
        if (retryCount <= 0) return InitialDelay;

        var exponentialDelay = InitialDelay.TotalSeconds * Math.Pow(2, retryCount - 1);

        var cappedDelay = Math.Min(exponentialDelay, MaxDelay.TotalSeconds);

        return TimeSpan.FromSeconds(cappedDelay);
    }
}