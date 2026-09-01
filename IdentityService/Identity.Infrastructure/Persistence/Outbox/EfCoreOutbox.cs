using System.Text.Json;
using Identity.Application.Common.Events.Base;
using Identity.Application.Interfaces;

namespace Identity.Infrastructure.Persistence.Outbox;

public sealed class EfCoreOutbox(IdentityDbContext dbContext) : IOutbox
{
    public Task AddAsync(IIntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        var message = new OutboxMessage(
            integrationEvent.EventId,
            integrationEvent.GetType().Name,
            JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()),
            integrationEvent.OccurredAt,
            integrationEvent.CorrelationId
        );

        dbContext.OutboxMessages.Add(message);

        return Task.CompletedTask;
    }
}