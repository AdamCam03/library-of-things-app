using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using StarterApp.Services;
using System.Collections.ObjectModel;

namespace StarterApp.ViewModels;

// ViewModel for displaying rental requests
public partial class RentalRequestsViewModel : BaseViewModel
{
    private readonly IRentalRepository _rentalRepository;
    private readonly IAuthenticationService _authService;

    [ObservableProperty]
    private ObservableCollection<Rental> rentals = new();

    public RentalRequestsViewModel(
        IRentalRepository rentalRepository,
        IAuthenticationService authService)
    {
        _rentalRepository = rentalRepository;
        _authService = authService;
        Title = "Rental Requests";
    }

    // Load all rental requests related to the user
    [RelayCommand]
    private async Task LoadRentalsAsync()
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
                ErrorMessage = "You must be logged in";
                return;
            }

            // Get rentals where user is either owner or renter
            var rentalList = await _rentalRepository.GetByUserIdAsync(_authService.CurrentUser.Id);

            Rentals.Clear();

            foreach (var rental in rentalList)
            {
                Rentals.Add(rental);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to load rentals: {ex.Message}";
        }
        finally
        {
            IsBusy = false;
        }
    }

    // Approve rental request (owner only)
    [RelayCommand]
    private async Task AcceptRentalAsync(Rental rental)
    {
        if (rental == null)
        {
            return;
        }

        try
        {
            rental.Status = "Approved";
            await _rentalRepository.UpdateAsync(rental);
            await LoadRentalsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to approve rental: {ex.Message}";
        }
    }

    // Reject rental request (owner only)
    [RelayCommand]
    private async Task RejectRentalAsync(Rental rental)
    {
        if (rental == null)
        {
            return;
        }

        try
        {
            rental.Status = "Rejected";
            await _rentalRepository.UpdateAsync(rental);
            await LoadRentalsAsync();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Failed to reject rental: {ex.Message}";
        }
    }
}