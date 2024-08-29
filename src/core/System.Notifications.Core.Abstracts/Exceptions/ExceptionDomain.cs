using System.Notifications.Core.Domain.Abstracts.Domain;

namespace System.Notifications.Core.Domain.Abstracts.Exceptions;

public class ExceptionDomain : Exception
{
    public List<NotificationDomain>? Messages { get; }    

    public ExceptionDomain(string message) : base(message) { }

    public ExceptionDomain(string message, Exception innerException) : base(message, innerException) { }

    public ExceptionDomain(List<NotificationDomain> messages) : base() => Messages = messages;
}

