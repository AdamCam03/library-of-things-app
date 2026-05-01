namespace StarterApp.Database.Models;

public class Rental
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public int RenterId { get; set; }

    public int OwnerId { get; set; }

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    public DateTime StartDate { get; set; } = DateTime.UtcNow.AddDays(1);

    public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(2);

    public string Message { get; set; } = string.Empty;

    public string Status { get; set; } = "Pending";

    // Navigation properties
    public Item? Item { get; set; }
    public User? Renter { get; set; }
    public User? Owner { get; set; }
}