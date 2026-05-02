using System.Net.Http.Json;
using StarterApp.Database.Models;
using StarterApp.Database.Data.Repositories;

namespace StarterApp.Services;

// API-backed implementation of IItemRepository
// ViewModels don't know or care whether this or the local DB version is injected
public class ApiItemService : IItemRepository
{
    private readonly HttpClient _httpClient;

    // Constructor - injects the shared HttpClient which already has the JWT token set
    public ApiItemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Response wrapper for the items endpoint
    // The API returns { "items": [...] } not just [...]
    private class ItemsResponse
    {
        public List<Item> Items { get; set; } = new();
    }

    // Gets all items from the API
    public async Task<List<Item>> GetAllAsync()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ItemsResponse>("items");
            return response?.Items ?? new List<Item>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"GetAllAsync error: {ex.Message}");
            return new List<Item>();
        }
    }

    // Gets a single item by its ID
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

    // Gets all items owned by a specific user
    public async Task<List<Item>> GetByOwnerIdAsync(int ownerId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<ItemsResponse>($"items?ownerId={ownerId}");
            return response?.Items ?? new List<Item>();
        }
        catch
        {
            return new List<Item>();
        }
    }

    // Gets all items NOT owned by the current user (for browsing)
    public async Task<List<Item>> GetAvailableItemsForUserAsync(int userId)
    {
        try
        {
            var allItems = await GetAllAsync();
            return allItems.Where(i => i.OwnerId != userId).ToList();
        }
        catch
        {
            return new List<Item>();
        }
    }

    // Creates a new item via the API
    // Sends categoryId, latitude and longitude as required by the API
    public async Task<Item> CreateAsync(Item item)
    {
        var response = await _httpClient.PostAsJsonAsync("items", new
        {
            title = item.Title,
            description = item.Description,
            dailyRate = item.DailyRate,
            categoryId = item.CategoryId,
            locationName = item.LocationName,
            latitude = item.Latitude,
            longitude = item.Longitude
        });

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"API error {response.StatusCode}: {error}");
        }

        var created = await response.Content.ReadFromJsonAsync<Item>();
        return created!;
    }

    // Updates an existing item via the API
    public async Task UpdateAsync(Item item)
    {
        var response = await _httpClient.PutAsJsonAsync($"items/{item.Id}", new
        {
            title = item.Title,
            description = item.Description,
            dailyRate = item.DailyRate,
            categoryId = item.CategoryId,
            locationName = item.LocationName,
            latitude = item.Latitude,
            longitude = item.Longitude,
            isAvailable = item.IsAvailable
        });

        response.EnsureSuccessStatusCode();
    }

    // Deletes an item by its ID
    public async Task DeleteAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"items/{id}");
        response.EnsureSuccessStatusCode();
    }
}