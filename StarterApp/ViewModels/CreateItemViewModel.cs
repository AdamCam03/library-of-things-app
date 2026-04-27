using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.ViewModels;

// ViewModel for creating a new item listing
public partial class CreateItemViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;

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

    public CreateItemViewModel(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
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

        var item = new Item
        {
            Title = ItemTitle,
            Description = ItemDescription,
            Category = ItemCategory,
            LocationName = LocationName,
            DailyRate = ItemDailyRate,
            OwnerId = 1
        };

        await _itemRepository.CreateAsync(item);

        await Shell.Current.GoToAsync("..");
    }
}