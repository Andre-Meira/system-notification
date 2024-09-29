using Microsoft.Extensions.Logging;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Adpater.OutBound.Email.Notifications;

public class EmailNotificationService : IEmailNotification
{
    private readonly ILogger<EmailNotificationService> _logger;
    private readonly IUserNotificationRepository _userNotification;

    public EmailNotificationService(ILogger<EmailNotificationService> logger, IUserNotificationRepository userNotification)
    {
        _logger = logger;
        _userNotification = userNotification;
    }

    public async Task<NotificationContext[]> PublishAsync(NotificationContext[] notificationContexts,
        CancellationToken cancellationToken = default)
    {
        foreach (var context in notificationContexts)
        {
            try
            {
                UserNotificationsParameters? userNotificationsParameters = await _userNotification.GeyByIdAsync(context.UserNotificationsId);

                if (userNotificationsParameters == null)
                    continue;

                _logger.LogInformation($"Enviando mensagem no email do {userNotificationsParameters.EmailAddress}");
                await Task.Delay(TimeSpan.FromSeconds(2));
                _logger.LogInformation($"Email enviado ao {userNotificationsParameters.EmailAddress}");

                context.ConfirmDelivered();
                context.ConfirmReceipt();
            }
            catch (Exception err)
            {
                context.AddError($"Fail send email error: {err.Message}");
                throw;
            }
        }

        return notificationContexts;
    }
}
