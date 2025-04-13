using MonctonEventsNet.Application.Excel;
using OfficeOpenXml;

namespace MonctonEventsNet.Application.EpPlus;

/// <summary>
/// Represents a row in an Excel worksheet using the EPPlus library.
/// Implements the <see cref="IRow"/> interface.
/// </summary>
public class EpPlusRow : IRow
{
    #region Private Fields

    /// <summary>
    /// The underlying ExcelWorksheet object representing the worksheet.
    /// </summary>
    private readonly ExcelWorksheet _worksheet;

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the row number of this row in the worksheet.
    /// </summary>
    public int RowNumber { get; }

    /// <summary>
    /// Gets an enumerable collection of cells in this row.
    /// </summary>
    public IEnumerable<ICell> Cells
    {
        get
        {
            int columnCount = _worksheet.Dimension?.Columns ?? 0;
            for (int i = 1; i <= columnCount; i++)
            {
                yield return GetCell(i);
            }
        }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="EpPlusRow"/> class.
    /// </summary>
    /// <param name="worksheet">The ExcelWorksheet object representing the worksheet.</param>
    /// <param name="rowNumber">The row number of this row in the worksheet.</param>
    public EpPlusRow(ExcelWorksheet worksheet, int rowNumber)
    {
        _worksheet = worksheet;
        RowNumber = rowNumber;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the cell at the specified column in this row.
    /// </summary>
    /// <param name="column">The column number of the cell to retrieve.</param>
    /// <returns>An <see cref="ICell"/> object representing the cell at the specified column.</returns>
    public ICell GetCell(int column)
    {
        return new EpPlusCell(_worksheet.Cells[RowNumber, column]);
    }

    #endregion
}