namespace MonctonEventsNet.Application.Event;

public interface IEventRepository
{
    Task<List<Model.Event>> GetEventsAsync(GetEventsQuery getEventsQuery);
    Task<Model.Event?> GetEventAsync(int eventId);
}