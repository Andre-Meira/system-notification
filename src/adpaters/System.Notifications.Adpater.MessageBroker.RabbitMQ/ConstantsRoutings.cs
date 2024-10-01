namespace System.Notifications.Adpater.MessageBroker.RabbitMQ;

public class ConstantsRoutings
{
    public const string ExchageEvent = "event-dispacher";
    public const string ExchageEventConsumer = "event-dispacher-consumer";

    public const string ExchangePublishNotifications = "publish-notifications";
    public const string ExchageEmailConsumer = "notification-email-consumer";
    public const string ExchageSocketConsumer = "notification-socket-consumer";

    public const string ExchangeSaveNotifications = "save-notifications";
    public const string ExchangeSaveNotificationsConsumer = "save-notifications-consumer";

    public const string ExchangeAckNotifications = "confirm-ack-notifications";
    public const string ExchangeAckNotificationsConsumer = "confirm-ack-notifications-consumer";

    public const string ExchangeDeliveryNotifications = "confirm-delivery-notifications";
    public const string ExchangeDeliveryNotificationsConsumer = "confirm-delivery-notifications-consumer";
}
