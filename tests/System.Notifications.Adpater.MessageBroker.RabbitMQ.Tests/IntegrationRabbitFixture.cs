using RabbitMQ.Client;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Tests;

public class IntegrationRabbitFixture
{
    public IAsyncConnectionFactory AsyncConnectionFactory { get; init; }

    public IntegrationRabbitFixture()
    {
        AsyncConnectionFactory = new ConnectionFactory();
    }
}
