using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

// ViewModel for creating a new item listing
public partial class CreateItemViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authService;

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

    public CreateItemViewModel(IItemRepository itemRepository, IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _authService = authService;
        Title = "List an Item";
    }

    // Creates a new item and saves it to the database
    [RelayCommand]
    private async Task CreateItemAsync()
    {
        if (string.IsNullOrWhiteSpace(ItemTitle))
        {
            ErrorMessage = "Title is required";
            return;
        }

        if (ItemDailyRate <= 0)
        {
            ErrorMessage = "Daily rate must be more than 0";
            return;
        }

        if (_authService.CurrentUser == null)
        {
            ErrorMessage = "You must be logged in to list an item";
            return;
        }

        var item = new Item
        {
            Title = ItemTitle,
            Description = ItemDescription,
            Category = ItemCategory,
            LocationName = LocationName,
            DailyRate = ItemDailyRate,
            OwnerId = _authService.CurrentUser.Id
        };

        await _itemRepository.CreateAsync(item);

        await Shell.Current.GoToAsync("..");
    }
}