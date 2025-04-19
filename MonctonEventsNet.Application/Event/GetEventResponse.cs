namespace MonctonEventsNet.Application.Event;

public class GetEventResponse
{
    public Guid EventId { get; set; }
    public string? Information { get; set; }
    public DateTime? DateTime { get; set; }
    
    public string VenueName { get; set; } = string.Empty;
    
    public string EventTypeDescription { get; set; } = string.Empty;
    
    public Decimal MinCost { get; set; }
    
    public Decimal? MaxCost { get; set; }
    
    public string? CostInformation { get; set; }

    public GetEventResponse()
    {
    }
    
    public GetEventResponse(Model.Event eventModel)
    {
        EventId = eventModel.EventId;
        Information = eventModel.Information;
        DateTime = eventModel.DateTime;
        VenueName = eventModel.Venue?.Name ?? string.Empty;
        EventTypeDescription = eventModel.EventType?.Description ?? string.Empty;
        MinCost = eventModel.Cost?.MinCost ?? 0;
        MaxCost = eventModel.Cost?.MaxCost;
        CostInformation = eventModel.Cost?.Information;
    }
}