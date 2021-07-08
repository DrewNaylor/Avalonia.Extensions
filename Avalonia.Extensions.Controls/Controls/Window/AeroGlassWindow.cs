using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using System;

namespace Avalonia.Extensions.Controls
{
    public class AeroGlassWindow : Window, IStyleable
    {
        Type IStyleable.StyleKey => typeof(Window);
        public AeroGlassWindow()
        {
            ExtendClientAreaToDecorationsHint = true;
            ExtendClientAreaTitleBarHeightHint = -1;
            TransparencyLevelHint = WindowTransparencyLevel.AcrylicBlur;
            this.GetObservable(WindowStateProperty)
                .Subscribe(x =>
                {
                    PseudoClasses.Set(":maximized", x == WindowState.Maximized);
                    PseudoClasses.Set(":fullscreen", x == WindowState.FullScreen);
                });
            this.GetObservable(IsExtendedIntoWindowDecorationsProperty)
                .Subscribe(x =>
                {
                    if (!x)
                    {
                        SystemDecorations = SystemDecorations.Full;
                        TransparencyLevelHint = WindowTransparencyLevel.Blur;
                    }
                });
        }
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            ExtendClientAreaChromeHints = Platform.ExtendClientAreaChromeHints.PreferSystemChrome
                | Platform.ExtendClientAreaChromeHints.OSXThickTitleBar;
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            Icon = default;
            Background = default;
        }
    }
}