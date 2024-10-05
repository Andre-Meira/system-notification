using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Notifications.Adpater.DataBase.MongoDB.Configurations;
using System.Notifications.Adpater.DataBase.MongoDB.Options;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Configurations;
using System.Notifications.Adpater.OutBound.Email.Configuration;
using System.Notifications.Adpater.OutBound.Sockets.Models;
using System.Notifications.Adpater.OutBound.Sockets.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.ServiceDefaults;

var host = Host.CreateApplicationBuilder(args);

var busOptions = host.Configuration.GetSection("Bus").Get<ConnectionFactory>();
var mongoOptions = host.Configuration.GetSection(MongoOptions.Key).Get<MongoOptions>();

host.Services.AddScoped(_ => new SocketsOptions
{
    Token = "Basic c2lzdGVtYTpjZWE3Y2VhYy1iYWM2LTQ0YjEtOTg4My0wOTBkZDViOTBmZTE=",
    UrlSocket = "http://localhost:5186/notifications"
});
host.Services.AddScoped<ISocketNotification,BaseSocketNotification>();

if (busOptions is null || mongoOptions is null)
{
    throw new ArgumentNullException();
}

host.Services.AddAdpaterMongoDb(mongoOptions);
host.Services.AddAdapterConsumerMessageBrokerRabbiMQ(busOptions);
host.Services.AddEmailAdpater();

host.AddServiceDefaults();

var app = host.Build();
app.Run();