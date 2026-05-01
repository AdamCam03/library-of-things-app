using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

// Repository implementation for rental request data access
public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    // Constructor - injects the database context
    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    // Creates a new rental request
    public async Task<Rental> CreateAsync(Rental rental)
    {
        rental.RequestedAt = DateTime.UtcNow;
        rental.Status = "Pending";

        _context.Rentals.Add(rental);
        await _context.SaveChangesAsync();

        return rental;
    }

    // Gets rental requests made by a user
    public async Task<List<Rental>> GetOutgoingRequestsAsync(int renterId)
    {
        return await _context.Rentals
            .Include(rental => rental.Item)
            .Include(rental => rental.Renter)
            .Include(rental => rental.Owner)
            .Where(rental => rental.RenterId == renterId)
            .OrderByDescending(rental => rental.RequestedAt)
            .ToListAsync();
    }

    // Gets rental requests received by an owner
    public async Task<List<Rental>> GetIncomingRequestsAsync(int ownerId)
    {
        return await _context.Rentals
            .Include(rental => rental.Item)
            .Include(rental => rental.Renter)
            .Include(rental => rental.Owner)
            .Where(rental => rental.OwnerId == ownerId)
            .OrderByDescending(rental => rental.RequestedAt)
            .ToListAsync();
    }

    // Gets all rental requests where the user is either renter or owner
    public async Task<List<Rental>> GetByUserIdAsync(int userId)
    {
        return await _context.Rentals
            .Include(rental => rental.Item)
            .Include(rental => rental.Renter)
            .Include(rental => rental.Owner)
            .Where(rental => rental.RenterId == userId || rental.OwnerId == userId)
            .OrderByDescending(rental => rental.RequestedAt)
            .ToListAsync();
    }

    // Updates a rental request
    public async Task UpdateAsync(Rental rental)
    {
        var existingRental = await _context.Rentals.FindAsync(rental.Id);

        if (existingRental == null)
        {
            throw new InvalidOperationException("Rental request not found");
        }

        existingRental.Status = rental.Status;

        await _context.SaveChangesAsync();
    }
}