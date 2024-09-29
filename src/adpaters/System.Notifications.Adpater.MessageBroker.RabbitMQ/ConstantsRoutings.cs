namespace System.Notifications.Adpater.MessageBroker.RabbitMQ;

internal class ConstantsRoutings
{
    public const string ExchageEvent = "event-dispacher";
    public const string ExchageEventConsumer = "event-dispacher-consumer";

    public const string ExchangePublishNotifications = "publish-notifications";
    public const string ExchageEmailConsumer = "notification-email-consumer";

    public const string ExchangeSaveNotifications = "save-notifications";
    public const string ExchangeSaveNotificationsConsumer = "save-notifications-consumer";
}
