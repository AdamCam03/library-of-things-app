using StarterApp.Database.Models;
using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class ItemsListPage : ContentPage
{
    private readonly ItemsListViewModel _viewModel;

    public ItemsListPage(ItemsListViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    // Reload items whenever this page appears
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadItemsCommand.ExecuteAsync(null);
    }

    // Opens the detail page when an item is selected
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Item item)
        {
            await Shell.Current.GoToAsync($"ItemDetailPage?ItemId={item.Id}");
        }
    }
}