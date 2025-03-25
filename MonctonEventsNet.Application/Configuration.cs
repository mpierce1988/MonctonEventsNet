using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonctonEventsNet.Application.Event;

namespace MonctonEventsNet.Application;

public static class Configuration
{
    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEventService, EventService>();
    }
}