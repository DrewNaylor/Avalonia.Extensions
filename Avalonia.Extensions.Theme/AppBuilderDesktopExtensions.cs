using Avalonia.Controls;
using Avalonia.Extensions.Controls;

namespace Avalonia.Extensions.Theme
{
    public static class AppBuilderDesktopExtensions
    {
        public static TAppBuilder UseDoveExtensionThemes<TAppBuilder>(this TAppBuilder builder) where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            builder.AfterSetup((_) =>
            {
                Application.Current.Resources.Add("Dove.ThemeDictionary", "avares://Avalonia.Extensions.Theme/ThemeDictionary.xaml".AsResource());
                Application.Current.Resources.MergedDictionaries.Add("avares://Avalonia.Extensions.Theme/ThemeDictionary.xaml".AsStyle());
            });
            return builder;
        }
    }
}