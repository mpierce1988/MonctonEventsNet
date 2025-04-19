using Microsoft.EntityFrameworkCore;
using MonctonEventsNet.Application.Event;
using MonctonEventsNet.Model;

namespace MonctonEventsNet.Infrastructure;

public class EventRepository : IEventRepository
{
    #region Private Fields

    private readonly EventContext _context;
    
    #endregion
    
    #region Constructor

    public EventRepository(EventContext context)
    {
        _context = context;
    }
    
    #endregion
    
    #region Event Public Methods
    
    public async Task<List<Event>> GetEventsAsync(GetEventsQuery getEventsQuery)
    {
        IQueryable<Event> query = _context.Events;

        if (getEventsQuery.MinDate.HasValue)
        {
            query = query.Where(ev => ev.DateTime >= getEventsQuery.MinDate);
        }

        if (getEventsQuery.MaxDate.HasValue)
        {
            query = query.Where(ev => ev.DateTime <= getEventsQuery.MaxDate);
        }

        return await query.ToListAsync();
    }
    
    public async Task<Event?> GetEventAsync(int eventId)
    {
        return await _context.Events.FindAsync(eventId);
    }
    
    public async Task<RefreshEventsResponse> BulkUpsertAsync(List<Event> events)
    {
        List<Event> eventsToCreate = new();
        List<Event> eventsToUpdate = new();

        var existingEvents = await _context.Events
            .Include(e => e.EventType)
            .Include(e => e.Venue)
            .Include(e => e.Cost)
            .ToListAsync();

        foreach (Event ev in events)
        {
            // Match on DateTime, VenueId, and EventTypeId
            Event? existingEvent = existingEvents.FirstOrDefault(ee =>
                ee.DateTime == ev.DateTime && ee.VenueId == ev.VenueId && ee.EventTypeId == ev.EventTypeId);

            if (existingEvent is null)
            {
                eventsToCreate.Add(ev);
                continue;
            }
           
            // Update existing event
            existingEvent.Information = ev.Information;
            existingEvent.LinkText = ev.LinkText;
            existingEvent.Url = ev.Url;
            
            // Update Cost information
            if (existingEvent.Cost is not null && ev.Cost is not null)
            {
                existingEvent.Cost.MinCost = ev.Cost.MinCost;
                existingEvent.Cost.MaxCost = ev.Cost.MaxCost;
                existingEvent.Cost.Information = ev.Cost.Information;
            }
            
            eventsToUpdate.Add(existingEvent);
        }
        
        if (eventsToCreate.Count > 0)
        {
            await _context.Events.AddRangeAsync(eventsToCreate);
        }
        
        if (eventsToUpdate.Count > 0)
        {
            _context.Events.UpdateRange(eventsToUpdate);
        }
        
        await _context.SaveChangesAsync();

        return new RefreshEventsResponse()
        {
            NumDownloaded = events.Count(),
            NumCreated = eventsToCreate.Count(),
            NumUpdated = eventsToUpdate.Count()
        };
    }
    
    #endregion
    
    #region Event Type Public Methods

    public async Task<EventType> GetOrCreateEventType(string eventTypeString)
    {
        EventType? eventType = await _context.EventTypes.Where(et => et.Description == eventTypeString).FirstOrDefaultAsync();
        
        if(eventType is null) {
            eventType = new EventType()
            {
                Description = eventTypeString
            };
            _context.EventTypes.Add(eventType);
            await _context.SaveChangesAsync();
        }

        return eventType;
    }
    
    #endregion
    
    #region Venue Public Methods

    public async Task<Venue> GetOrCreateVenue(string venueString)
    {
        Venue? venue = await _context.Venues.FirstOrDefaultAsync(ven => ven.Name == venueString);

        if (venue is null)
        {
            venue = new Venue()
            {
                Name = venueString
            };

            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
        }

        return venue;
    }
    
    #endregion
}