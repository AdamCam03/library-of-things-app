using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class ItemDetailPage : ContentPage
{
    private readonly ItemDetailViewModel _viewModel;

    public ItemDetailPage(ItemDetailViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Reload item details whenever this page appears
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (_viewModel.ItemId > 0)
        {
            await _viewModel.LoadItemCommand.ExecuteAsync(_viewModel.ItemId);
        }
    }
}