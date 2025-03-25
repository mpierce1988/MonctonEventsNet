namespace MonctonEventsNet.Application.Event;

public class GetEventsResponse
{
    public List<GetEventResponse> Events { get; set; } = new();
}