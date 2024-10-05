using System.Notifications.Core.ServiceDefaults;
using System.Notifications.Servers.API.Configuration;
using System.Notifications.Servers.API.Hubs;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddSwaggerGenDefault();

builder.Services.AddControllers()
    .AddJsonOptions(e => e.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServices(builder.Configuration);
builder.Services.AddMemoryCache();

builder.Services.AddAuthorization();

builder.AddServiceDefaults();

builder.Services.AddCors(options => options.AddPolicy("CORS",
        builder =>
        {
            builder.AllowAnyHeader()
                   .AllowAnyMethod()
                   .WithOrigins("http://localhost:3000")
                   .AllowCredentials();
        }));


var app = builder.Build();

app.UseSwaggerDefault();

app.UseCors("CORS");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<NotificationsHub>("/notifications", configureOptions =>
{
    configureOptions.CloseOnAuthenticationExpiration = true;
    configureOptions.AllowStatefulReconnects = true;    
})
    .RequireAuthorization();

app.MapControllers();

app.Run();
