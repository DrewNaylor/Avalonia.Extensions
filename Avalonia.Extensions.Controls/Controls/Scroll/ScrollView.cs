using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace Avalonia.Extensions.Controls
{
    public class ScrollView : ScrollViewer
    {
        private double lastSize = -1;
        /// <summary>
        /// Defines the <see cref="ScrollTop"/> event.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> ScrollTopEvent =
           RoutedEvent.Register<ScrollView, RoutedEventArgs>(nameof(ScrollTop), RoutingStrategies.Bubble);
        public event EventHandler<RoutedEventArgs> ScrollTop
        {
            add { AddHandler(ScrollTopEvent, value); }
            remove { RemoveHandler(ScrollTopEvent, value); }
        }
        /// <summary>
        /// Defines the <see cref="ScrollEnd"/> event.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> ScrollEndEvent =
           RoutedEvent.Register<ScrollView, RoutedEventArgs>(nameof(ScrollEnd), RoutingStrategies.Bubble);
        public event EventHandler<RoutedEventArgs> ScrollEnd
        {
            add { AddHandler(ScrollEndEvent, value); }
            remove { RemoveHandler(ScrollEndEvent, value); }
        }
        protected override void OnScrollChanged(ScrollChangedEventArgs e)
        {
            base.OnScrollChanged(e);
            if (e.Source is ScrollViewer scrollViewer && scrollViewer.Offset.Y > 0)
            {
                if (Content is Control child)
                {
                    if (scrollViewer.Offset.Y == 0 && lastSize != 0)
                    {
                        var args = new RoutedEventArgs(ScrollTopEvent);
                        RaiseEvent(args);
                        if (!args.Handled)
                            args.Handled = true;
                    }
                    else if ((scrollViewer.Offset.Y + Bounds.Height) >= (child.Bounds.Height * 0.8))
                    {
                        var args = new RoutedEventArgs(ScrollEndEvent);
                        RaiseEvent(args);
                        if (!args.Handled)
                            args.Handled = true;
                    }
                    lastSize = scrollViewer.Offset.Y;
                }
            }
        }
    }
}