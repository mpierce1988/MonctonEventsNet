using MonctonEventsNet.Application.Utilities;
using MonctonEventsNet.Model;

namespace MonctonEventsNet.Application.Tests;

public class ParserUtilityTests
{
    #region ParseCost
    
    [Fact]
    public void ParseCost_Free_ReturnsFreeCost()
    {
        // Arrange
        var costString = "Free";
        decimal expectedMinCost = 0.0m;
        decimal? expectedMaxCost = null;
        string expectedInformation = "Free";
        
        // Act
        Cost actualCost = ParseUtility.ParseCost(costString);
        
        // Assert
        Assert.Equal(expectedMinCost, actualCost.MinCost);
        Assert.Equal(expectedMaxCost, actualCost.MaxCost);
        Assert.Equal(expectedInformation, actualCost.Information);
    }

    [Fact]
    public void ParseCost_SingleValue_ReturnsMinCost()
    {
        // Arrange
        string costString = "$40";
        decimal expectedMinCost = 40;
        decimal? expectedMaxCost = null;
        
        // Act
        Cost actualCost = ParseUtility.ParseCost(costString);
        
        // Assert
        Assert.Equal(expectedMinCost, actualCost.MinCost);
        Assert.Equal(expectedMaxCost, actualCost.MaxCost);
    }

    [Fact]
    public void ParseCost_DoubleValue_ReturnsMinMaxCost()
    {
        // Arrange
        string costString = "$20-$89";
        decimal expectedMinCost = 20m;
        decimal? expectedMaxCost = 89m;
        
        // Act
        Cost actualCost = ParseUtility.ParseCost(costString);
        
        // Assert
        Assert.Equal(expectedMinCost, actualCost.MinCost);
        Assert.Equal(expectedMaxCost, actualCost.MaxCost);
    }

    [Fact]
    public void ParseCost_SingleValueWithText_ReturnsMinCostInformation()
    {
        // Arange
        string costString = "$138\n(2 sessions)";
        decimal expectedMinCost = 138m;
        decimal? expectedMaxCost = null;
        string expectedInformation = "(2 sessions)";
        
        // Act
        Cost actualCost = ParseUtility.ParseCost(costString);
        
        // Assert
        Assert.Equal(expectedMinCost, actualCost.MinCost);
        Assert.Equal(expectedMaxCost, actualCost.MaxCost);
        Assert.Equal(expectedInformation, actualCost.Information);
    }

    [Fact]
    public void ParseCost_EmptyString_ReturnsDefaultCost()
    {
        // Arrange
        string costString = string.Empty;
        decimal expectedMinCost = 0m;
        decimal? expectedMaxCost = null;
        string? expectedInformation = null;
        
        // Act
        Cost actualCost = ParseUtility.ParseCost(costString);
        
        // Assert
        Assert.Equal(expectedMinCost, actualCost.MinCost);
        Assert.Equal(expectedMaxCost, actualCost.MaxCost);
        Assert.Equal(expectedInformation, actualCost.Information);
    }
    
    #endregion
}