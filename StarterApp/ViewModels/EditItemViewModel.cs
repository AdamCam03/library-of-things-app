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

    [ObservableProperty]
    private int itemId;

    [ObservableProperty]
    private string itemTitle = string.Empty;

    [ObservableProperty]
    private string itemDescription = string.Empty;

    [ObservableProperty]
    private string itemCategory = string.Empty;

    [ObservableProperty]
    private string locationName = string.Empty;

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
        ItemCategory = item.Category;
        LocationName = item.LocationName;
        ItemDailyRate = item.DailyRate;
    }

    // Saves the updated item to the database
    [RelayCommand]
    private async Task SaveItemAsync()
    {
        if (_authService.CurrentUser == null)
        {
            ErrorMessage = "You must be logged in";
            return;
        }

        var item = new Item
        {
            Id = ItemId,
            Title = ItemTitle,
            Description = ItemDescription,
            Category = ItemCategory,
            LocationName = LocationName,
            DailyRate = ItemDailyRate,
            OwnerId = _authService.CurrentUser.Id
        };

        // Update item in database
        await _itemRepository.UpdateAsync(item);

        // Navigate back to browse item page
        await Shell.Current.GoToAsync("..");
        await Shell.Current.GoToAsync("..");
    }
}