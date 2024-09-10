using Newtonsoft.Json;
using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Text;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Extesions;


public static class MessageExtesions
{  
    public static byte[] SerializationMessage<IMessage>(this IMessage message) where IMessage: class
    {
        string json = JsonConvert.SerializeObject(message, Formatting.Indented);
        return Encoding.UTF8.GetBytes(json);
    }

    public static IMessage DeserializationMessage<IMessage>(this byte[] message) where IMessage: class
    {
        string json = Encoding.UTF8.GetString(message);

        IMessage? messageObject = JsonConvert.DeserializeObject<IMessage>(json);

        if (messageObject is null) 
            throw new ExceptionDomain($"Não foi possivel transforma message em {nameof(message)}");

        return messageObject;
    }
}

