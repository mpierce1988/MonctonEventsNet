using System.Runtime.Serialization;
using MonctonEventsNet.Application.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MonctonEventsNet.Application.EpPlus;

/// <summary>
/// Represents a cell in an Excel worksheet using the EPPlus library.
/// Implements the <see cref="ICell"/> interface.
/// </summary>
public class EpPlusCell : ICell
{
    #region Private Fields

    /// <summary>
    /// The underlying ExcelRange object representing the cell.
    /// </summary>
    private readonly ExcelRange? _cell;
    private static string _generalFormat = "General";

    #endregion

    #region Public Properties

    /// <summary>
    /// Gets the address of the cell (e.g., "A1").
    /// </summary>
    public string Address { get; }

    /// <summary>
    /// Gets the value of the cell.
    /// </summary>
    public object? Value => _cell?.Value;

    /// <summary>
    /// Gets the data type of the cell's value.
    /// </summary>
    public CellDataType DataType
    {
        get
        {
            if (_cell == null || _cell.Value == null)
                return CellDataType.Empty;

            switch (_cell.Value)
            {
                case string _:
                    return CellDataType.Text;
                case DateTime _:
                    return CellDataType.Date;
                case double val:
                    return _cell.Style.Numberformat.Format == _generalFormat ? CellDataType.Number : CellDataType.Date;
                case decimal _:
                case int _:
                case float _:
                case long _:
                    return CellDataType.Number;
                case bool _:
                    return CellDataType.Boolean;
                case ExcelErrorValue _:
                    return CellDataType.Error;
                default:
                    return CellDataType.Text;
            }
        }
    }

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="EpPlusCell"/> class.
    /// </summary>
    /// <param name="cell">The ExcelRange object representing the cell.</param>
    /// <param name="address">The address of the cell. If null, the address from the ExcelRange is used.</param>
    public EpPlusCell(ExcelRange? cell, string? address = null)
    {
        _cell = cell;
        Address = address ?? cell?.Address ?? string.Empty;
    }

    #endregion

    /// <summary>
    /// Gets the value of the cell as the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the cell's value should be converted.</typeparam>
    /// <returns>The value of the cell as the specified type, or the default value of the type if conversion fails.</returns>
    public T? GetValue<T>()
    {
        if (_cell?.Value == null)
        {
            return default;
        }

        try
        {
            if (typeof(T) == typeof(string))
            {
                return (T)(object)_cell.Text;
            }

            if (typeof(T) == typeof(int) && _cell.Value is double doubleValue)
            {
                return (T)(object)(int)doubleValue;
            }

            if (typeof(T) == typeof(DateTime) && _cell.Value is double dateDouble)
            {
                return (T)(object)DateTime.FromOADate(dateDouble);
            }

            // Generic conversion attempt
            return (T)Convert.ChangeType(_cell.Value, typeof(T));
        }
        catch (Exception)
        {
            return default;
        }
    }

    /// <summary>
    /// Tries to get the value of the cell as the specified type.
    /// </summary>
    /// <typeparam name="T">The type to which the cell's value should be converted.</typeparam>
    /// <param name="value">The converted value of the cell, or the default value of the type if conversion fails.</param>
    /// <returns>True if the conversion is successful; otherwise, false.</returns>
    public bool TryGetValue<T>(out T? value)
    {
        value = default;

        if (_cell?.Value == null)
        {
            return false;
        }

        try
        {
            value = GetValue<T>();

            return value is not null;
        }
        catch (Exception)
        {
            return false;
        }
    }
}