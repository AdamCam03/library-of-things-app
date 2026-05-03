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
    // Separates incoming and outgoing rentals and marks them accordingly
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

            // Get incoming and outgoing rentals separately
            var incoming = await _rentalRepository.GetIncomingRequestsAsync(_authService.CurrentUser.Id);
            var outgoing = await _rentalRepository.GetOutgoingRequestsAsync(_authService.CurrentUser.Id);

            // Mark incoming rentals so the UI can show Accept/Reject buttons
            foreach (var rental in incoming)
            {
                rental.IsIncoming = true;
            }

            // Combine and sort by most recent first
            var allRentals = incoming.Concat(outgoing)
                .OrderByDescending(rental => rental.CreatedAt)
                .ToList();

            Rentals.Clear();

            foreach (var rental in allRentals)
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