using MonctonEventsNet.Application.Excel;

namespace MonctonEventsNet.Application.EpPlus.Test;

public class EpPlusSpreadsheetReaderServiceTests
{
    private readonly ISpreadsheetReaderService _spreadsheetReaderService;
    private readonly string _basicExcelFileName = "Samples/BasicExcel.xlsx";

    public EpPlusSpreadsheetReaderServiceTests()
    {
        _spreadsheetReaderService = new EpPlusSpreadsheetReaderService();
    }
    
    [Fact]
    public async Task ReadAsync_ValidHeaders_ReturnsCorrectHeaders()
    {
        // Arrange
        FileStream fileStream = new FileStream(_basicExcelFileName, FileMode.Open, FileAccess.Read);
        string firstColumnName = "ID";
        string secondColumnName = "Name";
        string thirdColumnName = "Balance";
        string fourthColumnName = "IsValid";
        string fifthColumnName = "LastLogin";
        
        // Act
        IWorkbook workbook = await _spreadsheetReaderService.ReadAsync(fileStream);
        
        // Assert
        Assert.NotNull(workbook);
        Assert.NotEmpty(workbook.Worksheets);
        IWorksheet worksheet = workbook.Worksheets.First();
        Assert.NotEmpty(worksheet.GetRows(0));
        IRow headerRow = worksheet.GetRows(1, 1).First();
        Assert.Equivalent(firstColumnName, headerRow.GetCell(1).GetValue<string>());
        Assert.Equivalent(secondColumnName, headerRow.GetCell(2).GetValue<string>());
        Assert.Equivalent(thirdColumnName, headerRow.GetCell(3).GetValue<string>());
        Assert.Equivalent(fourthColumnName, headerRow.GetCell(4).GetValue<string>());
        Assert.Equivalent(fifthColumnName, headerRow.GetCell(5).GetValue<string>());
    }

    [Fact]
    public async Task ReadAsync_ThreeRows_ReturnsTwoRows()
    {
        // Assert
        FileStream fileStream = new FileStream(_basicExcelFileName, FileMode.Open, FileAccess.Read);
        int expectedNumRows = 3;
        
        // Act
        IWorkbook workbook = await _spreadsheetReaderService.ReadAsync(fileStream);
        
        // Assert
        Assert.NotNull(workbook);
        Assert.NotEmpty(workbook.Worksheets);
        IWorksheet worksheet = workbook.Worksheets.First();
        Assert.Equal(expectedNumRows, worksheet.RowCount);
    }

    [Fact]
    public async Task ReadAsync_MultipleDataTypes_ReturnsCorrectDataTypes()
    {
        // Assert
        FileStream fileStream = new FileStream(_basicExcelFileName, FileMode.Open, FileAccess.Read);
        CellDataType firstCellDataType = CellDataType.Number;
        CellDataType secondCellDataType = CellDataType.Text;
        CellDataType thirdCellDataType = CellDataType.Number;
        CellDataType fourthCellDataType = CellDataType.Boolean;
        CellDataType fifthCellDataType = CellDataType.Date;
        
        // Act
        IWorkbook workbook = await _spreadsheetReaderService.ReadAsync(fileStream);
        
        // Assert
        Assert.NotNull(workbook);
        Assert.NotEmpty(workbook.Worksheets);
        IWorksheet worksheet = workbook.Worksheets.First();
        Assert.NotEmpty(worksheet.GetRows(1));
        IRow firstDataRow = worksheet.GetRows(2, 2).First();
        Assert.Equal(firstCellDataType, firstDataRow.GetCell(1).DataType);
        Assert.Equal(secondCellDataType, firstDataRow.GetCell(2).DataType);
        Assert.Equal(thirdCellDataType, firstDataRow.GetCell(3).DataType);
        Assert.Equal(fourthCellDataType, firstDataRow.GetCell(4).DataType);
        Assert.Equal(fifthCellDataType, firstDataRow.GetCell(5).DataType);
    }
}