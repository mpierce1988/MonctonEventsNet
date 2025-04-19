using System.ComponentModel.DataAnnotations;

namespace MonctonEventsNet.Model;

public class Venue
{
    [Key]
    public Guid VenueId { get; set; }
    
    [Required]
    [MinLength(1, ErrorMessage = "Name Min length is 1")]
    public string Name { get; set; } = string.Empty;
}