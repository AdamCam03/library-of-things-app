namespace StarterApp.Database.Models;

// Represents a rental request for an item
public class Rental
{
    public int Id { get; set; }

    public int ItemId { get; set; }

    public string ItemTitle { get; set; } = string.Empty;

    // API uses borrowerId, mapped from RenterId
    public int BorrowerId { get; set; }

    public int RenterId
    {
        get => BorrowerId;
        set => BorrowerId = value;
    }

    public string BorrowerName { get; set; } = string.Empty;

    public int OwnerId { get; set; }

    public string OwnerName { get; set; } = string.Empty;

    public DateTime StartDate { get; set; } = DateTime.UtcNow.AddDays(1);

    public DateTime EndDate { get; set; } = DateTime.UtcNow.AddDays(2);

    public string Message { get; set; } = string.Empty;

    public string Status { get; set; } = "Requested";

    public decimal TotalPrice { get; set; }

    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Set by ViewModel to indicate if this is an incoming request
    // This is not stored in the database
    public bool IsIncoming { get; set; } = false;
    public bool IsOutgoing => !IsIncoming;
    
    // Navigation properties
    public Item? Item { get; set; }
    public User? Renter { get; set; }
    public User? Owner { get; set; }
}