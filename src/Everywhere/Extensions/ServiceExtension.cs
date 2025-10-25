using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using AlfredGPT.Common;
using AlfredGPT.Configuration;
using AlfredGPT.Database;
using AlfredGPT.Storage;
using AlfredGPT.Views;
using AlfredGPT.Views.Pages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AlfredGPT.Extensions;

public static class ServiceExtension
{
    public static IServiceCollection AddAvaloniaBasicServices(this IServiceCollection services) =>
        services
            .AddDialogManagerAndToastManager()
            .AddTransient<IClipboard>(_ =>
                Application.Current.As<App>()?.TopLevel.Clipboard ??
                throw new InvalidOperationException("Clipboard is not available."))
            .AddTransient<IStorageProvider>(_ =>
                Application.Current.As<App>()?.TopLevel.StorageProvider ??
                throw new InvalidOperationException("StorageProvider is not available."))
            .AddTransient<ILauncher>(_ =>
                Application.Current.As<App>()?.TopLevel.Launcher ??
                throw new InvalidOperationException("Launcher is not available."));

    public static IServiceCollection AddViewsAndViewModels(this IServiceCollection services) =>
        services
            .AddSingleton<VisualTreeDebugger>()
            .AddSingleton<ChatWindowViewModel>()
            .AddSingleton<ChatWindow>()
            .AddTransient<IMainViewPageFactory, SettingsCategoryPageFactory>()
            .AddSingleton<CustomAssistantPageViewModel>()
            .AddSingleton<IMainViewPage, CustomAssistantPage>()
            .AddSingleton<ChatPluginPageViewModel>()
            .AddSingleton<IMainViewPage, ChatPluginPage>()
            .AddSingleton<AboutPageViewModel>()
            .AddSingleton<IMainViewPage, AboutPage>()
            .AddTransient<WelcomeViewModel>()
            .AddTransient<WelcomeView>()
            .AddTransient<ChangeLogViewModel>()
            .AddTransient<ChangeLogView>()
            .AddSingleton<MainViewModel>()
            .AddSingleton<MainView>();

    public static IServiceCollection AddDatabaseAndStorage(this IServiceCollection services) =>
        services
            .AddDbContextFactory<ChatDbContext>((x, options) =>
            {
                var dbPath = x.GetRequiredService<IRuntimeConstantProvider>().GetDatabasePath("chat.db");
                options.UseSqlite($"Data Source={dbPath}");
            })
            .AddSingleton<IBlobStorage, BlobStorage>()
            .AddSingleton<IChatContextStorage, ChatContextStorage>()
            .AddTransient<IAsyncInitializer, ChatDbInitializer>();
}