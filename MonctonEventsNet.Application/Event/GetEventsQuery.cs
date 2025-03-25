namespace MonctonEventsNet.Application.Event;

public class GetEventsQuery
{
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
}