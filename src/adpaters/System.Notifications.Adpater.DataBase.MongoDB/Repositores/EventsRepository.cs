using MongoDB.Driver;
using System.Notifications.Adpater.DataBase.MongoDB.Contexts;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Repositories;

namespace System.Notifications.Adpater.DataBase.MongoDB.Repositores;

internal class EventsRepository(MongoContext mongoContext) : IEventsRepository
{
    public async Task<EventsRegistrys?> GetByCodeAsync(string code, CancellationToken cancellation = default)
        => await mongoContext.EventsRegistrys.Find(e => e.Code == code)
            .FirstOrDefaultAsync(cancellationToken: cancellation);

    public async Task<EventsRegistrys?> GetByIdAsync(Guid id, CancellationToken cancellation = default)
        => await mongoContext.EventsRegistrys.Find(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken: cancellation);

    public Task SaveChangeAsync(EventsRegistrys eventsRegistrys, CancellationToken cancellationToken = default)
        => mongoContext.EventsRegistrys.ReplaceOneAsync(
                filter: e => e.Id == eventsRegistrys.Id,
                replacement: eventsRegistrys, 
                options: new ReplaceOptions { IsUpsert = true },
                cancellationToken: cancellationToken);
}
