using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StarterApp.Database.Data.Repositories;
using StarterApp.Database.Models;
using System.Collections.ObjectModel;
using StarterApp.Views;

namespace StarterApp.ViewModels;

// ViewModel for displaying a list of items
public partial class ItemsListViewModel : BaseViewModel
{
    private readonly IItemRepository _itemRepository;

    [ObservableProperty]
    private ObservableCollection<Item> items = new();

    public ItemsListViewModel(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
        Title = "Browse Items";
    }

  // Loads all items from the repository
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

        var itemList = await _itemRepository.GetAllAsync();

        Items.Clear();

        foreach (var item in itemList)
        {
            Items.Add(item);
        }
    }
    catch (Exception ex)
    {
        ErrorMessage = $"Failed to load items: {ex.Message}";
    }
    finally
    {
        IsBusy = false;
    }
}

    // Navigates to the create item page
    [RelayCommand]
    private async Task GoToCreateItemAsync()
    {
        await Shell.Current.GoToAsync("CreateItemPage");
    }
}