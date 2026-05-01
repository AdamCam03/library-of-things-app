using System.Net.Http.Json;
using StarterApp.Database.Models;
using StarterApp.Database.Data.Repositories;

namespace StarterApp.Services;

// API-backed implementation of IItemRepository
// ViewModels don't know or care whether this or the local DB version is injected
public class ApiItemService : IItemRepository
{
    private readonly HttpClient _httpClient;

    public ApiItemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Item>> GetAllAsync()
    {
        try
        {
            var items = await _httpClient.GetFromJsonAsync<List<Item>>("items");
            return items ?? new List<Item>();
        }
        catch
        {
            return new List<Item>();
        }
    }

    public async Task<Item?> GetByIdAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<Item>($"items/{id}");
        }
        catch
        {
            return null;
        }
    }

    public async Task<List<Item>> GetByOwnerIdAsync(int ownerId)
    {
        try
        {
            var items = await _httpClient.GetFromJsonAsync<List<Item>>($"items?ownerId={ownerId}");
            return items ?? new List<Item>();
        }
        catch
        {
            return new List<Item>();
        }
    }

    public async Task<List<Item>> GetAvailableItemsForUserAsync(int userId)
    {
        try
        {
            var allItems = await GetAllAsync();
            // Filter out items owned by the current user
            return allItems.Where(i => i.OwnerId != userId).ToList();
        }
        catch
        {
            return new List<Item>();
        }
    }

    public async Task<Item> CreateAsync(Item item)
    {
        var response = await _httpClient.PostAsJsonAsync("items", new
        {
            title = item.Title,
            description = item.Description,
            dailyRate = item.DailyRate,
            category = item.Category,
            locationName = item.LocationName
        });

        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<Item>();
        return created!;
    }

    public async Task UpdateAsync(Item item)
    {
        var response = await _httpClient.PutAsJsonAsync($"items/{item.Id}", new
        {
            title = item.Title,
            description = item.Description,
            dailyRate = item.DailyRate,
            category = item.Category,
            locationName = item.LocationName,
            isAvailable = item.IsAvailable
        });

        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"items/{id}");
        response.EnsureSuccessStatusCode();
    }
}