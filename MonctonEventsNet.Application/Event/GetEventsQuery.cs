namespace MonctonEventsNet.Application.Event;

public class GetEventsQuery
{
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    
    public int? EventTypeId { get; set; }
    
    public Guid? VenueId { get; set; }
    
    public Decimal? MinCost { get; set; }
    
    public Decimal? MaxCost { get; set; }
    
    public string? SearchText { get; set; }
}