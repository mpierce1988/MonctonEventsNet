using MonctonEventsNet.Model;

namespace MonctonEventsNet.Application.Event;

public interface IEventRepository
{
    Task<List<Model.Event>> GetEventsAsync(GetEventsQuery getEventsQuery);
    Task<Model.Event?> GetEventAsync(Guid eventId);
    Task<RefreshEventsResponse> BulkUpsertAsync(List<Model.Event> events);
    Task<EventType> GetOrCreateEventType(string eventTypeString);
    Task<Venue> GetOrCreateVenue(string venueString);
    Task<List<Venue>> GetVenuesAsync();
    Task<List<EventType>> GetEventTypesAsync();
}