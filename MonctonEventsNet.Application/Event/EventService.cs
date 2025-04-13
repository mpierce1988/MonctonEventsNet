using Microsoft.Extensions.Configuration;

namespace MonctonEventsNet.Application.Event;

public class EventService : IEventService
{
    #region Private Fields
    
    private readonly IEventRepository _repository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string? _eventsGoogleFormsUrl;
    
    #endregion
    
    #region Constructor
    
    public EventService(IEventRepository repository, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _repository = repository;
        _httpClientFactory = httpClientFactory;
        _eventsGoogleFormsUrl = configuration["EventsGoogleFormsUrl"];
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
            return Error.UncaughtError(e.Message);
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
            return Error.UncaughtError(e.Message);
        }
    }

    public async Task<Result<RefreshEventsResponse, Error>> RefreshEventsAsync()
    {
        try
        {
            // Download file from web
            HttpClient client = _httpClientFactory.CreateClient();

            if (string.IsNullOrEmpty(_eventsGoogleFormsUrl))
                return EventErrors.EventUrlNotConfigured();

            HttpResponseMessage response = await client.GetAsync(_eventsGoogleFormsUrl);

            response.EnsureSuccessStatusCode();

            // Parse file using Excel Service

            // Save to database using repository

            // Populate results 
            throw new NotImplementedException();
        }
        catch (HttpRequestException e)
        {
            return Error.UncaughtError(string.Concat("Error downloading file at URL: ", _eventsGoogleFormsUrl, " with HTTP error message: ", e.Message));
        }
        catch (Exception e)
        {
            return Error.UncaughtError(e.Message);
        }
    }

    #endregion
}