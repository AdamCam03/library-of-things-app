using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

// Repository interface for rental request data access
public interface IRentalRepository
{
    // Creates a new rental request
    Task<Rental> CreateAsync(Rental rental);

    // Gets rental requests made by a user
    Task<List<Rental>> GetOutgoingRequestsAsync(int renterId);

    // Gets rental requests received by an owner
    Task<List<Rental>> GetIncomingRequestsAsync(int ownerId);

    // Gets all rental requests where the user is either renter or owner
    Task<List<Rental>> GetByUserIdAsync(int userId);

    // Updates a rental request
    Task UpdateAsync(Rental rental);
}