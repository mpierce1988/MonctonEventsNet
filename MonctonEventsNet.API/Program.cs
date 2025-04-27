using System.Diagnostics;
using MonctonEventsNet.Application;
using MonctonEventsNet.Application.EpPlus;
using MonctonEventsNet.Application.Event;
using MonctonEventsNet.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddEpPlusServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

if (app.Configuration.GetValue<bool>("RefreshOnStart"))
{
    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;

    IEventService eventService = services.GetRequiredService<IEventService>();

    Stopwatch stopwatch = new Stopwatch();
    stopwatch.Start();

    var result = await eventService.RefreshEventsAsync();

    stopwatch.Stop();

    var (resultEvents, resultError) = result.Match<(RefreshEventsResponse?, Error?)>(
        success: response => (response, null),
        failure: e => (null, e)
    );

    if (result.IsSuccess)
    {
        Console.WriteLine($"Events refreshed successfully in {stopwatch.ElapsedMilliseconds} ms. " +
                          $"Events Downloaded: {resultEvents?.NumDownloaded}, " +
                          $"Created events: {resultEvents?.NumCreated}, " +
                          $"Updated events: {resultEvents?.NumUpdated}, " +
                          $"Deleted events: {resultEvents?.NumDeleted}");
    }
    else
    {
        Console.WriteLine($"Error refreshing events: {resultError?.Description}");
    }
}

app.Run();