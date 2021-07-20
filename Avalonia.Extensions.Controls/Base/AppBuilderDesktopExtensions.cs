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
                for (var idx = 0; idx < Application.Current.Styles.Count; idx++)
                {
                    if (Application.Current.Styles.ElementAt(idx) is FluentTheme fluentTheme)
                        Themes.AddTheme(fluentTheme.Mode);
                }
            });
            return builder;
        }
    }
}