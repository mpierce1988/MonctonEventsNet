using System.ComponentModel.DataAnnotations;

namespace MonctonEventsNet.Model;

public class EventType
{
    [Key]
    public int EventTypeId { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "Name Min length is 1")]
    public string Description { get; set; } = string.Empty;
}