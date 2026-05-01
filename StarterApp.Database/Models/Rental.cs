namespace StarterApp.Database.Models;

// Represents a rental request for an item
public class Rental
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public int RenterId { get; set; }

    public int OwnerId { get; set; }

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    public string Status { get; set; } = "Pending";

    // Item being requested
    public Item? Item { get; set; }

    // User requesting the rental
    public User? Renter { get; set; }

    // User who owns the item
    public User? Owner { get; set; }
}