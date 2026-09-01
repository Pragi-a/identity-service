using Identity.Infrastructure.Messaging.RabbitMq.Contracts;
using Identity.Infrastructure.Messaging.RabbitMq.Contracts.Connection;
using Identity.Infrastructure.Messaging.RabbitMq.Contracts.Scaffolding;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Identity.Infrastructure.Messaging.RabbitMq.Scaffolding;

public sealed class RabbitMqTopologyInitializerInitializer(
    IRabbitMqConnection connection,
    IOptions<RabbitMqOptions> options) : IRabbitMqTopologyInitializer
{
    private readonly RabbitMqOptions _options = options.Value;

    public async Task EnsureTopologyAsync(CancellationToken cancellationToken)
    {
        await using var channel = await connection.CreateChannelAsync(false,cancellationToken);

        await channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            autoDelete: false,
            cancellationToken: cancellationToken
        );

        await channel.QueueDeclareAsync(
            queue: "authorization-cache",
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken
        );

        await channel.QueueBindAsync(
            queue: "authorization-cache",
            exchange: _options.ExchangeName,
            routingKey: "role.permissions.*",
            cancellationToken: cancellationToken);
    }
}