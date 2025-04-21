using System.ComponentModel.DataAnnotations;

namespace MonctonEventsNet.Application.Event;

public class GetEventQuery
{
    [Required]
    public Guid EventId { get; set; }
    
    public GetEventQuery() {}
    
    public GetEventQuery(Guid eventId)
    {
        EventId = eventId;
    }
}