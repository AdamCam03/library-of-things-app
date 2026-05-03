using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;

namespace StarterApp.Test;

public class DatabaseFixture : IDisposable
{
    public AppDbContext Context { get; private set; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid())
            .Options;

        Context = new AppDbContext(options);
        Context.Database.EnsureCreated();
        SeedTestData();
    }

    private void SeedTestData()
    {
        var items = new List<Item>
        {
            new Item { Id = 1, Title = "Power Drill", Description = "A great drill", DailyRate = 12, CategoryId = 1, Category = "Tools", OwnerId = 1, Latitude = 55.9533, Longitude = -3.1883 },
            new Item { Id = 2, Title = "Tent", Description = "4 person tent", DailyRate = 20, CategoryId = 3, Category = "Camping", OwnerId = 2, Latitude = 55.8617, Longitude = -4.2583 },
            new Item { Id = 3, Title = "Bicycle", Description = "Mountain bike", DailyRate = 15, CategoryId = 8, Category = "Cycling", OwnerId = 1, Latitude = 55.8456, Longitude = -4.4239 }
        };
        Context.Items.AddRange(items);

        var rentals = new List<Rental>
        {
            new Rental { Id = 1, ItemId = 1, BorrowerId = 2, OwnerId = 1, Status = "Requested", StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3) },
            new Rental { Id = 2, ItemId = 2, BorrowerId = 1, OwnerId = 2, Status = "Approved", StartDate = DateTime.Today.AddDays(2), EndDate = DateTime.Today.AddDays(4) }
        };
        Context.Rentals.AddRange(rentals);

        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}