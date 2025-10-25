namespace AlfredGPT.Views;

public partial class WelcomeView : ReactiveUserControl<WelcomeViewModel>
{
    public WelcomeView()
    {
        InitializeComponent();
        ViewModel.ApiKeyValidated += () => ConfettiEffect.Start();
    }
}