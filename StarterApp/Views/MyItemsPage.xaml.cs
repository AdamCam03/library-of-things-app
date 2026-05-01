using StarterApp.Database.Models;
using StarterApp.ViewModels;

namespace StarterApp.Views;

public partial class MyItemsPage : ContentPage
{
    private readonly MyItemsViewModel _viewModel;

    public MyItemsPage(MyItemsViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    /// @brief Reload items whenever this page appears
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.LoadItemsCommand.ExecuteAsync(null);
    }

    /// @brief Opens the item detail page when a user selects an item
    /// @param sender The source of the event
    /// @param args Contains information about the selection change
    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs args)
    {
        if (args.CurrentSelection.FirstOrDefault() is Item item)
        {
            await Shell.Current.GoToAsync($"ItemDetailPage?ItemId={item.Id}");
        }
    }
}