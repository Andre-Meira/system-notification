using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using System.Notifications.Adpater.DataBase.MongoDB.Configurations;
using System.Notifications.Adpater.DataBase.MongoDB.Options;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Configurations;
using System.Notifications.Adpater.OutBound.Email.Configuration;
using System.Notifications.Core.ServiceDefaults;

var host = Host.CreateApplicationBuilder(args);

var busOptions = host.Configuration.GetSection("Bus").Get<ConnectionFactory>();
var mongoOptions = host.Configuration.GetSection(MongoOptions.Key).Get<MongoOptions>();

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