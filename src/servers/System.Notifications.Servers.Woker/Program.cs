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
    Token = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InNpc3RlbWEiLCJuYW1laWQiOiJhM2RjMDRkOC0wZWRkLTQyNzgtOWEyMS04NGNmMDlmZjgxZjIiLCJuYmYiOjE3MjgxODE0MzgsImV4cCI6MTcyODE5NTgzOCwiaWF0IjoxNzI4MTgxNDM4LCJpc3MiOiJub3RpZmljYXRpb24tYXBpIiwiYXVkIjoibm90aWZpY2F0aW9ucyJ9.b9xD0YL30g-saJT8RVB3qM9XYmgaqgwul37LucqIb94",
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