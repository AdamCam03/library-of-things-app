using System.Net.Http.Json;
using StarterApp.Database.Models;
using StarterApp.Database.Data.Repositories;

namespace StarterApp.Services;

public class ApiRentalService : IRentalRepository
{
    private readonly HttpClient _httpClient;

    public ApiRentalService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Rental> CreateAsync(Rental rental)
    {
        var response = await _httpClient.PostAsJsonAsync("rentals", new
        {
            itemId = rental.ItemId,
            startDate = rental.StartDate,
            endDate = rental.EndDate,
            message = rental.Message ?? ""
        });

        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<Rental>();
        return created!;
    }

    public async Task<List<Rental>> GetOutgoingRequestsAsync(int renterId)
    {
        try
        {
            var rentals = await _httpClient.GetFromJsonAsync<List<Rental>>("rentals/outgoing");
            return rentals ?? new List<Rental>();
        }
        catch
        {
            return new List<Rental>();
        }
    }

    public async Task<List<Rental>> GetIncomingRequestsAsync(int ownerId)
    {
        try
        {
            var rentals = await _httpClient.GetFromJsonAsync<List<Rental>>("rentals/incoming");
            return rentals ?? new List<Rental>();
        }
        catch
        {
            return new List<Rental>();
        }
    }

    public async Task<List<Rental>> GetByUserIdAsync(int userId)
    {
        try
        {
            var incoming = await GetIncomingRequestsAsync(userId);
            var outgoing = await GetOutgoingRequestsAsync(userId);
            return incoming.Concat(outgoing)
                           .DistinctBy(r => r.Id)
                           .OrderByDescending(r => r.RequestedAt)
                           .ToList();
        }
        catch
        {
            return new List<Rental>();
        }
    }

    public async Task UpdateAsync(Rental rental)
    {
        var response = await _httpClient.PatchAsJsonAsync($"rentals/{rental.Id}/status", new
        {
            status = rental.Status
        });

        response.EnsureSuccessStatusCode();
    }
}