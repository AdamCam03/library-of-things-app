using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class RentalRequestsPage : ContentPage
{
    private readonly RentalRequestsViewModel _viewModel;

    public RentalRequestsPage(RentalRequestsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Reload rental requests whenever this page appears
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadRentalsCommand.ExecuteAsync(null);
    }
}