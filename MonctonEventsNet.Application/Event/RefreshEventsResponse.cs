namespace MonctonEventsNet.Application.Event;

public class RefreshEventsResponse
{
    public int NumDownloaded { get; set; }
    public int NumUpdated { get; set; }
    public int NumDeleted { get; set; }
    public int NumCreated { get; set; }
}