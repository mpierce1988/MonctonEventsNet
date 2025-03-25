namespace MonctonEventsNet.Application;

public static class EventErrors
{
    public static Error EventNotFound(int eventId) => new Error("404", $"Event with ID {eventId} Not Found");
    public static Error EventsNotFound() => new Error("403", "No Events Founds");

}