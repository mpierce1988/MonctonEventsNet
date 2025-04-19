using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonctonEventsNet.Application.Event;

namespace MonctonEventsNet.Infrastructure;

public static class Configuration
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEventRepository, EventRepository>();

        services.AddDbContext<EventContext>(optionsBuilder =>
        {
            string dataSource = configuration.GetValue<string>("ConnectionStrings:DefaultConnection") ?? throw new Exception("Connection string is missing");
            optionsBuilder.UseSqlite(dataSource);

        });
    }
}