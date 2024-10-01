using System.Notifications.Core.ServiceDefaults;
using System.Notifications.Servers.API.Configuration;
using System.Notifications.Servers.API.Hubs;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(e => e.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddMemoryCache();

builder.Services.AddAuthorization();

builder.AddServiceDefaults();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationsHub>("/notifications", confg =>
{
    confg.CloseOnAuthenticationExpiration = true;
})
    .RequireAuthorization();

app.Run();
