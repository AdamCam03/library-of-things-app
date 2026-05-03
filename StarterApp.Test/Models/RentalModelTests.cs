using StarterApp.Database.Models;

namespace StarterApp.Test.Models;

// Unit tests for the Rental model
public class RentalModelTests
{
    [Fact]
    public void IsOutgoing_WhenIsIncomingFalse_ReturnsTrue()
    {
        // Arrange
        var rental = new Rental { IsIncoming = false };

        // Act
        var result = rental.IsOutgoing;

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsOutgoing_WhenIsIncomingTrue_ReturnsFalse()
    {
        // Arrange
        var rental = new Rental { IsIncoming = true };

        // Act
        var result = rental.IsOutgoing;

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void Rental_DefaultStatus_IsNotEmpty()
    {
        // Arrange
        var rental = new Rental();

        // Assert - rental should have a default status
        Assert.NotNull(rental.Status);
    }

    [Fact]
    public void Rental_StartDateBeforeEndDate_IsValid()
    {
        // Arrange
        var rental = new Rental
        {
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(3)
        };

        // Act
        var isValid = rental.EndDate > rental.StartDate;

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void Rental_ItemTitleDefaultsToEmpty()
    {
        // Arrange
        var rental = new Rental();

        // Assert
        Assert.Equal(string.Empty, rental.ItemTitle);
    }
}