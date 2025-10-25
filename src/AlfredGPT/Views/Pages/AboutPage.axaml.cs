using Lucide.Avalonia;

namespace AlfredGPT.Views.Pages;

public partial class AboutPage : ReactiveUserControl<AboutPageViewModel>, IMainViewPage
{
    public int Index => int.MaxValue;

    public DynamicResourceKey Title => new(LocaleKey.AboutPage_Title);

    public LucideIconKind Icon => LucideIconKind.Info;

    public AboutPage()
    {
        InitializeComponent();
    }
}