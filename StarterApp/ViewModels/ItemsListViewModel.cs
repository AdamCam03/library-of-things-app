using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

// ViewModel for displaying items available to rent from other users
public partial class ItemsListViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<Item> items = new();

    public ItemsListViewModel(IItemRepository itemRepository, IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _authService = authService;
        Title = "Browse Items";
    }

    // Loads items not owned by the logged-in user
    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            if (_authService.CurrentUser == null)
            {
                ErrorMessage = "You must be logged in to browse items";
                return;
            }

            var itemList = await _itemRepository.GetAvailableItemsForUserAsync(_authService.CurrentUser.Id);

            Items.Clear();

            foreach (var item in itemList)
            {
                Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load items: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Navigates to the create item page
    [RelayCommand]
    private async Task GoToCreateItemAsync()
    {
        await Shell.Current.GoToAsync("CreateItemPage");
    }

    // Navigates to the rental requests page
    [RelayCommand]
    private async Task GoToRentalRequestsAsync()
    {
        await Shell.Current.GoToAsync("RentalRequestsPage");
    }

    // Navigates to the item detail page when an item is selected
    [RelayCommand]
    private async Task GoToItemDetailAsync(Item item)
    {
        if (item == null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"ItemDetailPage?ItemId={item.Id}");
    }
}