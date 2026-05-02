using System.Net.Http.Json;
using StarterApp.Database.Models;
using StarterApp.Database.Data.Repositories;

namespace StarterApp.Services;

// API-backed implementation of IRentalRepository
public class ApiRentalService : IRentalRepository
{
    private readonly HttpClient _httpClient;

    // Constructor - injects the shared HttpClient which already has the JWT token set
    public ApiRentalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Response wrapper for the rentals endpoint
    // The API returns { "rentals": [...] } not just [...]
    private class RentalsResponse
    {
        public List<Rental> Rentals { get; set; } = new();
    }

    // Creates a new rental request via the API
    public async Task<Rental> CreateAsync(Rental rental)
    {
        var response = await _httpClient.PostAsJsonAsync("rentals", new
        {
            itemId = rental.ItemId,
            startDate = rental.StartDate.ToString("yyyy-MM-dd"),
            endDate = rental.EndDate.ToString("yyyy-MM-dd"),
            message = rental.Message ?? ""
        });

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"API error {response.StatusCode}: {error}");
        }

        var created = await response.Content.ReadFromJsonAsync<Rental>();
        return created!;
    }

    // Gets rental requests made by the current user
    public async Task<List<Rental>> GetOutgoingRequestsAsync(int renterId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<RentalsResponse>("rentals/outgoing");
            return response?.Rentals ?? new List<Rental>();
        }
        catch
        {
            return new List<Rental>();
        }
    }

    // Gets rental requests received by the current user as owner
    public async Task<List<Rental>> GetIncomingRequestsAsync(int ownerId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<RentalsResponse>("rentals/incoming");
            return response?.Rentals ?? new List<Rental>();
        }
        catch
        {
            return new List<Rental>();
        }
    }

    // Gets all rentals where user is either renter or owner
    public async Task<List<Rental>> GetByUserIdAsync(int userId)
    {
        try
        {
            var incoming = await GetIncomingRequestsAsync(userId);
            var outgoing = await GetOutgoingRequestsAsync(userId);
            return incoming.Concat(outgoing)
                           .DistinctBy(rental => rental.Id)
                           .OrderByDescending(rental => rental.RequestedAt)
                           .ToList();
        }
        catch
        {
            return new List<Rental>();
        }
    }

    // Updates a rental status via the API
    public async Task UpdateAsync(Rental rental)
    {
        var response = await _httpClient.PatchAsJsonAsync($"rentals/{rental.Id}/status", new
        {
            status = rental.Status
        });

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"API error {response.StatusCode}: {error}");
        }
    }
}