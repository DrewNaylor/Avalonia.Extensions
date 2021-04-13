namespace Avalonia.Controls.Demo
{
    class Program
    {
        public static void Main(string[] args)
        {
            AppBuilder.Configure<App>()
                    .UsePlatformDetect()
                    .LogToTrace()
            .StartWithClassicDesktopLifetime(args);
        }
    }
}