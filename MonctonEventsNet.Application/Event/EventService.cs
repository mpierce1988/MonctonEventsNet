namespace MonctonEventsNet.Application.Event;

public class EventService : IEventService
{
    #region Private Fields
    
    private readonly IEventRepository _repository;
    
    #endregion
    
    #region Constructor
    
    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }
    
    #endregion
    
    #region Public Methods

    public async Task<Result<GetEventsResponse, Error>> GetEventsAsync(GetEventsQuery getEventsQuery)
    {
        try
        {
            List<Model.Event> events = await _repository.GetEventsAsync(getEventsQuery);

            if (!events.Any()) return EventErrors.EventsNotFound();

            return new GetEventsResponse()
            {
                Events = events.Select(ev => new GetEventResponse(ev)).ToList()
            };
        }
        catch (Exception e)
        {
            return new Error("500", e.Message);
        }
    }

    public async Task<Result<GetEventResponse, Error>> GetEventAsync(GetEventQuery getEventQuery)
    {
        try
        {
            Model.Event? ev = await _repository.GetEventAsync(getEventQuery.EventId);

            if (ev is null) return EventErrors.EventNotFound(getEventQuery.EventId);

            return new GetEventResponse(ev);
        }
        catch (Exception e)
        {
            return new Error("500", e.Message);
        }
    }
    
    #endregion
}