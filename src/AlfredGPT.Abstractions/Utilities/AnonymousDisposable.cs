﻿using System.ComponentModel;

namespace AlfredGPT.Utilities;

public class AnonymousDisposable(Action disposeAction) : IDisposable
{
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        disposeAction();
    }

    public static AnonymousDisposable FromNotifyPropertyChanged(INotifyPropertyChanged source, PropertyChangedEventHandler handler)
    {
        source.PropertyChanged += handler;
        return new AnonymousDisposable(() => source.PropertyChanged -= handler);
    }
}