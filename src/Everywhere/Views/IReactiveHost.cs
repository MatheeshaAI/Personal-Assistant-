using ShadUI;

namespace AlfredGPT.Views;

public interface IReactiveHost
{
    DialogHost DialogHost { get; }

    ToastHost ToastHost { get; }
}