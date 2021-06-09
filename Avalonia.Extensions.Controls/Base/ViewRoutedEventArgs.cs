using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Extensions.Controls
{
    public class ViewRoutedEventArgs : RoutedEventArgs
    {
        public MouseButton ClickMouse { get; }
        public ViewRoutedEventArgs(RoutedEvent args, MouseButton mouseButton) : base(args)
        {
            ClickMouse = mouseButton;
        }
    }
}