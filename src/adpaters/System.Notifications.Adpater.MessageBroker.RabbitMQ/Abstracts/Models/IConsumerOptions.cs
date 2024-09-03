using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Threading.Channels;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Models;

public interface IConsumerOptions
{
    public string Exchange { get; protected set; }
    public string RoutingKey { get; protected set; }
    public string ExchageType { get; protected set; }
    public ushort PrefetchCount { get; protected set; }

    public string ExchangeConsumer { get; protected set; }
    public string ExchangeTypeConsumer { get; protected set; }
    
    //TODO
    public IFaultConsumerConfiguration? FaultConfig { get; protected set; }

    public void Validate();

    public IConsumerOptions ConfigureExchangeConsumer(string exchange, string exchangeType);

    public IConsumerOptions Configure(string exchange, string exchangeType,
        string routingKey = "", Dictionary<string, object>? arguments = null);
}


internal record ConsumerOptions : IConsumerOptions
{
    private readonly IModel _channel;

    public ConsumerOptions(IModel model) => _channel = model;

    public string Exchange { get; set; } = null!;
    public string RoutingKey { get; set; } = null!;
    public string ExchageType { get; set; } = null!;
    public ushort PrefetchCount { get; set; }
    public string ExchangeConsumer { get; set; } = null!;
    public string ExchangeTypeConsumer { get; set; } = null!;

    IFaultConsumerConfiguration? IConsumerOptions.FaultConfig { get; set; }

    public IConsumerOptions Configure(string exchange, 
        string exchangeType, string routingKey = "",
        Dictionary<string, object>? arguments = null)
    {
        Exchange = exchange;
        ExchageType = exchangeType;
        RoutingKey = routingKey ?? string.Empty;

        _channel.ExchangeDeclare(exchange, exchangeType, true, arguments: arguments);
        _channel.QueueDeclare(exchange, true, false, false);

        _channel.QueueBind(exchange, exchange, routingKey, null);

        return this;
    }

    public IConsumerOptions ConfigureExchangeConsumer(string exchange, string exchangeType)
    {
        ExchangeConsumer = exchange;
        ExchangeTypeConsumer = exchangeType;

        _channel.ExchangeDeclare(exchange, exchangeType, true);

        return this;
    }

    public void Validate()
    {
        _channel.ExchangeBind(Exchange, ExchangeConsumer, RoutingKey);
    }
}
