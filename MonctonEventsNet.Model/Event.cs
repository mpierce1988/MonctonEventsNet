namespace MonctonEventsNet.Model;

public class Event
{
    public int EventId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
}