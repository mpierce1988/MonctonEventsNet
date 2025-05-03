using Microsoft.AspNetCore.Mvc;
using MonctonEventsNet.Application;
using MonctonEventsNet.Application.Event;
using MonctonEventsNet.Model;

namespace MonctonEventsNet.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    #region Private Fields
    
    private readonly IEventService _eventService;
    
    #endregion
    
    #region Constructor
    
    public EventsController(IEventService eventService)
    {
        _eventService = eventService;
    }
    
    #endregion
    
    #region Event Methods
    
    // GET
    [HttpGet]
    public async Task<IActionResult> GetEvents([FromQuery] GetEventsQuery query)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.GetEventsAsync(query);

            return HandleResult(result);
        }
        catch (Exception e)
        {
            return Problem(
                detail: e.Message,
                title: "An error occurred while getting events",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetEvent(string id)
    {
        try
        {
            if (!Guid.TryParse(id, out Guid eventGuid))
            {
                return BadRequest("Invalid event ID format.");
            }
            
            var result = await _eventService.GetEventAsync(new GetEventQuery(eventGuid));

            return HandleResult(result);
        }
        catch (Exception e)
        {
            return Problem(
                detail: e.Message,
                title: $"An error occurred while getting event by id {id}",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            var result = await _eventService.RefreshEventsAsync();

            return HandleResult(result);
        }
        catch (Exception e)
        {
            return Problem(
                detail: e.Message,
                title: "An error occurred while refreshing events",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }
    
    #endregion
    
    #region Venues and Event Types Methods

    [HttpGet("venues")]
    public async Task<IActionResult> GetVenues()
    {
        try
        {
            var results = await _eventService.GetVenuesAsync();

            return HandleResult(results);
        }
        catch (Exception e)
        {
            return Problem(
                detail: e.Message,
                title: "An error occurred while getting venues",
                statusCode: StatusCodes.Status500InternalServerError
            );
        }
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetEventTypes()
    {
        try
        {
            Result<List<EventType>, Error> results = await _eventService.GetEventTypesAsync();

            return HandleResult(results);
        }
        catch (Exception e)
        {
            return Problem(
                detail: e.Message,
                title: "An error occurred while getting event types",
                statusCode: StatusCodes.Status500InternalServerError
                );
        }
    }
    
    #endregion
    
    #region Private Methods

    private IActionResult HandleResult<T>(Result<T, Error> result)
    {
       
        return result.Match(
            success: arg => Ok(arg),
            failure: failure =>
            {
                int statusCode = int.TryParse(failure.Code, out var code) ? code : 500;
                return Problem(
                    detail: failure.Description,
                    title: failure.GetType().ToString(),
                    statusCode: statusCode
                    );
            });
       
    }
    
    #endregion
}