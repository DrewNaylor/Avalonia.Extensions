using Avalonia.Extensions.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using System;
using System.Text;

namespace Avalonia.Extensions.Styles
{
    internal static class AvaloniaThemeXamlLoader
    {
        static AvaloniaThemeXamlLoader()
        {
            Core.Instance.Init();
        }
        public static void ApplyTheme(this StyledElement element, string typeName)
        {
            try
            {
                var sourceUri = new Uri($"avares://Avalonia.Extensions.Controls/Styles/Xaml/{typeName}.xml");
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                using var stream = assets.Open(sourceUri);
                var bytes = new byte[stream.Length];
                stream.Read(bytes);
                var xaml = Encoding.UTF8.GetString(bytes);
                var styles = AvaloniaRuntimeXamlLoader.Parse<Styling.Styles>(xaml);
                element.UpdateStyles(styles);
            }
            catch { }
        }
    }
}