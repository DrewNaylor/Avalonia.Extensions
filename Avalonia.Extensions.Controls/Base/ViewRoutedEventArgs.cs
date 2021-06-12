using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Extensions.Controls
{
    public class ViewRoutedEventArgs : RoutedEventArgs
    {
        public object ClickItem { get; set; }
        public MouseButton ClickMouse { get; }
        public ViewRoutedEventArgs(RoutedEvent args, MouseButton mouseButton) : base(args)
        {
            ClickMouse = mouseButton;
        }
        public ViewRoutedEventArgs(RoutedEvent args, MouseButton mouseButton, object item) : base(args)
        {
            ClickItem = item;
            ClickMouse = mouseButton;
        }
    }
}