using Avalonia;
using Avalonia.Controls;
using AlfredGPT.AI;
using AlfredGPT.Chat;
using AlfredGPT.Chat.Plugins;
using AlfredGPT.Common;
using AlfredGPT.Configuration;
using AlfredGPT.Extensions;
using AlfredGPT.Initialization;
using AlfredGPT.Interop;
using AlfredGPT.Windows.Chat.Plugins;
using AlfredGPT.Windows.Configuration;
using AlfredGPT.Windows.Interop;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using SoftwareUpdater = AlfredGPT.Windows.Common.SoftwareUpdater;

namespace AlfredGPT.Windows;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Entrance.Initialize(args);

        ServiceLocator.Build(x => x

                #region Basic

                .AddLogging(builder => builder
                    .AddSerilog(dispose: true)
                    .AddFilter<SerilogLoggerProvider>("Microsoft.EntityFrameworkCore", LogLevel.Warning))
                .AddSingleton<IRuntimeConstantProvider, RuntimeConstantProvider>()
                .AddSingleton<IVisualElementContext, Win32VisualElementContext>()
                .AddSingleton<IHotkeyListener, Win32HotkeyListener>()
                .AddSingleton<INativeHelper, Win32NativeHelper>()
                .AddSingleton<IWindowHelper, Win32WindowHelper>()
                .AddSingleton<ISoftwareUpdater, SoftwareUpdater>()
                .AddSettings()
                .AddWatchdogManager()

                #endregion

                .AddAvaloniaBasicServices()
                .AddViewsAndViewModels()
                .AddDatabaseAndStorage()

                #region Chat Plugins

                .AddTransient<BuiltInChatPlugin, VisualTreePlugin>()
                .AddTransient<BuiltInChatPlugin, WebBrowserPlugin>()
                .AddTransient<BuiltInChatPlugin, FileSystemPlugin>()
                .AddTransient<BuiltInChatPlugin, PowerShellPlugin>()
                .AddTransient<BuiltInChatPlugin, WindowsSystemApiPlugin>()
                .AddTransient<BuiltInChatPlugin, EverythingPlugin>()

                #endregion

                #region Chat

                .AddSingleton<IKernelMixinFactory, KernelMixinFactory>()
                .AddSingleton<IChatPluginManager, ChatPluginManager>()
                .AddSingleton<IChatService, ChatService>()
                .AddChatContextManager()

                #endregion

                #region Initialize

                .AddTransient<IAsyncInitializer, ChatWindowInitializer>()
                .AddTransient<IAsyncInitializer, UpdaterInitializer>()

            #endregion

        );

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args, ShutdownMode.OnExplicitShutdown);
    }

    private static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}