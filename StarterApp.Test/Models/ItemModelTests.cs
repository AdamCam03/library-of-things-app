using StarterApp.Database.Models;

namespace StarterApp.Test.Models;

// Unit tests for the Item model
public class ItemModelTests
{
    [Fact]
    public void DisplayLocation_EdinburghCoordinates_ReturnsEdinburgh()
    {
        // Arrange
        var item = new Item { Latitude = 55.9533, Longitude = -3.1883 };

        // Act
        var result = item.DisplayLocation;

        // Assert
        Assert.Equal("Edinburgh", result);
    }

    [Fact]
    public void DisplayLocation_GlasgowCoordinates_ReturnsGlasgow()
    {
        // Arrange
        var item = new Item { Latitude = 55.8617, Longitude = -4.2583 };

        // Act
        var result = item.DisplayLocation;

        // Assert
        Assert.Equal("Glasgow", result);
    }

    [Fact]
    public void DisplayLocation_PaisleyCoordinates_ReturnsPaisley()
    {
        // Arrange
        var item = new Item { Latitude = 55.8456, Longitude = -4.4239 };

        // Act
        var result = item.DisplayLocation;

        // Assert
        Assert.Equal("Paisley", result);
    }

    [Fact]
    public void DisplayLocation_UnknownCoordinates_ReturnsRawCoordinates()
    {
        // Arrange
        var item = new Item { Latitude = 51.5074, Longitude = -0.1278 };

        // Act
        var result = item.DisplayLocation;

        // Assert - unknown location should return raw coordinates
        Assert.Contains("51.5074", result);
    }

    [Fact]
    public void DisplayLocation_ZeroCoordinates_ReturnsEmptyString()
    {
        // Arrange
        var item = new Item { Latitude = 0, Longitude = 0 };

        // Act
        var result = item.DisplayLocation;

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void Item_DailyRate_ShouldBeSetCorrectly()
    {
        // Arrange
        var item = new Item { DailyRate = 12.50m };

        // Assert
        Assert.Equal(12.50m, item.DailyRate);
    }
}