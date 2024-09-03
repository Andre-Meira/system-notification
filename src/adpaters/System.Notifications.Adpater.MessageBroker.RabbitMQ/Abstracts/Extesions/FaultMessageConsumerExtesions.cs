using RabbitMQ.Client;
using System.ComponentModel.DataAnnotations;
using System.Threading.Channels;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Extesions;

internal static class FaultMessageConsumerExtesions
{
    public static async Task SendQueueFault<TMessage>(this IModel model, 
        TMessage message, Exception exception, string nameConsumer)
        where TMessage : class
    {        
        string nameQueue = nameConsumer + "-error";

        model.ExchangeDeclare(nameQueue, "topic", true, false);
        model.QueueDeclare(nameQueue, true, false, false);
        model.QueueBind(nameQueue, nameQueue, "", null);

        IBasicProperties properties = model.GetProperties();        
        properties.Headers.Add(ActivityConstants.MessageException, exception.Message);
        properties.Headers.Add(ActivityConstants.MessageExceptionType, exception.GetType().Name);

        await model.PublishConfirmMessage(message, nameQueue, "topic" ,basicProperties: properties);
    }
}
