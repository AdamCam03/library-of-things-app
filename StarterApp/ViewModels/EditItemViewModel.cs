using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

// ViewModel for editing an existing item
[QueryProperty(nameof(ItemId), "ItemId")]
public partial class EditItemViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authService;

    // Maps category names to their API IDs
    private static readonly Dictionary<string, int> CategoryIds = new()
    {
        { "Tools", 1 },
        { "Garden", 2 },
        { "Camping", 3 },
        { "Sports", 4 },
        { "Electronics", 5 },
        { "Games", 6 },
        { "DIY", 7 },
        { "Cycling", 8 },
        { "Music", 9 },
        { "Outdoors", 10 }
    };

    // Maps location names to their coordinates [latitude, longitude]
    private static readonly Dictionary<string, double[]> LocationCoords = new()
    {
        { "Edinburgh", new[] { 55.9533, -3.1883 } },
        { "Glasgow", new[] { 55.8617, -4.2583 } },
        { "Paisley", new[] { 55.8456, -4.4239 } }
    };

    [ObservableProperty]
    private int itemId;

    [ObservableProperty]
    private string itemTitle = string.Empty;

    [ObservableProperty]
    private string itemDescription = string.Empty;

    [ObservableProperty]
    private string selectedCategory = "Tools";

    [ObservableProperty]
    private string selectedLocation = "Edinburgh";

    [ObservableProperty]
    private decimal itemDailyRate;

    public EditItemViewModel(IItemRepository itemRepository, IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _authService = authService;
        Title = "Edit Item";
    }

    // Loads the item when ItemId is passed via navigation
    partial void OnItemIdChanged(int value)
    {
        _ = LoadItemAsync(value);
    }

    // Retrieves the item and populates the edit fields
    private async Task LoadItemAsync(int id)
    {
        var item = await _itemRepository.GetByIdAsync(id);

        if (item == null)
        {
            ErrorMessage = "Item not found";
            return;
        }

        // Ensure only the owner can edit the item
        if (_authService.CurrentUser == null || item.OwnerId != _authService.CurrentUser.Id)
        {
            ErrorMessage = "You do not have permission to edit this item";
            return;
        }

        ItemTitle = item.Title;
        ItemDescription = item.Description;
        SelectedCategory = item.Category;
        SelectedLocation = item.LocationName ?? "Edinburgh";
        ItemDailyRate = item.DailyRate;
    }

    // Saves the updated item via the API
    [RelayCommand]
    private async Task SaveItemAsync()
    {
        if (_authService.CurrentUser == null)
        {
            ErrorMessage = "You must be logged in";
            return;
        }

        // Map selected category name to its API ID
        var categoryId = CategoryIds.TryGetValue(SelectedCategory ?? "Tools", out var id) ? id : 1;

        // Map selected location to coordinates
        var coords = LocationCoords.TryGetValue(SelectedLocation ?? "Edinburgh", out var loc)
            ? loc
            : new[] { 55.9533, -3.1883 };

        var item = new Item
        {
            Id = ItemId,
            Title = ItemTitle,
            Description = ItemDescription,
            Category = SelectedCategory ?? "Tools",
            CategoryId = categoryId,
            LocationName = SelectedLocation ?? "Edinburgh",
            Latitude = coords[0],
            Longitude = coords[1],
            DailyRate = ItemDailyRate,
            OwnerId = _authService.CurrentUser.Id
        };

        try
        {
            await _itemRepository.UpdateAsync(item);
            await Shell.Current.GoToAsync("..");
            await Shell.Current.GoToAsync("..");
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}