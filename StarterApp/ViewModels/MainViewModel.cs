/// @file MainViewModel.cs
/// @brief Main dashboard view model for authenticated users
/// @author StarterApp Development Team
/// @date 2025

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Models;
using StarterApp.Services;

namespace StarterApp.ViewModels;

/// @brief View model for the main dashboard page
/// @details Manages the main dashboard display, user information, and navigation to other sections
/// @extends BaseViewModel
public partial class MainViewModel : BaseViewModel
{
    /// @brief Authentication service for managing user authentication
    private readonly IAuthenticationService _authService;
    
    /// @brief Navigation service for managing page navigation
    private readonly INavigationService _navigationService;

    /// @brief The currently authenticated user
    [ObservableProperty]
    private User? currentUser;

    /// @brief Welcome message displayed to the user
    [ObservableProperty]
    private string welcomeMessage = string.Empty;

    /// @brief Indicates whether the current user has admin privileges
    [ObservableProperty]
    private bool isAdmin;

    /// @brief Default constructor for design-time support
    public MainViewModel()
    {
        Title = "Dashboard";
    }
    
    /// @brief Initializes a new instance of the MainViewModel class
    public MainViewModel(IAuthenticationService authService, INavigationService navigationService)
    {
        _authService = authService;
        _navigationService = navigationService;
        Title = "Dashboard";

        LoadUserData();
    }

    /// @brief Loads the current user's data and sets up the dashboard
    private void LoadUserData()
    {
        CurrentUser = _authService.CurrentUser;
        IsAdmin = _authService.HasRole("Admin");
        
        if (CurrentUser != null)
        {
            WelcomeMessage = $"Welcome, {CurrentUser.FullName}!";
        }
    }

    /// @brief Logs out the current user
    [RelayCommand]
    private async Task LogoutAsync()
    {
        var result = await Application.Current.MainPage.DisplayAlert(
            "Logout", 
            "Are you sure you want to logout?", 
            "Yes", 
            "No");

        if (result)
        {
            await _authService.LogoutAsync();
            await _navigationService.NavigateToAsync("LoginPage");
        }
    }

    /// @brief Navigates to the user profile page
    [RelayCommand]
    private async Task NavigateToProfileAsync()
    {
        await _navigationService.NavigateToAsync("TempPage");
    }

    /// @brief Navigates to the settings page
    [RelayCommand]
    private async Task NavigateToSettingsAsync()
    {
        await _navigationService.NavigateToAsync("TempPage");
    }

    /// @brief Navigates to the user list page (admin only)
    [RelayCommand]
    private async Task NavigateToUserListAsync()
    {
        if (!IsAdmin)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Access Denied", 
                "You don't have permission to access admin features.", 
                "OK");
            return;
        }
        
        await _navigationService.NavigateToAsync("UserListPage");
    }

    /// @brief Navigates to the items browsing page
    [RelayCommand]
    private async Task GoToItemsAsync()
    {
        await _navigationService.NavigateToAsync("ItemsListPage");
    }

    /// @brief Navigates to the user's own inventory page
    [RelayCommand]
    private async Task GoToMyItemsAsync()
    {
        await _navigationService.NavigateToAsync("MyItemsPage");
    }

    /// @brief Refreshes the dashboard data
    [RelayCommand]
    private async Task RefreshDataAsync()
    {
        try
        {
            IsBusy = true;
            LoadUserData();
            
            await Task.Delay(1000);
        }
        catch (Exception ex)
        {
            SetError($"Failed to refresh data: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}