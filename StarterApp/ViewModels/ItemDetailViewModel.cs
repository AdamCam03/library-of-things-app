using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

// ViewModel for showing details of a selected item
[QueryProperty(nameof(ItemId), "ItemId")]
public partial class ItemDetailViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private int itemId;

    [ObservableProperty]
    private Item? selectedItem;

    [ObservableProperty]
    private bool isOwner;

    public ItemDetailViewModel(IItemRepository itemRepository, IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _authService = authService;
        Title = "Item Details";
    }

    // ItemId is set from navigation
    // Loading is handled by ItemDetailPage.OnAppearing
    partial void OnItemIdChanged(int value)
    {
    }

    // Loads the selected item from the repository
    [RelayCommand]
    private async Task LoadItemAsync(int id)
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = string.Empty;

            SelectedItem = await _itemRepository.GetByIdAsync(id);

            if (SelectedItem == null)
            {
                ErrorMessage = "Item not found";
                IsOwner = false;
                return;
            }

            // Checks whether the logged-in user owns this item
            IsOwner = _authService.CurrentUser != null
                      && SelectedItem.OwnerId == _authService.CurrentUser.Id;
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

    // Navigates to edit item page
    [RelayCommand]
    private async Task GoToEditItemAsync()
    {
        if (SelectedItem == null)
        {
            return;
        }

        await Shell.Current.GoToAsync($"EditItemPage?ItemId={SelectedItem.Id}");
    }

    // Deletes the selected item if the logged-in user owns it
    [RelayCommand]
    private async Task DeleteItemAsync()
    {
        if (SelectedItem == null)
        {
            return;
        }

        if (!IsOwner)
        {
            ErrorMessage = "You do not have permission to delete this item";
            return;
        }

        bool confirm = await Application.Current.MainPage.DisplayAlert(
            "Delete Item",
            "Are you sure you want to delete this item?",
            "Delete",
            "Cancel");

        if (!confirm)
        {
            return;
        }

        await _itemRepository.DeleteAsync(SelectedItem.Id);

        // Return to browse items after deleting
        await Shell.Current.GoToAsync("..");
    }
}