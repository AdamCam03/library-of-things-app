using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Test;

namespace StarterApp.Test.Repositories;

// Integration tests for the ItemRepository using an in-memory database
public class ItemRepositoryTests : IClassFixture<DatabaseFixture>
{
    private readonly DatabaseFixture _fixture;

    public ItemRepositoryTests(DatabaseFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllItems()
    {
        // Arrange
        var repository = new ItemRepository(_fixture.Context);

        // Act
        var items = await repository.GetAllAsync();

        // Assert - should return all seeded items
        Assert.NotEmpty(items);
    }

[Fact]
public async Task GetByIdAsync_ValidId_ReturnsItem()
{
    // Arrange
    var repository = new ItemRepository(_fixture.Context);

    // Act
    var item = await repository.GetByIdAsync(1);

    // Assert - item should exist with correct ID
    Assert.NotNull(item);
    Assert.Equal(1, item.Id);
}

    [Fact]
    public async Task GetByIdAsync_InvalidId_ReturnsNull()
    {
        // Arrange
        var repository = new ItemRepository(_fixture.Context);

        // Act
        var item = await repository.GetByIdAsync(999);

        // Assert
        Assert.Null(item);
    }

    [Fact]
    public async Task GetByOwnerIdAsync_ValidOwner_ReturnsOwnerItems()
    {
        // Arrange
        var repository = new ItemRepository(_fixture.Context);

        // Act
        var items = await repository.GetByOwnerIdAsync(1);

        // Assert - owner 1 has 2 items seeded
        Assert.NotEmpty(items);
        Assert.All(items, item => Assert.Equal(1, item.OwnerId));
    }

    [Fact]
    public async Task GetAvailableItemsForUserAsync_ExcludesOwnItems()
    {
        // Arrange
        var repository = new ItemRepository(_fixture.Context);

        // Act
        var items = await repository.GetAvailableItemsForUserAsync(1);

        // Assert - should not include items owned by user 1
        Assert.All(items, item => Assert.NotEqual(1, item.OwnerId));
    }

    [Fact]
    public async Task CreateAsync_ValidItem_SavesAndReturnsItem()
    {
        // Arrange
        var repository = new ItemRepository(_fixture.Context);
        var newItem = new Item
        {
            Title = "Test Ladder",
            Description = "A sturdy ladder",
            DailyRate = 8,
            CategoryId = 1,
            Category = "Tools",
            OwnerId = 1,
            Latitude = 55.9533,
            Longitude = -3.1883
        };

        // Act
        var created = await repository.CreateAsync(newItem);

        // Assert
        Assert.NotNull(created);
        Assert.Equal("Test Ladder", created.Title);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task UpdateAsync_ValidItem_UpdatesSuccessfully()
    {
        // Arrange
        var repository = new ItemRepository(_fixture.Context);
        var item = await repository.GetByIdAsync(1);
        item!.Title = "Updated Drill";

        // Act
        await repository.UpdateAsync(item);
        var updated = await repository.GetByIdAsync(1);

        // Assert
        Assert.Equal("Updated Drill", updated!.Title);
    }
}