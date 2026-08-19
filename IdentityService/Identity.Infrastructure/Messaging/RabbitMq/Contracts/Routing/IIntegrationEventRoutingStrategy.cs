namespace Identity.Infrastructure.Messaging.RabbitMq.Contracts.Routing;

public interface IIntegrationEventRoutingStrategy
{
    string GetRoutingKey(string eventType);
}