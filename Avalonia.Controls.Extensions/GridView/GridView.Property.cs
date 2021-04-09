using Avalonia.Collections;
using Avalonia.Controls.Extensions.Presenters;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using System;
using System.Collections;

namespace Avalonia.Controls.Extensions
{
    public partial class GridView
    {
        private static readonly FuncTemplate<IPanel> DefaultPanel =
          new FuncTemplate<IPanel>(() => new StackPanel());
        //ItemClick
        public static readonly RoutedEvent<RoutedEventArgs> ItemClickEvent =
            RoutedEvent.Register<Button, RoutedEventArgs>(nameof(ItemClick), RoutingStrategies.Bubble);
        public event EventHandler<RoutedEventArgs> ItemClick
        {
            add { AddHandler(ItemClickEvent, value); }
            remove { RemoveHandler(ItemClickEvent, value); }
        }
        //Scroll
        public static readonly DirectProperty<GridView, IScrollable> ScrollProperty =
            AvaloniaProperty.RegisterDirect<GridView, IScrollable>(nameof(Scroll), o => o.Scroll);
        private IScrollable _scroll;
        public IScrollable Scroll
        {
            get { return _scroll; }
            private set { SetAndRaise(ScrollProperty, ref _scroll, value); }
        }
        //ItemsPanel
        public static readonly StyledProperty<ITemplate<IPanel>> ItemsPanelProperty =
             AvaloniaProperty.Register<ItemsControl, ITemplate<IPanel>>(nameof(ItemsPanel), DefaultPanel);
        public ITemplate<IPanel> ItemsPanel
        {
            get { return GetValue(ItemsPanelProperty); }
            set { SetValue(ItemsPanelProperty, value); }
        }
        //ColumnNum
        public static readonly DirectProperty<GridView, int> ColumnNumProperty =
            AvaloniaProperty.RegisterDirect<GridView, int>(nameof(ColumnNum), o => o.ColumnNum, (o, v) => o.ColumnNum = v);
        public int ColumnNum
        {
            get { return GetValue(ColumnNumProperty); }
            set { SetValue(ColumnNumProperty, value); }
        }
    }
}