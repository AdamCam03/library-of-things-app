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

    // Gets all rental requests from the database
    public async Task<List<Rental>> GetAllAsync()
    {
        return await _context.Rentals
            .OrderByDescending(rental => rental.RequestedAt)
            .ToListAsync();
    }

    // Gets a single rental request by its ID
    public async Task<Rental?> GetByIdAsync(int id)
    {
        return await _context.Rentals
            .FirstOrDefaultAsync(rental => rental.Id == id);
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
            .Where(rental => rental.RenterId == renterId)
            .OrderByDescending(rental => rental.RequestedAt)
            .ToListAsync();
    }

    // Gets rental requests received by an owner
    public async Task<List<Rental>> GetIncomingRequestsAsync(int ownerId)
    {
        return await _context.Rentals
            .Where(rental => rental.OwnerId == ownerId)
            .OrderByDescending(rental => rental.RequestedAt)
            .ToListAsync();
    }

    // Gets all rental requests where the user is either renter or owner
    public async Task<List<Rental>> GetByUserIdAsync(int userId)
    {
        return await _context.Rentals
            .Where(rental => rental.RenterId == userId || rental.OwnerId == userId)
            .OrderByDescending(rental => rental.RequestedAt)
            .ToListAsync();
    }

    // Updates a rental request status
    public async Task UpdateAsync(Rental rental)
    {
        var existingRental = await _context.Rentals.FindAsync(rental.Id);
        if (existingRental == null)
            throw new InvalidOperationException("Rental request not found");
        existingRental.Status = rental.Status;
        await _context.SaveChangesAsync();
    }

    // Deletes a rental request by its ID
    public async Task DeleteAsync(int id)
    {
        var rental = await _context.Rentals.FindAsync(id);
        if (rental == null)
            throw new InvalidOperationException("Rental request not found");
        _context.Rentals.Remove(rental);
        await _context.SaveChangesAsync();
    }
}