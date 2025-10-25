using Lucide.Avalonia;

namespace AlfredGPT.Views.Pages;

public partial class ChatPluginPage : ReactiveUserControl<ChatPluginPageViewModel>, IMainViewPage
{
    public int Index => 10;

    public DynamicResourceKey Title => new(LocaleKey.ChatPluginPage_Title);

    public LucideIconKind Icon => LucideIconKind.Hammer;

    public ChatPluginPage()
    {
        InitializeComponent();
    }
}