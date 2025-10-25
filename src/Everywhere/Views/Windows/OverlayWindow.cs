﻿using Avalonia.Controls;
using AlfredGPT.Common;
using AlfredGPT.Interop;
using Serilog;

namespace AlfredGPT.Views;

public class OverlayWindow : Window
{
    private readonly WindowBase? _owner;

    public OverlayWindow(WindowBase? owner = null)
    {
        _owner = owner;

        CanResize = false;
        ShowInTaskbar = false;
        ShowActivated = false;
        SystemDecorations = SystemDecorations.None;
        TransparencyLevelHint = [WindowTransparencyLevel.Transparent];
        Background = null;

        var windowHelper = ServiceLocator.Resolve<IWindowHelper>();
        windowHelper.SetFocusable(this, false);
        windowHelper.SetHitTestVisible(this, false);
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if (e.CloseReason != WindowCloseReason.WindowClosing) e.Cancel = true;
        base.OnClosing(e);
    }

    public async void UpdateForVisualElement(IVisualElement? element)
    {
        try
        {
            if (element == null)
            {
                Hide();
            }
            else
            {
                Show();
                var boundingRectangle = await Task.Run(() => element.BoundingRectangle).WaitAsync(TimeSpan.FromSeconds(0.5));
                Position = new PixelPoint(boundingRectangle.X, boundingRectangle.Y);
                var scaling = DesktopScaling;
                Width = boundingRectangle.Width / scaling;
                Height = boundingRectangle.Height / scaling;

                if (_owner is { Topmost: true })
                {
                    _owner.Topmost = false;
                    _owner.Topmost = true;
                }
            }
        }
        catch (TimeoutException) { }
        catch (Exception ex)
        {
            Log.Logger.ForContext<OverlayWindow>().Error(ex, "Failed to update OverlayWindow for visual element.");
        }
    }
}