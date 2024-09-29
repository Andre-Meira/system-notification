using RabbitMQ.Client;
using System.Diagnostics;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Monitory;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Extesions;

internal static class PublishContextExtesions
{
    public static async Task PublishConfirmMessage<TMessage>(
        this IModel model,
        TMessage message,
        string exchange,
        string exchangeType,
        string routingKey = "",
        IBasicProperties? basicProperties = null,
        TimeSpan timeout = default)
        where TMessage : class
    {
        ActivityBus? activityBus = message.CreatePublishActivityBus(exchange, exchangeType, routingKey);
        activityBus?.Start();

        model.ConfirmSelect();
        try
        {
            timeout = timeout == default ? TimeSpan.FromMinutes(1) : timeout;

            model.ExchangeDeclare(exchange, exchangeType, true);
            byte[] body = message.SerializationMessage();

            await Task.Run(() =>
            {
                IBasicProperties properties = basicProperties ?? GetProperties(model);

                model.BasicPublish(exchange: exchange, routingKey, properties, body);
                model.WaitForConfirmsOrDie(timeout);
            });
        }
        catch (Exception err)
        {
            activityBus?.AddExceptionEvent(err);
            throw;
        }
        finally
        {
            activityBus?.Stop();
        }
    }


    public static IBasicProperties GetProperties(this IModel model)
    {
        IBasicProperties properties = model.CreateBasicProperties();
        properties.DeliveryMode = 2;
        properties.ContentType = "application/json";

        properties.Headers = new Dictionary<string, object?>()
        {
            { ActivityConstants.Header_Id,  Activity.Current?.Id?.ToString() },
            { ActivityConstants.Header_TraceId,  Activity.Current?.TraceId.ToString() },
            { ActivityConstants.Header_SpanId, Activity.Current?.Context.SpanId.ToString() },
            { ActivityConstants.Header_TraceFlags, Activity.Current?.Context.TraceFlags.ToString() },
            { ActivityConstants.Header_TraceState, Activity.Current?.Context.TraceState?.ToString() }
        };

        return properties;
    }

}