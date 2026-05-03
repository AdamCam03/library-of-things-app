using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

// Repository interface for rental request data access
// Extends the generic IRepository<T> with rental-specific methods
public interface IRentalRepository : IRepository<Rental>
{
    // Gets rental requests made by a user
    Task<List<Rental>> GetOutgoingRequestsAsync(int renterId);

    // Gets rental requests received by an owner
    Task<List<Rental>> GetIncomingRequestsAsync(int ownerId);

    // Gets all rental requests where the user is either renter or owner
    Task<List<Rental>> GetByUserIdAsync(int userId);
}