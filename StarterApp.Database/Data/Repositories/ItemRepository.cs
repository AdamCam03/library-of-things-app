using Microsoft.EntityFrameworkCore;
using StarterApp.Database.Data;
using StarterApp.Database.Models;

namespace StarterApp.Database.Data.Repositories;

//Repository implementation for Item data access using Entity Framework
public class ItemRepository : IItemRepository
{
    private readonly AppDbContext _context;

    //Constructor - injects the database context
    public ItemRepository(AppDbContext context)
    {
        _context = context;
    }

    //Gets all items from the database
    public async Task<List<Item>> GetAllAsync()
    {
        return await _context.Items
            .OrderByDescending(item => item.CreatedAt)
            .ToListAsync();
    }

    //Gets a single item by its ID
    public async Task<Item?> GetByIdAsync(int id)
    {
        return await _context.Items
            .FirstOrDefaultAsync(item => item.Id == id);
    }

    //Creates a new item in the database
    public async Task<Item> CreateAsync(Item item)
    {
        item.CreatedAt = DateTime.UtcNow;

        _context.Items.Add(item);
        await _context.SaveChangesAsync();

        return item;
    }

    //Updates an existing item
    public async Task UpdateAsync(Item item)
    {
        var existingItem = await _context.Items.FindAsync(item.Id);

        if (existingItem == null)
        {
            throw new InvalidOperationException("Item not found");
        }

        existingItem.Title = item.Title;
        existingItem.Description = item.Description;
        existingItem.DailyRate = item.DailyRate;
        existingItem.Category = item.Category;
        existingItem.LocationName = item.LocationName;
        existingItem.IsAvailable = item.IsAvailable;
        existingItem.OwnerId = item.OwnerId;

        await _context.SaveChangesAsync();
    }

    //Deletes an item by its ID
    public async Task DeleteAsync(int id)
    {
        var item = await _context.Items.FindAsync(id);

        if (item == null)
        {
            throw new InvalidOperationException("Item not found");
        }

        _context.Items.Remove(item);
        await _context.SaveChangesAsync();
    }
}