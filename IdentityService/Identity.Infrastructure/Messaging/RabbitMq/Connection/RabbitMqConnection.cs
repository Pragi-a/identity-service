using Identity.Infrastructure.Messaging.RabbitMq.Contracts.Connection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Identity.Infrastructure.Messaging.RabbitMq.Connection;

public sealed class RabbitMqConnection(IOptions<RabbitMqOptions> options) : IRabbitMqConnection, IAsyncDisposable
{
    private readonly RabbitMqOptions _options = options.Value;

    private IConnection? _connection;


    public async Task<IChannel> CreateChannelAsync(bool publisherConfirmationEnabled,
        CancellationToken cancellationToken)
    {
        if (_connection == null)
        {
            var factory = new ConnectionFactory
            {
                HostName = _options.HostName,
                Port = _options.Port,
                UserName = _options.UserName,
                Password = _options.Password,
                VirtualHost = _options.VirtualHost,
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
        }

        var channelOptions = new CreateChannelOptions(publisherConfirmationsEnabled: publisherConfirmationEnabled,
            publisherConfirmationTrackingEnabled: publisherConfirmationEnabled);

        return await _connection.CreateChannelAsync(channelOptions, cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
        {
            await _connection.DisposeAsync();
        }
    }
}