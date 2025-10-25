using Lucide.Avalonia;

namespace AlfredGPT.Views.Pages;

public partial class CustomAssistantPage : ReactiveUserControl<CustomAssistantPageViewModel>, IMainViewPage
{
    public int Index => 9;
    public DynamicResourceKey Title => new(LocaleKey.CustomAssistantPage_Title);
    public LucideIconKind Icon => LucideIconKind.Bot;

    public CustomAssistantPage()
    {
        InitializeComponent();
    }
}