namespace MonctonEventsNet.Application;

public static class EventErrors
{
    public static Error EventNotFound(Guid eventId) => new Error("404", $"Event with ID {eventId} Not Found");
    public static Error EventsNotFound() => new Error("403", "No Events Founds");
    
    public static Error EventUrlNotConfigured() => new Error("400", "Event URL Not Configured");

}