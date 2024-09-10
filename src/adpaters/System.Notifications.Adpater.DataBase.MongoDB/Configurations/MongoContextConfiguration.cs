using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Abstracts.Domain;


namespace System.Notifications.Adpater.DataBase.MongoDB.Configurations;
internal sealed class MongoContextConfiguration
{
    public static void RegisterClassMap()
    {
        BsonClassMap.TryRegisterClassMap<UserNotificationSettings>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapCreator(e =>
                new UserNotificationSettings(e.EventId, e.OutboundNotificationId, e.EventCode, e.OutboundNotificationCode)
            );

        });

        BsonClassMap.TryRegisterClassMap<UserNotificationsParameters>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);

            classMap.MapField("_notificationSettings")
                    .SetElementName(nameof(UserNotificationsParameters.NotificationSettings));
        });

        BsonClassMap.TryRegisterClassMap<NotificationContext>(classMap =>
        {
            classMap.AutoMap();
            classMap.SetIgnoreExtraElements(true);

            classMap.MapField("_error")
                    .SetElementName(nameof(NotificationContext.Error));
        });

        BsonClassMap.TryRegisterClassMap<Entity>(classMap =>
        {
            classMap.AutoMap();

            classMap.MapField("_id")
                    .SetElementName(nameof(Entity.Id));

            classMap.MapIdProperty(e => e.Id);
        });

        
        BsonClassMap.TryRegisterClassMap<OutboundNotifications>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapCreator(e =>
                new OutboundNotifications(e.Id, e.Code, e.Name, e.Description)
            );
        });

        BsonClassMap.TryRegisterClassMap<EventsRegistrys>(classMap =>
        {
            classMap.AutoMap();
            classMap.MapCreator(e =>
                new EventsRegistrys(e.Id, e.Code, e.Name, e.Description)
            );
        });

        BsonClassMap.TryRegisterClassMap<NotificationMessage>();
    }

    public static void RegisterSerializer()
    {
        #pragma warning disable CS0618
        if (BsonDefaults.GuidRepresentationMode == GuidRepresentationMode.V2)
            BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        #pragma warning restore CS06181

        BsonSerializer.TryRegisterSerializer(new ObjectSerializer(x => true));
        BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.TryRegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));
    }
} 