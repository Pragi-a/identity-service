using System.Text;
using Identity.Infrastructure.Messaging.RabbitMq.Contracts.Connection;
using Identity.Infrastructure.Messaging.RabbitMq.Contracts.Routing;
using Identity.Infrastructure.Persistence.Outbox;
using Identity.Infrastructure.Persistence.Outbox.Contracts;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Identity.Infrastructure.Messaging.RabbitMq;

public sealed class RabbitMqIntegrationEventPublisher(
    IRabbitMqConnection rabbitMqConnection,
    IIntegrationEventRoutingStrategy routingStrategy,
    IOptions<RabbitMqOptions> options) : IIntegrationEventPublisher
{
    private readonly RabbitMqOptions _options = options.Value;

    public async Task PublishAsync(OutboxMessage message, CancellationToken cancellationToken)
    {
        var routingKey = routingStrategy.GetRoutingKey(message.EventType);

        await using var channel = await rabbitMqConnection.CreateChannelAsync(true,cancellationToken);

        var body = Encoding.UTF8.GetBytes(message.Payload);

        var properties = new BasicProperties
        {
            Persistent = true,
            MessageId = message.Id.ToString(),
            CorrelationId = message.CorrelationId,
            ContentType = "application/json",
            ContentEncoding = "utf-8",
            Type = message.EventType
        };

        await channel.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken
        );
    }
}