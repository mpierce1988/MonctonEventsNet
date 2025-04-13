namespace MonctonEventsNet.Application;

public record Error(string Code, string Description)
{
    public static readonly Error None = new(string.Empty, string.Empty);
    public static Error UncaughtError(string error) => new("500", error);
}