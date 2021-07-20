using Avalonia.Extensions.Controls;

namespace Avalonia.Controls.Demo
{
    class Program
    {
        public static void Main(string[] args)
        {
            AppBuilder.Configure<App>()
                    .UsePlatformDetect()
                    .UseDoveExtensions()
                    .LogToTrace()
            .StartWithClassicDesktopLifetime(args);
        }
    }
}