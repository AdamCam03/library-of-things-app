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
    private readonly IRentalRepository _rentalRepository;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private int itemId;

    [ObservableProperty]
    private Item? selectedItem;

    [ObservableProperty]
    private bool isOwner;

    [ObservableProperty]
    private bool canRequestRental;

    public ItemDetailViewModel(
        IItemRepository itemRepository,
        IRentalRepository rentalRepository,
        IAuthenticationService authService)
    {
        _itemRepository = itemRepository;
        _rentalRepository = rentalRepository;
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
                CanRequestRental = false;
                return;
            }

            IsOwner = _authService.CurrentUser != null
                      && SelectedItem.OwnerId == _authService.CurrentUser.Id;

            // Users can only request rentals for items they do not own
            CanRequestRental = _authService.CurrentUser != null && !IsOwner;
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

    // Creates a rental request for this item
   [RelayCommand]
private async Task RequestRentalAsync()
{
    if (SelectedItem == null) return;

    if (_authService.CurrentUser == null)
    {
        ErrorMessage = "You must be logged in to request a rental";
        return;
    }

    if (IsOwner)
    {
        ErrorMessage = "You cannot rent your own item";
        return;
    }

    try
    {
        var rental = new Rental
        {
            ItemId = SelectedItem.Id,
            RenterId = _authService.CurrentUser.Id,
            OwnerId = SelectedItem.OwnerId,
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(3),
            Message = "Rental request",
            Status = "Pending"
        };

        await _rentalRepository.CreateAsync(rental);

        await Application.Current!.MainPage!.DisplayAlert(
            "Rental Requested",
            "Your rental request has been submitted.",
            "OK");
    }
    catch (Exception ex)
    {
        ErrorMessage = $"Failed to request rental: {ex.Message}";
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

        await Shell.Current.GoToAsync("..");
    }
}