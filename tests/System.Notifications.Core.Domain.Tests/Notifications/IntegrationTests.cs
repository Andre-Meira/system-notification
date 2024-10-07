using Microsoft.Extensions.DependencyInjection;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Core.Domain.Tests.Notifications;

public class IntegrationTests
{
    public IntegrationTests()
    {
        var serviceCollection = new ServiceCollection();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        PublishNotification = serviceProvider.GetRequiredService<IPublishNotificationChannel>();
    }

    public IPublishNotificationChannel PublishNotification { get; private set; }
}
