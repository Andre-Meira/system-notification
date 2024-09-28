using RabbitMQ.Client;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Extesions;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;

public interface IPublishContext
{
    public Task PublishDirectMessage<TMessage>(TMessage message,
        string exchange,
        string routingKey = "",
        TimeSpan timeout = default) where TMessage : class;

    public Task PublishTopicMessage<TMessage>(TMessage message,
        string exchange,
        string routingKey = "",
        TimeSpan timeout = default) where TMessage : class;

    public Task PublishFanountMessage<TMessage>(TMessage message,
        string exchange,
        TimeSpan timeout = default) where TMessage : class;
}

internal class PublishContext : IPublishContext
{
    private readonly IModel _channel;

    public PublishContext(IModel channel) => _channel = channel;

    public Task PublishDirectMessage<TMessage>(
        TMessage message,
        string exchange,
        string routingKey = "",
        TimeSpan timeout = default) where TMessage : class
    => _channel.PublishConfirmMessage(message, exchange, ExchangeType.Direct, routingKey, timeout: timeout);

    public Task PublishFanountMessage<TMessage>(TMessage message,
        string exchange,
        TimeSpan timeout = default) where TMessage : class
        => _channel.PublishConfirmMessage(message, exchange, ExchangeType.Fanout, timeout: timeout);

    public Task PublishTopicMessage<TMessage>(TMessage message,
        string exchange,
        string routingKey = "",
        TimeSpan timeout = default)
        where TMessage : class
        => _channel.PublishConfirmMessage(message, exchange, ExchangeType.Topic, routingKey, timeout: timeout);

}