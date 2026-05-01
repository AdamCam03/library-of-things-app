using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

// ViewModel for displaying items owned by the logged-in user
public partial class MyItemsViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<Item> items = new();

    public MyItemsViewModel(IItemRepository itemRepository, IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _authService = authService;
        Title = "Your Inventory";
    }

    // Loads items owned by the logged-in user
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
                ErrorMessage = "You must be logged in to view your inventory";
                return;
            }

            var itemList = await _itemRepository.GetByOwnerIdAsync(_authService.CurrentUser.Id);

            Items.Clear();

            foreach (var item in itemList)
            {
                Items.Add(item);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load inventory: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Navigates to create item page
    [RelayCommand]
    private async Task GoToCreateItemAsync()
    {
        await Shell.Current.GoToAsync("CreateItemPage");
    }

    // Navigates to rental requests page
    [RelayCommand]
    private async Task GoToRentalRequestsAsync()
    {
        await Shell.Current.GoToAsync("RentalRequestsPage");
    }

    // Navigates to item detail page
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