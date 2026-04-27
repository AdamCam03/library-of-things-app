using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

//Repository Interface for Item data access
// This abstracts the data source from the ViewModels
public interface IItemRepository
{
   //Gets all items and returns them as a list
    Task<List<Item>> GetAllAsync();

//Gets a single item by its ID and returns item if ID is found otherwise returns null
    Task<Item?> GetByIdAsync(int id);

   //Creates an item and returns the created item
    Task<Item> CreateAsync(Item item);

  //Updates an item
    Task UpdateAsync(Item item);

    //Deletes an item by its ID
    Task DeleteAsync(int id);
}