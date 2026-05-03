using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

// Repository interface for Item data access
// Extends the generic IRepository<T> with Item-specific methods
// Abstracts the data source from ViewModels
public interface IItemRepository : IRepository<Item>
{
    // Gets all items owned by a specific user
    Task<List<Item>> GetByOwnerIdAsync(int ownerId);

    // Gets all items NOT owned by the current user (for browsing)
    Task<List<Item>> GetAvailableItemsForUserAsync(int userId);
}