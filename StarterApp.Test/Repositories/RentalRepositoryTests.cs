using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Test;

namespace StarterApp.Test.Repositories;

// Integration tests for the RentalRepository using an in-memory database
public class RentalRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public RentalRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllRentals()
    {
        // Arrange
        var repository = new RentalRepository(_fixture.Context);

        // Act
        var rentals = await repository.GetAllAsync();

        // Assert
        Assert.NotEmpty(rentals);
    }

    [Fact]
    public async Task GetByIdAsync_ValidId_ReturnsRental()
    {
        // Arrange
        var repository = new RentalRepository(_fixture.Context);

        // Act
        var rental = await repository.GetByIdAsync(1);

        // Assert
        Assert.NotNull(rental);
        Assert.Equal(1, rental.Id);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        var repository = new RentalRepository(_fixture.Context);

        // Act
        var rental = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(rental);
    }

    [Fact]
    public async Task GetIncomingRequestsAsync_ReturnsOwnerRentals()
    {
        // Arrange
        var repository = new RentalRepository(_fixture.Context);

        // Act
        var rentals = await repository.GetIncomingRequestsAsync(1);

        // Assert - owner 1 should have incoming rentals
        Assert.NotEmpty(rentals);
        Assert.All(rentals, rental => Assert.Equal(1, rental.OwnerId));
    }

    [Fact]
    public async Task GetOutgoingRequestsAsync_ReturnsRenterRentals()
    {
        // Arrange
        var repository = new RentalRepository(_fixture.Context);

        // Act
        var rentals = await repository.GetOutgoingRequestsAsync(1);

        // Assert - renter 1 should have outgoing rentals
        Assert.NotEmpty(rentals);
        Assert.All(rentals, rental => Assert.Equal(1, rental.BorrowerId));
    }

    [Fact]
    public async Task CreateAsync_ValidRental_SavesAndReturnsRental()
    {
        // Arrange
        var repository = new RentalRepository(_fixture.Context);
        var newRental = new Rental
        {
            ItemId = 1,
            BorrowerId = 2,
            OwnerId = 1,
            StartDate = DateTime.Today.AddDays(1),
            EndDate = DateTime.Today.AddDays(3)
        };

        // Act
        var created = await repository.CreateAsync(newRental);

        // Assert
        Assert.NotNull(created);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task UpdateAsync_ValidRental_UpdatesStatus()
    {
        // Arrange
        var repository = new RentalRepository(_fixture.Context);
        var rental = await repository.GetByIdAsync(1);
        rental!.Status = "Approved";

        // Act
        await repository.UpdateAsync(rental);
        var updated = await repository.GetByIdAsync(1);

        // Assert
        Assert.Equal("Approved", updated!.Status);
    }
}