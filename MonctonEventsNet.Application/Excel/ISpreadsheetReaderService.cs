namespace MonctonEventsNet.Application.Excel;

public interface ISpreadsheetReaderService
{
    Task<IWorkbook> ReadAsync(Stream fileStream);
}

public interface IWorkbook
{
    IEnumerable<IWorksheet> Worksheets { get; }
    IWorksheet? GetWorksheetByName(string name);
    IWorksheet? GetWorksheetByIndex(int index);
}

public interface IWorksheet
{
    string Name { get; }
    ICell? GetCell(int row, int column);
    ICell? GetCell(string address);
    IEnumerable<IRow> GetRows(int startRow, int? endRow = null);
    IRow GetRow(int row);
    int RowCount { get; }
    int ColumnCount { get; }
}

public interface IRow
{
    int RowNumber { get; }
    IEnumerable<ICell> Cells { get; }
    ICell GetCell(int column);
}

public interface ICell
{
    string Address { get; }
    object? Value { get; }
    T? GetValue<T>();
    bool TryGetValue<T>(out T? value);

    string? GetHyperlink();
    CellDataType DataType { get; }
}

public enum CellDataType
{
    Text,
    Number,
    Date,
    Boolean,
    Error,
    Empty
}