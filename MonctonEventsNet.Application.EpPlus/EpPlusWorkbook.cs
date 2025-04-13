using MonctonEventsNet.Application.Excel;
using OfficeOpenXml;

namespace MonctonEventsNet.Application.EpPlus;

public class EpPlusWorkbook : IWorkbook, IDisposable
{
    #region Private Fields
    
    private readonly ExcelPackage _package;
    
    #endregion
    
    #region Public Properties
    
    public IEnumerable<IWorksheet> Worksheets { get; }
    
    #endregion
    
    #region Constructor
    
    public EpPlusWorkbook(ExcelPackage package)
    {
        _package = package;
        Worksheets = _package.Workbook.Worksheets.Select(ws => new EpPlusWorksheet(ws));
    }
    
    #endregion
    
    #region Public Methods
    
    public IWorksheet? GetWorksheetByName(string name)
    {
        var worksheet = _package.Workbook.Worksheets[name];
        return worksheet != null ? new EpPlusWorksheet(worksheet) : null;
    }
    
    public IWorksheet? GetWorksheetByIndex(int index)
    {
        if (index < 1 || index > _package.Workbook.Worksheets.Count)
            return null;

        return new EpPlusWorksheet(_package.Workbook.Worksheets[index]);
    }
    
    #endregion
    
    #region IDisposable

    public void Dispose()
    {
        _package.Dispose();
    }
    
    #endregion
}