using MonctonEventsNet.Application.Excel;
using OfficeOpenXml;

namespace MonctonEventsNet.Application.EpPlus;

/// <summary>
/// Service for reading Excel spreadsheets using the EPPlus library.
/// Implements the <see cref="ISpreadsheetReaderService"/> interface.
/// </summary>
public class EpPlusSpreadsheetReaderService : ISpreadsheetReaderService
{
    /// <summary>
    /// Reads an Excel workbook asynchronously from the provided file stream.
    /// </summary>
    /// <param name="fileStream">The stream containing the Excel file to be read.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains
    /// an <see cref="IWorkbook"/> object representing the read workbook.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the Excel package could not be created from the provided stream.
    /// </exception>
    public async Task<IWorkbook> ReadAsync(Stream fileStream)
    {
        // Set the EPPlus license to non-commercial personal use.
        ExcelPackage.License.SetNonCommercialPersonal("<Your Name>");
        ExcelPackage? package = null;

        // Create the ExcelPackage asynchronously using a separate task.
        await Task.Run(() => { package = new ExcelPackage(fileStream); });

        // Throw an exception if the package creation failed.
        if (package == null)
            throw new InvalidOperationException("Failed to create ExcelPackage.");

        // Return the workbook wrapped in an EpPlusWorkbook instance.
        return new EpPlusWorkbook(package);
    }
}