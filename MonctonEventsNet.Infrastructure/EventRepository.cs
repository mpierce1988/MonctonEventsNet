using MonctonEventsNet.Application.Event;
using MonctonEventsNet.Model;

namespace MonctonEventsNet.Infrastructure;

public class EventRepository : IEventRepository
{
    public async Task<List<Event>> GetEventsAsync(GetEventsQuery getEventsQuery)
    {
        throw new NotImplementedException();
    }

    public async Task<Event?> GetEventAsync(int eventId)
    {
        throw new NotImplementedException();
    }
}