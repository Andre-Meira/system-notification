namespace System.Notifications.Core.Domain.Notifications.Services;


public interface ISocketPublishNotification : IPublishNotificationChannel;

public interface IEmailPublishNotification : IPublishNotificationChannel;

public interface ISmsPublishNotification : IPublishNotificationChannel;
