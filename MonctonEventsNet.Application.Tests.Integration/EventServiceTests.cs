using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MonctonEventsNet.Application.Event;
using MonctonEventsNet.Application.FileProvider;
using MonctonEventsNet.Infrastructure;
using MonctonEventsNet.Model;

namespace MonctonEventsNet.Application.Tests.Integration;

public class EventServiceTests
{
    #region Private Fields

    private readonly IEventService _service;
    private readonly EventContext _context;
    private Venue _testVenueOne;
    private Model.Event _eventOne;
    
    #endregion
    
    #region Constructor

    public EventServiceTests()
    {
        var options = new DbContextOptionsBuilder<EventContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        _context = new EventContext(options);
        IEventRepository eventRepository = new EventRepository(_context);
        IFileProvider fileProvider = new LocalFileProvider(configuration);

        _service = new EventService(eventRepository, fileProvider, configuration);

        _testVenueOne = new()
        {
            VenueId = Guid.NewGuid(),
            Name = "Test Venue One"
        };
    }
    
    #endregion
    
    #region GetEventsAsync Tests
    
    [Fact]
    public async Task GetEventsAsync_EmptyQuery_ReturnsAllResults()
    {
        // Arrange
        await SeedDatabase();
        int expectedCount = 2;
        
        // Act
        var result = await _service.GetEventsAsync(new GetEventsQuery());
        GetEventsResponse? resultEvents = result.Match<GetEventsResponse?>(success => success, failure => null);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(resultEvents);
        Assert.Equal(expectedCount, resultEvents.Events.Count);
    }

    [Fact]
    public async Task GetEventsAsync_MinDate_ReturnsFilteredResults()
    {
        // Arrange
        await SeedDatabase();
        DateTime minDate = new DateTime(2020, 01, 15);
        int expectedCount = 1;
        GetEventsQuery getEventsQuery = new()
        {
            MinDate = minDate
        };
        
        // Act
        var result = await _service.GetEventsAsync(getEventsQuery);
        GetEventsResponse? resultEvents = result.Match<GetEventsResponse?>(success => success, failure => null);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(resultEvents);
        Assert.Equal(expectedCount, resultEvents.Events.Count);
    }
    
    [Fact]
    public async Task GetEventsAsync_MaxDate_ReturnsFilteredResults()
    {
        // Arrange
        await SeedDatabase();
        DateTime maxDate = new DateTime(2020, 01, 15);
        int expectedCount = 1;
        GetEventsQuery getEventsQuery = new()
        {
            MaxDate = maxDate
        };
        
        // Act
        var result = await _service.GetEventsAsync(getEventsQuery);
        GetEventsResponse? resultEvents = result.Match<GetEventsResponse?>(success => success, failure => null);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(resultEvents);
        Assert.Equal(expectedCount, resultEvents.Events.Count);
    }
    
    [Fact]
    public async Task GetEventsAsync_EventType_ReturnsFilteredResults()
    {
        // Arrange
        await SeedDatabase();
        int eventTypeId = 1;
        int expectedCount = 1;
        GetEventsQuery getEventsQuery = new()
        {
            EventTypeId = eventTypeId
        };
        
        // Act
        var result = await _service.GetEventsAsync(getEventsQuery);
        GetEventsResponse? resultEvents = result.Match<GetEventsResponse?>(success => success, failure => null);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(resultEvents);
        Assert.Equal(expectedCount, resultEvents.Events.Count);
    }
    
    [Fact]
    public async Task GetEventsAsync_Venue_ReturnsFilteredResults()
    {
        // Arrange
        await SeedDatabase();
        Guid venueId = _testVenueOne.VenueId;
        int expectedCount = 1;
        GetEventsQuery getEventsQuery = new()
        {
            VenueId = venueId
        };
        
        // Act
        var result = await _service.GetEventsAsync(getEventsQuery);
        GetEventsResponse? resultEvents = result.Match<GetEventsResponse?>(success => success, failure => null);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(resultEvents);
        Assert.Equal(expectedCount, resultEvents.Events.Count);
    }
    
