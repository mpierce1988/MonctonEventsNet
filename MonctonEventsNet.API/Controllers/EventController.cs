using Microsoft.AspNetCore.Mvc;
using MonctonEventsNet.Application;
using MonctonEventsNet.Application.Event;
using MonctonEventsNet.Model;

namespace MonctonEventsNet.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class EventController : ControllerBase
{
    #region Private Fields
    
    private readonly IEventService _eventService;
    
    #endregion
    
    #region Constructor
    
    public EventController(IEventService eventService)
    {
        _eventService = eventService;
    }
    
    #endregion
    
    #region Event Methods
    
    // GET
    [HttpGet]
    public async Task<IActionResult> Index([FromQuery] GetEventsQuery query)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.GetEventsAsync(query);

            return result.Match(
                success: Ok,
                failure: failure => StatusCode(int.TryParse(failure.Code, out var code) ? code : 500, failure.Description)
                );
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("{eventId}")]
    public async Task<IActionResult> GetEvent(string eventId)
    {
        try
        {
            if (!Guid.TryParse(eventId, out Guid eventGuid))
            {
                return BadRequest("Invalid event ID format.");
            }
            
            var result = await _eventService.GetEventAsync(new GetEventQuery(eventGuid));

            return result.Match(
                success: Ok,
                failure: failure => StatusCode(int.TryParse(failure.Code, out var code) ? code : 500, failure.Description)
                );
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("refresh")]
    public async Task<IActionResult> Refresh()
    {
        try
        {
            var result = await _eventService.RefreshEventsAsync();
            
            return result.Match(
                success: Ok,
                failure: failure => StatusCode(int.TryParse(failure.Code, out var code) ? code : 500, failure.Description)
                );
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
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

            return results.Match(
                success: Ok,
                failure: failure => StatusCode(int.TryParse(failure.Code, out var code) ? code : 500, failure.Description)
                );
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }

    [HttpGet("event-types")]
    public async Task<IActionResult> GetEventTypes()
    {
        try
        {
            Result<List<EventType>, Error> results = await _eventService.GetEventTypesAsync();
            
            return results.Match(
                success: Ok,
                failure: failure => StatusCode(int.TryParse(failure.Code, out var code) ? code : 500, failure.Description)
                );
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
        }
    }
    
    #endregion
}