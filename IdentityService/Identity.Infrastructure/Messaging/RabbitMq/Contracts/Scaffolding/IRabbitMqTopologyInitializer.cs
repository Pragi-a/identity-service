namespace Identity.Infrastructure.Messaging.RabbitMq.Contracts.Scaffolding;

public interface IRabbitMqTopologyInitializer
{
    Task EnsureTopologyAsync( CancellationToken cancellationToken);
}