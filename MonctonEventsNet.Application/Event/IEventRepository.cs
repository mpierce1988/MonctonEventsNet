using MonctonEventsNet.Model;

namespace MonctonEventsNet.Application.Event;

public interface IEventRepository
{
    Task<List<Model.Event>> GetEventsAsync(GetEventsQuery getEventsQuery);
    Task<Model.Event?> GetEventAsync(int eventId);
    Task<RefreshEventsResponse> BulkUpsertAsync(List<Model.Event> events);
    Task<EventType> GetOrCreateEventType(string eventTypeString);
    Task<Venue> GetOrCreateVenue(string venueString);
}