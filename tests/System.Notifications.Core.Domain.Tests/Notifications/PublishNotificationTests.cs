using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Core.Domain.Tests.Notifications;

public class PublishNotificationTests : IClassFixture<IntegrationTests>
{
    private readonly IPublishNotification _publishNotification;

    public PublishNotificationTests(IntegrationTests channels)
    {
        _publishNotification = channels.PublishNotification;
    }

    [Fact]
    public async Task teste()
    {
        var notification = SamplesNotifications.CreateNotificationSMS();
        await _publishNotification.PublishAsync(new List<NotificationContext> { notification });
    }
}