    [Fact]
    public async Task GetEventsAsync_MinCost_ReturnsFilteredResults()
    {
        // Arrange
        await SeedDatabase();
        decimal minCost = 10.0m;
        int expectedCount = 1;
        GetEventsQuery getEventsQuery = new()
        {
            MinCost = minCost
        };
        
        // Act
        var result = await _service.GetEventsAsync(getEventsQuery);
        GetEventsResponse? resultEvents = result.Match<GetEventsResponse?>(success => success, failure => null);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(resultEvents);
        Assert.Equal(expectedCount, resultEvents.Events.Count);
    }
    
    [Fact]
    public async Task GetEventsAsync_MaxCost_ReturnsFilteredResults()
    {
        // Arrange
        await SeedDatabase();
        decimal minCost = 15.0m;
        int expectedCount = 1;
        GetEventsQuery getEventsQuery = new()
        {
            MaxCost = minCost
        };
        
        // Act
        var result = await _service.GetEventsAsync(getEventsQuery);
        GetEventsResponse? resultEvents = result.Match<GetEventsResponse?>(success => success, failure => null);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(resultEvents);
        Assert.Equal(expectedCount, resultEvents.Events.Count);
    }
    
    #endregion
    
    #region GetEventAsync Tests

    [Fact]
    public async Task GetEventAsync_ValidId_ReturnsEvent()
    {
        // Arrange
        await SeedDatabase();
        Guid eventId = _eventOne.EventId;
        GetEventQuery getEventQuery = new()
        {
            EventId = eventId
        };
        
        // Act
        var result = await _service.GetEventAsync(getEventQuery);
        GetEventResponse? resultEvent = result.Match<GetEventResponse?>(success => success, failure => null);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(resultEvent);
        Assert.Equal(eventId, resultEvent.EventId);
    }
    
    [Fact]
    public async Task GetEventAsync_InvalidId_ReturnsError()
    {
        // Arrange
        await SeedDatabase();
        Guid eventId = Guid.NewGuid();
        GetEventQuery getEventQuery = new()
        {
            EventId = eventId
        };
        
        // Act
        var result = await _service.GetEventAsync(getEventQuery);
        Error? resultError = result.Match<Error?>(success => null, error => error);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotNull(resultError);
        Assert.Equal(EventErrors.EventNotFound(eventId).Description, resultError.Description);
    }
    
    #endregion
    
    #region Private Methods

    private async Task SeedDatabase()
    {
        EventType eventTypeOne = new()
        {
            EventTypeId = 1,
            Description = "Test Event Type"
        };

        EventType eventTypeTwo = new()
        {
            EventTypeId = 2,
            Description = "Test Event Type Two"
        };

        _context.EventTypes.Add(eventTypeOne);
        _context.EventTypes.Add(eventTypeTwo);

        Venue venueOne = _testVenueOne;
        
        Venue venueTwo = new()
        {
            VenueId = Guid.NewGuid(),
            Name = "Venue Two"
        };
        
        _context.Venues.Add(venueOne);
        _context.Venues.Add(venueTwo);
        
        Cost costOne = new()
        {
            CostId = 1,
            MinCost = 10,
            MaxCost = 20,
            Information = "Cost One Information"
        };
        
        Cost costTwo = new()
        {
            CostId = 2,
            MinCost = 5,
            MaxCost = 15,
            Information = "Cost Two Information"
        };

        _eventOne = new()
        {
            DateTime = new DateTime(2020, 01, 01),
            EventType = eventTypeOne,
            EventTypeId = eventTypeOne.EventTypeId,
            Information = "Event One",
            Venue = venueOne,
            VenueId = venueOne.VenueId,
            Cost = costOne,
            CostId = costOne.CostId,
            LinkText = "Event One Link",
            Url = "http://example.com/event-one"
        };
        
        Model.Event eventTwo = new()
        {
            DateTime = new DateTime(2020, 02, 01),
            EventType = eventTypeTwo,
            EventTypeId = eventTypeTwo.EventTypeId,
            Information = "Event Two",
            Venue = venueTwo,
            VenueId = venueTwo.VenueId,
            Cost = costTwo,
            CostId = costTwo.CostId,
            LinkText = "Event Two Link",
            Url = "http://example.com/event-two"
        };
        
        _context.Events.Add(_eventOne);
        _context.Events.Add(eventTwo);
        
        await _context.SaveChangesAsync();
    }
    
    #endregion
}