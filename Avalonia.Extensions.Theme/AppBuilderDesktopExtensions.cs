using Avalonia.Controls;

namespace Avalonia.Extensions.Theme
{
    public static class AppBuilderDesktopExtensions
    {
        public static TAppBuilder UseDoveExtensionThemes<TAppBuilder>(this TAppBuilder builder) where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            builder.AfterSetup((_) =>
            {
                Application.Current.Styles.Add(new ControlsTheme());
            });
            return builder;
        }
    }
}