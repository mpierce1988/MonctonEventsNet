namespace MonctonEventsNet.Application.Event;

public interface IEventService
{
    Task<Result<GetEventsResponse, Error>> GetEventsAsync(GetEventsQuery getEventsQuery);
    Task<Result<GetEventResponse, Error>> GetEventAsync(GetEventQuery getEventQuery);

    Task<Result<RefreshEventsResponse, Error>> RefreshEventsAsync();
}