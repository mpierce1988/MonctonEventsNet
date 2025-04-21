using MonctonEventsNet.Model;

namespace MonctonEventsNet.Application.Event;

public interface IEventService
{
    Task<Result<GetEventsResponse, Error>> GetEventsAsync(GetEventsQuery getEventsQuery);
    Task<Result<GetEventResponse, Error>> GetEventAsync(GetEventQuery getEventQuery);

    Task<Result<RefreshEventsResponse, Error>> RefreshEventsAsync();
    Task<Result<List<Venue>, Error>> GetVenuesAsync();
    Task<Result<List<EventType>, Error>> GetEventTypesAsync();
}