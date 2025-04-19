using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonctonEventsNet.Model;

public class Event
{
    [Key]
    public Guid EventId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Information Min length is 1")]
    public string Information { get; set; } = string.Empty;
    
    [Required]
    public DateTime DateTime { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "LinkText Min length is 1")]
    public string LinkText { get; set; } = string.Empty;

    [Required]
    [MinLength(1, ErrorMessage = "Url Min length is 1")]
    public string Url { get; set; } = string.Empty;

    [ForeignKey("EventType")]
    public int EventTypeId { get; set; }
    
    public virtual EventType? EventType { get; set; }

    [ForeignKey("Venue")]
    public Guid VenueId { get; set; }
    
    public virtual Venue? Venue { get; set; }
    
    [ForeignKey("Cost")]
    public int CostId { get; set; }
    
    public virtual Cost? Cost { get; set; }
}