using Microsoft.Extensions.Logging;
using StarterApp.ViewModels;
using StarterApp.Database.Data;
using StarterApp.Views;
using StarterApp.Services;
using StarterApp.Database.Data.Repositories;

namespace StarterApp;

public static class MauiProgram
{
    // Toggle this to switch between local DB and shared API
    // Set to true to use the API
    const bool useSharedApi = true;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        if (useSharedApi)
        {
            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://set09102-api.b-davison.workers.dev/")
            };
            builder.Services.AddSingleton(httpClient);
            builder.Services.AddSingleton<IAuthenticationService, ApiAuthenticationService>();
            builder.Services.AddSingleton<IItemRepository, ApiItemService>();
            builder.Services.AddSingleton<IRentalRepository, ApiRentalService>();
        }
        else
        {
            builder.Services.AddDbContext<AppDbContext>();
            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddScoped<IItemRepository, ItemRepository>();
            builder.Services.AddScoped<IRentalRepository, RentalRepository>();
        }

        builder.Services.AddSingleton<INavigationService, NavigationService>();

        builder.Services.AddSingleton<AppShellViewModel>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<App>();

        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddSingleton<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<UserListViewModel>();
        builder.Services.AddTransient<UserListPage>();
        builder.Services.AddTransient<UserDetailPage>();
        builder.Services.AddTransient<UserDetailViewModel>();
        builder.Services.AddSingleton<TempViewModel>();
        builder.Services.AddTransient<TempPage>();
        builder.Services.AddTransient<ItemsListViewModel>();
        builder.Services.AddTransient<ItemsListPage>();
        builder.Services.AddTransient<CreateItemViewModel>();
        builder.Services.AddTransient<CreateItemPage>();
        builder.Services.AddTransient<ItemDetailViewModel>();
        builder.Services.AddTransient<ItemDetailPage>();
        builder.Services.AddTransient<EditItemViewModel>();
        builder.Services.AddTransient<EditItemPage>();
        builder.Services.AddTransient<MyItemsViewModel>();
        builder.Services.AddTransient<MyItemsPage>();
        builder.Services.AddTransient<RentalRequestsViewModel>();
        builder.Services.AddTransient<RentalRequestsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}