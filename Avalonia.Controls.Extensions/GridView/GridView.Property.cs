using Avalonia.Collections;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
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
        //Items
        public static readonly DirectProperty<GridView, IEnumerable> ItemsProperty =
         AvaloniaProperty.RegisterDirect<GridView, IEnumerable>(nameof(Items), o => o.Items, (o, v) => o.Items = v);
        private IEnumerable _items = new AvaloniaList<object>();
        [Content]
        public IEnumerable Items
        {
            get { return _items; }
            set
            {
                SetAndRaise(ItemsProperty, ref _items, value);
                UpdateItemView();
            }
        }
        //ItemCount
        public static readonly DirectProperty<GridView, int> ItemCountProperty =
            AvaloniaProperty.RegisterDirect<GridView, int>(nameof(ItemCount), o => o.ItemCount);
        private int _itemCount;
        public int ItemCount
        {
            get => _itemCount;
            private set => SetAndRaise(ItemCountProperty, ref _itemCount, value);
        }
        //ItemTemplate
        public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty =
             AvaloniaProperty.Register<ItemsControl, IDataTemplate>(nameof(ItemTemplate));
        public IDataTemplate ItemTemplate
        {
            get { return GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        //other
        private IItemContainerGenerator _itemContainerGenerator;
        public IItemsPresenter Presenter
        {
            get;
            protected set;
        }
        public IItemContainerGenerator ItemContainerGenerator
        {
            get
            {
                if (_itemContainerGenerator == null)
                {
                    _itemContainerGenerator = CreateItemContainerGenerator();
                    if (_itemContainerGenerator != null)
                    {
                        _itemContainerGenerator.ItemTemplate = ItemTemplate;
                        _itemContainerGenerator.Materialized += (_, e) => OnContainersMaterialized(e);
                        _itemContainerGenerator.Dematerialized += (_, e) => OnContainersDematerialized(e);
                        _itemContainerGenerator.Recycled += (_, e) => OnContainersRecycled(e);
                    }
                }
                return _itemContainerGenerator;
            }
        }
    }
}