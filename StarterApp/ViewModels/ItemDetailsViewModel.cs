using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;

namespace StarterApp.ViewModels;

// ViewModel for showing details of a selected item
[QueryProperty(nameof(ItemId), "ItemId")]
public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;

    [ObservableProperty]
    private int itemId;

    [ObservableProperty]
    private Item? selectedItem;

    public ItemDetailViewModel(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
        Title = "Item Details";
    }

    // Loads item details when ItemId is passed through
    partial void OnItemIdChanged(int value)
    {
        _ = LoadItemAsync(value);
    }

    private async Task LoadItemAsync(int id)
    {
        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            SelectedItem = await _itemRepository.GetByIdAsync(id);

            if (SelectedItem == null)
            {
                ErrorMessage = "Item not found";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load item: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }
}