namespace System.Notifications.Core.Domain.Abstracts.Domain;

public interface IUnitOfWork
{
    Task CommitTransaction(CancellationToken cancellationToken = default);
    Task RollbackTransaction(CancellationToken cancellationToken = default);
}