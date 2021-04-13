using Avalonia.Collections;
using Avalonia.Controls.Extensions.Utils;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Metadata;
using System.Collections;
using System.Collections.Specialized;

namespace Avalonia.Controls.Extensions
{
    public class GridViewItem : Grid, ICollectionChangedListener
    {
        private ViewManager viewManager;
        internal int LastAncor { get; set; } = 0;
        static GridViewItem()
        {
            ItemsProperty.Changed.AddClassHandler<GridViewItem>((x, e) => x.ItemsChanged(e));
            ColumnNumProperty.Changed.AddClassHandler<GridViewItem>((x, e) => x.ColumnNumChanged(e));
            ItemTemplateProperty.Changed.AddClassHandler<GridViewItem>((x, e) => x.ItemTemplateChanged(e));
        }
        public GridViewItem()
        {
            PseudoClasses.Add(":empty");
            SubscribeToItems(_items);
            HorizontalAlignment = Layout.HorizontalAlignment.Stretch;
            viewManager = new ViewManager(this);
        }
        private static readonly FuncTemplate<IPanel> DefaultPanel =
          new FuncTemplate<IPanel>(() => new StackPanel());
        public static readonly StyledProperty<ITemplate<IPanel>> ItemsPanelProperty =
             AvaloniaProperty.Register<ItemsControl, ITemplate<IPanel>>(nameof(ItemsPanel), DefaultPanel);
        public ITemplate<IPanel> ItemsPanel
        {
            get { return GetValue(ItemsPanelProperty); }
            set { SetValue(ItemsPanelProperty, value); }
        }
        //ItemCount
        public static readonly DirectProperty<GridViewItem, int> ItemCountProperty =
            AvaloniaProperty.RegisterDirect<GridViewItem, int>(nameof(ItemCount), o => o.ItemCount);
        private int _itemCount;
        public int ItemCount
        {
            get => _itemCount;
            private set => SetAndRaise(ItemCountProperty, ref _itemCount, value);
        }
        //Items
        public static readonly DirectProperty<GridViewItem, IEnumerable> ItemsProperty =
         AvaloniaProperty.RegisterDirect<GridViewItem, IEnumerable>(nameof(Items), o => o.Items, (o, v) => o.Items = v);
        private IEnumerable _items = new AvaloniaList<object>();
        [Content]
        public IEnumerable Items
        {
            get => _items;
            set => SetAndRaise(ItemsProperty, ref _items, value);
        }
        //ColumnNum
        public static readonly StyledProperty<int> ColumnNumProperty =
            AvaloniaProperty.Register<GridViewItem, int>(nameof(ColumnNum));
        public int ColumnNum
        {
            get { return GetValue(ColumnNumProperty); }
            set { SetValue(ColumnNumProperty, value); }
        }
        public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty =
             AvaloniaProperty.Register<ItemsControl, IDataTemplate>(nameof(ItemTemplate));
        public IDataTemplate ItemTemplate
        {
            get { return GetValue(ItemTemplateProperty); }
            set => SetValue(ItemTemplateProperty, value);
        }
        public void PreChanged(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e) { }
        public void Changed(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e) { }
        public void PostChanged(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e)
        {
            ItemsCollectionChanged(sender, e);
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
        protected virtual void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateItemCount();
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    viewManager.AddControlItemsToLogicalChildren(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    viewManager.RemoveControlItemsFromLogicalChildren(e.OldItems);
                    break;
            }
            var collection = sender as ICollection;
            PseudoClasses.Set(":empty", collection == null || collection.Count == 0);
            PseudoClasses.Set(":singleitem", collection != null && collection.Count == 1);
        }
        protected virtual void OnContainersRecycled(ItemContainerEventArgs e) { }
        protected virtual void OnContainersMaterialized(ItemContainerEventArgs e)
        {
            foreach (var container in e.Containers)
            {
                if (container.ContainerControl != null && container.ContainerControl != container.Item)
                    LogicalChildren.Add(container.ContainerControl);
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (!e.Handled)
            {

            }
        }
        internal virtual void ColumnNumChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is int columnNum)
            {
                Children.Clear();
                var columnDefinition = new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                };
                for (var i = 0; i < columnNum; i++)
                    this.ColumnDefinitions.Add(columnDefinition);
            }
        }
        internal virtual void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as IEnumerable;
            var newValue = e.NewValue as IEnumerable;
            if (oldValue is INotifyCollectionChanged incc)
                CollectionChangedEventManager.Instance.RemoveListener(incc, this);
            UpdateItemCount();
            viewManager.RemoveControlItemsFromLogicalChildren(oldValue);
            viewManager.AddControlItemsToLogicalChildren(newValue);
            SubscribeToItems(newValue);
        }
        protected virtual void OnContainersDematerialized(ItemContainerEventArgs e)
        {
            foreach (var container in e.Containers)
            {
                if (container?.ContainerControl != container?.Item)
                    Children.Remove(container.ContainerControl);
            }
        }
        internal void ItemTemplateChanged(AvaloniaPropertyChangedEventArgs e)
        {

        }
        private void SubscribeToItems(IEnumerable items)
        {
            PseudoClasses.Set(":empty", items == null || items.Count() == 0);
            PseudoClasses.Set(":singleitem", items != null && items.Count() == 1);
            if (items is INotifyCollectionChanged incc)
                CollectionChangedEventManager.Instance.AddListener(incc, this);
        }
    }
}