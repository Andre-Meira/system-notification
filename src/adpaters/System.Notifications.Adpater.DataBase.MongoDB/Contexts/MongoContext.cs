using MongoDB.Driver;
using System.Notifications.Adpater.DataBase.MongoDB.Options;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Users;

namespace System.Notifications.Adpater.DataBase.MongoDB.Contexts;

internal sealed class MongoContext
{
    private readonly IMongoDatabase _database;

    public IMongoCollection<UserNotificationsParameters> UserNotificationParameter
        => _database.GetCollection<UserNotificationsParameters>("user-notification-parameter");

    public IMongoCollection<OutboundNotifications> OutboundNotifications =>
        _database.GetCollection<OutboundNotifications>("outbound-notifications");

    public IMongoCollection<NotificationContext> NotificationContext =>
        _database.GetCollection<NotificationContext>("notifications");

    public IMongoCollection<EventsRegistrys> EventsRegistrys =>
        _database.GetCollection<EventsRegistrys>("events-registrys");


    public MongoContext(MongoOptions options)
    {
        string connection = options.Connection;
        string dataBaseName = options.DatabaseName;

        var client = new MongoClient(connection);
        _database = client.GetDatabase(dataBaseName);
    }
}


