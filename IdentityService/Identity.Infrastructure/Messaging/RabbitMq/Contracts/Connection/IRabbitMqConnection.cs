using RabbitMQ.Client;

namespace Identity.Infrastructure.Messaging.RabbitMq.Contracts.Connection;

public interface IRabbitMqConnection
{
    Task<IChannel> CreateChannelAsync(bool publisherConfirmationEnable ,CancellationToken cancellationToken);
}