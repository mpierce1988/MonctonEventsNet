using System.Globalization;
using Microsoft.Extensions.Configuration;
using MonctonEventsNet.Application.Excel;
using MonctonEventsNet.Application.FileProvider;
using MonctonEventsNet.Application.Utilities;
using MonctonEventsNet.Model;

namespace MonctonEventsNet.Application.Event;

public class EventService : IEventService
{
    #region Private Fields
    
    private readonly IEventRepository _repository;
    private readonly IFileProvider _fileProvider;
    private readonly ISpreadsheetReaderService _spreadsheetReaderService;
    private readonly string? _eventsGoogleFormsUrl;
    
    #endregion
    
    #region Constructor
    
    public EventService(IEventRepository repository, IFileProvider fileProvider, ISpreadsheetReaderService spreadsheetReaderService, 
        IConfiguration configuration)
    {
        _repository = repository;
        _fileProvider = fileProvider;
        _eventsGoogleFormsUrl = configuration["EventsGoogleFormsUrl"];
        _spreadsheetReaderService = spreadsheetReaderService;
    }
    
    #endregion
    
    #region Public Methods

    public async Task<Result<GetEventsResponse, Error>> GetEventsAsync(GetEventsQuery getEventsQuery)
    {
        try
        {
            List<Model.Event> events = await _repository.GetEventsAsync(getEventsQuery);

            if (!events.Any())
                return new GetEventsResponse();

            return new GetEventsResponse()
            {
                Events = events.Select(ev => new GetEventResponse(ev)).ToList()
            };
        }
        catch (Exception e)
        {
            return Error.UncaughtError(e.Message);
        }
    }

    public async Task<Result<GetEventResponse, Error>> GetEventAsync(GetEventQuery getEventQuery)
    {
        try
        {
            Model.Event? ev = await _repository.GetEventAsync(getEventQuery.EventId);

            if (ev is null) return EventErrors.EventNotFound(getEventQuery.EventId);

            return new GetEventResponse(ev);
        }
        catch (Exception e)
        {
            return Error.UncaughtError(e.Message);
        }
    }

    public async Task<Result<RefreshEventsResponse, Error>> RefreshEventsAsync()
    {
        try
        {
            // Get the file
            var fileResult = await _fileProvider.GetEventsExcelFileAsync();

            List<Model.Event> events = await ParseFileResult(fileResult);

            return await _repository.BulkUpsertAsync(events);
        }
        catch (Exception e)
        {
            return Error.UncaughtError(e.Message);
        }
    }

    public async Task<Result<List<Venue>, Error>> GetVenuesAsync()
    {
        try
        {
            List<Venue> venues = await _repository.GetVenuesAsync();

            return venues;
        }
        catch (Exception e)
        {
            return Error.UncaughtError(e.Message);
        }
    }

    public async Task<Result<List<EventType>, Error>> GetEventTypesAsync()
    {
        try
        {
            List<EventType> eventTypes = await _repository.GetEventTypesAsync();

            return eventTypes;
        }
        catch (Exception e)
        {
            return Error.UncaughtError(e.Message);
        }
    }

    #endregion
    
    #region Private Methods
    
    private async Task<List<Model.Event>> ParseFileResult(Stream result)
    {
        IWorkbook workbook = await _spreadsheetReaderService.ReadAsync(result);
        
        // Ensure there is at least one worksheet
        if (!workbook.Worksheets.Any())
        {
            throw new ArgumentException("No worksheet found");
        }
        
        List<Model.Event> events = new List<Model.Event>();
        
        // Loop through each worksheet
        foreach (IWorksheet sheet in workbook.Worksheets)
        {
            // Parse the worksheet name into a date
            DateTime date;
            //if (!DateTime.TryParse(sheet.Name, out date))
            //    throw new ArgumentException($"Invalid worksheet name: {sheet.Name}");
            
            // Loop through each row in the worksheet, skipping the header row
            for (int rowIndex = 2; rowIndex <= sheet.RowCount; rowIndex++)
            {
                IRow row = sheet.GetRow(rowIndex);

                Model.Event parsedEvent = await ParseEvent(row);

                events.Add(parsedEvent);
            }
        }

        return events;
    }

    private async Task<Model.Event> ParseEvent(IRow row)
    {
        // Get the event data from the row
        string eventDateString = row.GetCell(1).GetValue<string>() ?? string.Empty;
        string eventTimeString = row.GetCell(2).GetValue<string>() ?? string.Empty;
                
        DateTime eventDateTime = ParseEventDateTime(eventDateString, eventTimeString);
        
        string eventTypeString = row.GetCell(3).GetValue<string>() ?? "Unknown";
        EventType eventType = await _repository.GetOrCreateEventType(eventTypeString);

        string eventInformation = row.GetCell(4).GetValue<string>() ?? string.Empty;

        string venueString = row.GetCell(5).GetValue<string>() ?? "Unknown";
        Venue venue = await _repository.GetOrCreateVenue(venueString);

        string costString = row.GetCell(6).GetValue<string>() ?? string.Empty;
        Cost cost = ParseUtility.ParseCost(costString);

        ICell eventCell = row.GetCell(7);
        string eventLink = eventCell.GetValue<string>() ?? "Unknown";
        string url = eventCell.GetHyperlink() ?? string.Empty;

        return new Model.Event()
        {
            DateTime = eventDateTime,
            EventType = eventType,
            EventTypeId = eventType.EventTypeId,
            Information = eventInformation,
            Venue = venue,
            VenueId = venue.VenueId,
            Cost = cost,
            CostId = cost.CostId,
            LinkText = eventLink,
            Url = url,
        };
    }

    private Cost ParseCost(string costString)
    {
        throw new NotImplementedException();
    }

    private DateTime ParseEventDateTime(string? eventDateString, string? eventTimeString)
    {
        try
        {
            if (eventDateString is null) throw new ArgumentNullException(nameof(eventDateString));
            if (string.IsNullOrEmpty(eventTimeString))
            {
                // If no time is provided, just parse the date
                return DateTime.ParseExact(eventDateString, "MMM d, yyyy", CultureInfo.InvariantCulture);
            }

            DateTime date = DateTime.ParseExact(eventDateString, "MMM d, yyyy", CultureInfo.InvariantCulture);
            DateTime time = DateTime.ParseExact(eventTimeString, "h:mmm tt", CultureInfo.InvariantCulture);

            return new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
        }
        catch (Exception e)
        {
            return new DateTime();
        }
    }

    #endregion
}