using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MonctonEventsNet.Application.Excel;

namespace MonctonEventsNet.Application.EpPlus;

public static class Configuration
{
    public static void AddEpPlusServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ISpreadsheetReaderService, EpPlusSpreadsheetReaderService>();
    }
}