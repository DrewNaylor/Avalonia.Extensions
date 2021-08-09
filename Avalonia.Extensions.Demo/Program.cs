using Avalonia.Extensions.Controls;
using Avalonia.Extensions.Theme;

namespace Avalonia.Controls.Demo
{
    class Program
    {
        public static void Main(string[] args)
        {
            AppBuilder.Configure<App>()
                    .UsePlatformDetect()
                    .UseDoveExtensions()
                    .UseChineseInputSupport()
                    .UseDoveExtensionThemes()
                    .LogToTrace()
            .StartWithClassicDesktopLifetime(args);
        }
    }
}