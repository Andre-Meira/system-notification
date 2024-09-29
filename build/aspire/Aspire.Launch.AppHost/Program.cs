var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.System_Notifications_Servers_API>("apiservice");
var woker = builder.AddProject<Projects.System_Notifications_Servers_Woker>("worker");

builder.Build().Run();
