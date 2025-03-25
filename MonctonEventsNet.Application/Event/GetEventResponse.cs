namespace MonctonEventsNet.Application.Event;

public class GetEventResponse
{
    public int EventId { get; set; }
    public string? Name { get; set; }
    public DateTime? DateTime { get; set; }

    public GetEventResponse()
    {
    }
    
    public GetEventResponse(Model.Event eventModel)
    {
        EventId = eventModel.EventId;
        Name = eventModel.Name;
        DateTime = eventModel.DateTime;
    }
}