using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonctonEventsNet.Application.Event;

namespace MonctonEventsNet.Infrastructure;

public static class Configuration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEventRepository, EventRepository>();
    }
}