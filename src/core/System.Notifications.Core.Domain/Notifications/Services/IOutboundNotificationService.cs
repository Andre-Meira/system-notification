using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Notifications.Core.Domain.Notifications.Repositories;

namespace System.Notifications.Core.Domain.Notifications.Services;

public interface IOutboundNotificationService
{
    Task<Guid> CreateAsync(OutboundNotificationModel outboundModel, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid id, OutboundNotificationModel outboundModel, CancellationToken cancellationToken = default);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}


public sealed class OutboundNotificationService(IOutboundNotificationRepository repository) : IOutboundNotificationService
{
    public async Task<Guid> CreateAsync(OutboundNotificationModel outboundModel, 
        CancellationToken cancellationToken = default)
    {
        Guid id = Guid.NewGuid();

        OutboundNotifications? outbound = await repository.GetByCodeAsync(outboundModel.Code, cancellationToken);

        if (outbound is not null)
            throw new ExceptionDomain("O serviço de notificação já existe.");

        OutboundNotifications outboundNotification = new OutboundNotifications(id, 
            outboundModel.Code, 
            outboundModel.Name,
            outboundModel.Description);

        await repository.SaveChangeAsync(outboundNotification, cancellationToken);
        return id;
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        OutboundNotifications? outbound = await repository.GetByIdAsync(id, cancellationToken);

        if (outbound is null)
            throw new ExceptionDomain("Serviço de notificação não encontrado");

        outbound.Disable();
        await repository.SaveChangeAsync(outbound, cancellationToken);
    }


    public async Task UpdateAsync(Guid id, OutboundNotificationModel outboundModel, 
        CancellationToken cancellationToken = default)
    {
        OutboundNotifications? outbound = await repository.GetByIdAsync(id, cancellationToken);

        if (outbound is null)
            throw new ExceptionDomain("Serviço de notificação não encontrado");

        outbound.Update(outboundModel.Name, outboundModel.Description);
        await repository.SaveChangeAsync(outbound, cancellationToken);
    }
}