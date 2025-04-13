using MonctonEventsNet.Application.Excel;
using OfficeOpenXml;

namespace MonctonEventsNet.Application.EpPlus;

/// <summary>
/// Represents a worksheet in an Excel workbook using the EPPlus library.
/// Implements the <see cref="IWorksheet"/> interface.
/// </summary>
public class EpPlusWorksheet : IWorksheet
{
    #region Private Fields

    /// <summary>
    /// The underlying ExcelWorksheet object representing the worksheet.
    /// </summary>
    private readonly ExcelWorksheet _worksheet;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the name of the worksheet.
    /// </summary>
    public string Name => _worksheet.Name;

    /// <summary>
    /// Gets the total number of rows in the worksheet.
    /// </summary>
    public int RowCount => _worksheet.Dimension?.Rows ?? 0;

    /// <summary>
    /// Gets the total number of columns in the worksheet.
    /// </summary>
    public int ColumnCount => _worksheet.Dimension?.Columns ?? 0;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="EpPlusWorksheet"/> class.
    /// </summary>
    /// <param name="worksheet">The ExcelWorksheet object representing the worksheet.</param>
    public EpPlusWorksheet(ExcelWorksheet worksheet)
    {
        _worksheet = worksheet;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the cell at the specified row and column.
    /// </summary>
    /// <param name="row">The row number of the cell.</param>
    /// <param name="column">The column number of the cell.</param>
    /// <returns>An <see cref="ICell"/> object representing the cell at the specified row and column.</returns>
    public ICell GetCell(int row, int column)
    {
        if (row < 1 || column < 1 || row > RowCount || column > ColumnCount)
        {
            string address = $"{GetColumnLetter(column)}{row}";
            return new EpPlusCell(null, address);
        }

        return new EpPlusCell(_worksheet.Cells[row, column]);
    }

    /// <summary>
    /// Gets the cell at the specified address.
    /// </summary>
    /// <param name="address">The address of the cell (e.g., "A1").</param>
    /// <returns>An <see cref="ICell"/> object representing the cell at the specified address.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the address is null or empty.</exception>
    public ICell GetCell(string address)
    {
        if (string.IsNullOrEmpty(address))
            throw new ArgumentNullException(nameof(address), "Address cannot be null or empty.");

        return new EpPlusCell(_worksheet.Cells[address]);
    }

    /// <summary>
    /// Gets an enumerable collection of rows in the worksheet, starting from the specified row.
    /// </summary>
    /// <param name="startRow">The row number to start from.</param>
    /// <param name="endRow">The optional row number to end at. If null, all rows to the end of the worksheet are included.</param>
    /// <returns>An enumerable collection of <see cref="IRow"/> objects representing the rows in the worksheet.</returns>
    public IEnumerable<IRow> GetRows(int startRow, int? endRow = null)
    {
        if (RowCount == 0)
            yield break;

        int lastRow = endRow ?? RowCount;

        for (int i = startRow; i <= lastRow; i++)
        {
            yield return new EpPlusRow(_worksheet, i);
        }
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Converts a column number to its corresponding Excel column letter.
    /// </summary>
    /// <param name="columnNumber">The column number to convert.</param>
    /// <returns>A string representing the Excel column letter.</returns>
    private string GetColumnLetter(int columnNumber)
    {
        int dividend = columnNumber;
        string columnName = string.Empty;

        while (dividend > 0)
        {
            // Converts column number into a letter (1 -> A, 2 -> B, ..., 26 -> Z, 27 -> AA, 28 -> AB, ...)
            int modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo) + columnName;
            dividend = (dividend - modulo) / 26;
        }

        return columnName;
    }

    #endregion
}