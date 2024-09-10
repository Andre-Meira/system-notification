using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Extesions;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Models;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Monitory;
using System.Text;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;

public class ConsumerHandlerBase<TMessage, TConsumerHandler> : 
    BackgroundService, IDisposable
    where TMessage : class
    where TConsumerHandler : IConsumerHandler<TMessage>
{
    #region Configuracao
    private readonly IModel _channel;

    private readonly IConsumerOptions _consumerOptions;
    private readonly IServiceScopeFactory _serviceScope;

    private readonly ILogger<ConsumerHandlerBase<TMessage, TConsumerHandler>> _logger;
    private readonly IFaultConsumerConfiguration? _configDeadLetter;
    private readonly IConnection _connection;
    


    private bool IsRetryMessage => _configDeadLetter is not null;
    private string ExchageNameRetry => _consumerOptions.Exchange + "-retry";

    public ConsumerHandlerBase(IConnection connection,
        IServiceScopeFactory serviceScope,
        IConsumerOptions consumerOptions,
        ILogger<ConsumerHandlerBase<TMessage, TConsumerHandler>> logger)
    {        
        if (consumerOptions.FaultConfig is not null) _configDeadLetter = consumerOptions.FaultConfig;

        _channel = connection.CreateModel();
        _consumerOptions = consumerOptions;
        _connection = connection;
        
        _logger = logger;
        _serviceScope = serviceScope;

        Initilize();
    }

    private void Initilize()
    {        
        if (IsRetryMessage)
        {
            _channel.ExchangeDeclare(ExchageNameRetry, "topic", true, false);
            _channel.QueueDeclare(ExchageNameRetry, true, false, false);
            _channel.QueueBind(ExchageNameRetry, ExchageNameRetry, "", null);
        }

        if (_consumerOptions.PrefetchCount > 0) 
            _channel.BasicQos(0, _consumerOptions.PrefetchCount, false);

    }
    #endregion    

    #region Metodos/Functions
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumerEvent = new AsyncEventingBasicConsumer(_channel);

        await Task.Run(() =>
        {
            consumerEvent.Received += async (model, ea) =>
            {
                ea.Exchange = _consumerOptions.Exchange;
                await ReceivedMessageAsync(ea);
            };
        });

        _channel.BasicConsume(_consumerOptions.Exchange, false, consumerEvent);        
    }

    private async Task ReceivedMessageAsync(BasicDeliverEventArgs args)
    {
        ActivityBus? activityBus = args.CreateConsumerActivityBus<TConsumerHandler>();
        activityBus?.Start();

        var service = _serviceScope.CreateScope();
        var consumerHandler = service.ServiceProvider.GetRequiredService<TConsumerHandler>();

        try
        {
            TMessage message = TransformMessage(args);

            var context = new ConsumerContext<TMessage>(message, args.DeliveryTag, _channel);
            await consumerHandler.Consumer(context).ConfigureAwait(false);
        }
        catch (Exception err) when (err is not ArgumentNullException)
        {
            _logger.LogError("Consumer: {0}, error: {1}", consumerHandler.GetType().Name, err.Message);
            activityBus?.AddExceptionEvent(err);
            
            await _channel.SendQueueFault(TransformMessage(args), err, _consumerOptions.Exchange);
            _channel.BasicNack(args.DeliveryTag, false, false);
        }
        finally
        {
            service.Dispose();  
            activityBus?.Stop();
        }
    }

    private TMessage TransformMessage(BasicDeliverEventArgs eventArgs)
    {
        try
        {
            var body = eventArgs.Body.ToArray();
            var messageString = Encoding.UTF8.GetString(body);

            TMessage? message = JsonConvert.DeserializeObject<TMessage>(messageString);

            if (message is null)
            {
                throw new ArgumentNullException("message is null");
            }

            return message;
        }
        catch (Exception err)
        {
            _logger.LogError("Fail transform body the message, err: {0}", err.Message);
            _channel.BasicReject(eventArgs.DeliveryTag, false);
            throw;
        }
    }
    
    public override void Dispose()
    {
        _channel.Dispose();
        _connection.Dispose();        
    }
    #endregion
}

