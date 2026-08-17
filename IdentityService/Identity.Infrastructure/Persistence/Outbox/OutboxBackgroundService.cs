using Identity.Infrastructure.Persistence.Outbox.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure.Persistence.Outbox;

public sealed class OutboxBackgroundService(IServiceScopeFactory scopeFactory, ILogger<OutboxBackgroundService> logger)
    : BackgroundService
{
    
    private static readonly TimeSpan PollingInterval = TimeSpan.FromSeconds(5);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Outbox background service Started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();

                var processor = scope.ServiceProvider.GetRequiredService<IOutboxProcessor>();

                await processor.ProcessBatchAsync(stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception e)
            {
                logger.LogError(e, "An error occurred while processing the Outbox.");
            }
            await Task.Delay(PollingInterval, stoppingToken);
        }
    }
}