using System;

namespace StarterApp.Database.Models;

public class Item
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal DailyRate { get; set; }

    public int CategoryId { get; set; } = 1;

    public string Category { get; set; } = string.Empty;

    public string LocationName { get; set; } = string.Empty;

    public double Latitude { get; set; } = 55.9533;

    public double Longitude { get; set; } = -3.1883;

    public bool IsAvailable { get; set; } = true;

    public int OwnerId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
