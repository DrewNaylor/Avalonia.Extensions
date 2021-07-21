using Avalonia.Controls;
using Avalonia.Themes.Fluent;

namespace Avalonia.Extensions.Controls
{
    public static class AppBuilderDesktopExtensions
    {
        public static TAppBuilder UseDoveExtensions<TAppBuilder>(this TAppBuilder builder) where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            Core.Instance.AppAssembly = builder.ApplicationType.Assembly;
            builder.AfterSetup((_) =>
            {
                //ExtThemes.AddTheme();
            });
            return builder;
        }
    }
}