namespace StarterApp.Database.Data.Repositories;

// Generic repository interface
// Provides a standard contract for all data access operations
// Abstracts data source from ViewModels following the Repository Pattern
public interface IRepository<T> where T : class
{
    // Gets all entities
    Task<List<T>> GetAllAsync();

    // Gets a single entity by its ID
    Task<T?> GetByIdAsync(int id);

    // Creates a new entity and returns it
    Task<T> CreateAsync(T entity);

    // Updates an existing entity
    Task UpdateAsync(T entity);

    // Deletes an entity by its ID
    Task DeleteAsync(int id);
}
