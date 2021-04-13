using Avalonia.Collections;
using Avalonia.Controls.Extensions.Controls;
using Avalonia.Controls.Extensions.Utils;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Avalonia.Controls.Extensions
{
    public partial class GridView : ScrollViewer
    {
        static GridView()
        {
            ItemsProperty.Changed.AddClassHandler<GridView>((x, e) => x.ItemsChanged(e));
            ColumnNumProperty.Changed.AddClassHandler<GridView>((x, e) => x.ColumnNumChanged(e));
            ItemTemplateProperty.Changed.AddClassHandler<GridView>((x, e) => x.ItemTemplateChanged(e));
        }
        private void ItemTemplateChanged(AvaloniaPropertyChangedEventArgs e)
        {
            Grid?.ItemTemplateChanged(e);
        }
        private void ColumnNumChanged(AvaloniaPropertyChangedEventArgs e)
        {
            Grid?.ColumnNumChanged(e);
        }
        private void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            UpdateItemCount();
            Grid?.ItemsChanged(e);
        }
        private GridViewItem Grid { get; }
        public GridView()
        {
            Grid = new GridViewItem();
            Content = Grid;
        }
        private static readonly FuncTemplate<IPanel> DefaultPanel =
          new FuncTemplate<IPanel>(() => new StackPanel());
        public static readonly StyledProperty<ITemplate<IPanel>> ItemsPanelProperty =
             AvaloniaProperty.Register<GridView, ITemplate<IPanel>>(nameof(ItemsPanel), DefaultPanel);
        public ITemplate<IPanel> ItemsPanel
        {
            get { return GetValue(ItemsPanelProperty); }
            set { SetValue(ItemsPanelProperty, value); }
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
             AvaloniaProperty.Register<GridView, IDataTemplate>(nameof(ItemTemplate));
        public IDataTemplate ItemTemplate
        {
            get { return GetValue(ItemTemplateProperty); }
            set => SetValue(ItemTemplateProperty, value);
        }
        //ColumnNum
        public static readonly StyledProperty<int> ColumnNumProperty =
            AvaloniaProperty.Register<GridView, int>(nameof(ColumnNum));
        public int ColumnNum
        {
            get { return GetValue(ColumnNumProperty); }
            set { SetValue(ColumnNumProperty, value); }
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
        //Items
        public static readonly DirectProperty<GridView, IEnumerable> ItemsProperty =
         AvaloniaProperty.RegisterDirect<GridView, IEnumerable>(nameof(Items), o => o.Items, (o, v) => o.Items = v);
        private IEnumerable _items = new AvaloniaList<object>();
        [Content]
        public IEnumerable Items
        {
            get => _items;
            set => SetAndRaise(ItemsProperty, ref _items, value);
        }
        private void UpdateItemCount()
        {
            if (Items == null)
                ItemCount = 0;
            else if (Items is IList list)
                ItemCount = list.Count;
            else
                ItemCount = Items.Count();
        }
        public void OnContentClick(ClickablePanel clickablePanel)
        {
            if (clickablePanel != null)
            {

            }
        }
    }
}