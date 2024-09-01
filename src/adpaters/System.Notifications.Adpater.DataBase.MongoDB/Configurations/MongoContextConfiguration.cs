﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;


namespace System.Notifications.Adpater.DataBase.MongoDB.Configurations;
internal sealed class MongoContextConfiguration
{
    public static void RegisterClassMap()
    {
        BsonClassMap.TryRegisterClassMap<UserNotificationsParameters>();
        BsonClassMap.TryRegisterClassMap<OutboundNotifications>();
        BsonClassMap.TryRegisterClassMap<NotificationContext>();
        BsonClassMap.TryRegisterClassMap<EventsRegistrys>();
        BsonClassMap.TryRegisterClassMap<NotificationMessage>();
    }

    public static void RegisterSerializer()
    {
        #pragma warning disable CS0618
        BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
        BsonDefaults.GuidRepresentationMode = GuidRepresentationMode.V3;
        #pragma warning restore CS06181

        BsonSerializer.TryRegisterSerializer(new ObjectSerializer(x => true));
        BsonSerializer.TryRegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        BsonSerializer.TryRegisterSerializer(typeof(DateTime), new DateTimeSerializer(DateTimeKind.Local));
    }
} 