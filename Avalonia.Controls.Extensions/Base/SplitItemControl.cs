using Avalonia.Collections;
using Avalonia.Controls.Extensions.Presenters;
using Avalonia.Controls.Extensions.Utils;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.LogicalTree;
using Avalonia.Metadata;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Avalonia.Controls.Extensions
{
    public class SplitItemControl : TemplatedControl, ICollectionChangedListener, IColumnsPresenterHost
    {
        static SplitItemControl()
        {
            ItemsProperty.Changed.AddClassHandler<GridView>((x, e) => x.ItemsChanged(e));
            ItemTemplateProperty.Changed.AddClassHandler<GridView>((x, e) => x.ItemTemplateChanged(e));
        }
        public SplitItemControl()
        {
            PseudoClasses.Add(":empty");
            SubscribeToItems(_items);
        }
        public IColumnsPresenter Presenter
        {
            get;
            protected set;
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
        public static readonly DirectProperty<GridView, int> ItemCountProperty =
            AvaloniaProperty.RegisterDirect<GridView, int>(nameof(ItemCount), o => o.ItemCount);
        private int _itemCount;
        public int ItemCount
        {
            get => _itemCount;
            private set => SetAndRaise(ItemCountProperty, ref _itemCount, value);
        }
        public static readonly DirectProperty<GridView, IEnumerable> ItemsProperty =
         AvaloniaProperty.RegisterDirect<GridView, IEnumerable>(nameof(Items), o => o.Items, (o, v) => o.Items = v);
        private IEnumerable _items = new AvaloniaList<object>();
        [Content]
        public IEnumerable Items
        {
            get => _items;
            set => SetAndRaise(ItemsProperty, ref _items, value);
        }
        public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty =
             AvaloniaProperty.Register<ItemsControl, IDataTemplate>(nameof(ItemTemplate));
        public IDataTemplate ItemTemplate
        {
            get { return GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        private IItemContainerGenerator _itemContainerGenerator;
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
        public void PreChanged(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e) { }
        public void Changed(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e) { }
        public void PostChanged(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e)
        {
            ItemsCollectionChanged(sender, e);
        }
        void IColumnsPresenterHost.RegisterItemsPresenter(IColumnsPresenter presenter)
        {
            Presenter = presenter;
            ItemContainerGenerator.Clear();
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
                    AddControlItemsToLogicalChildren(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveControlItemsFromLogicalChildren(e.OldItems);
                    break;
            }
            Presenter?.ItemsChanged(e);
            var collection = sender as ICollection;
            PseudoClasses.Set(":empty", collection == null || collection.Count == 0);
            PseudoClasses.Set(":singleitem", collection != null && collection.Count == 1);
        }
        private void AddControlItemsToLogicalChildren(IEnumerable items)
        {
            var toAdd = new List<ILogical>();
            if (items != null)
            {
                foreach (var i in items)
                {
                    if (i is IControl control && !LogicalChildren.Contains(control))
                        toAdd.Add(control);
                }
            }
            LogicalChildren.AddRange(toAdd);
        }
        private void RemoveControlItemsFromLogicalChildren(IEnumerable items)
        {
            var toRemove = new List<ILogical>();
            if (items != null)
            {
                foreach (var i in items)
                {
                    if (i is IControl control)
                        toRemove.Add(control);
                }
            }
            LogicalChildren.RemoveAll(toRemove);
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
        protected virtual void ItemsChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var oldValue = e.OldValue as IEnumerable;
            var newValue = e.NewValue as IEnumerable;
            if (oldValue is INotifyCollectionChanged incc)
                CollectionChangedEventManager.Instance.RemoveListener(incc, this);
            UpdateItemCount();
            RemoveControlItemsFromLogicalChildren(oldValue);
            AddControlItemsToLogicalChildren(newValue);
            if (Presenter != null)
                Presenter.Items = newValue;
            SubscribeToItems(newValue);
        }
        protected virtual IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator(this);
        }
        protected virtual void OnContainersDematerialized(ItemContainerEventArgs e)
        {
            foreach (var container in e.Containers)
            {
                if (container?.ContainerControl != container?.Item)
                    LogicalChildren.Remove(container.ContainerControl);
            }
        }
        protected static object ElementAt(IEnumerable items, int index)
        {
            if (index != -1 && index < items.Count())
                return items.ElementAt(index) ?? null;
            else
                return null;
        }
        protected static int IndexOf(IEnumerable items, object item)
        {
            if (items != null && item != null)
            {
                if (items is IList list)
                    return list.IndexOf(item);
                else
                {
                    int index = 0;
                    foreach (var i in items)
                    {
                        if (Equals(i, item))
                            return index;
                        ++index;
                    }
                }
            }
            return -1;
        }
        private void ItemTemplateChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (_itemContainerGenerator != null)
                _itemContainerGenerator.ItemTemplate = (IDataTemplate)e.NewValue;
        }
        private void SubscribeToItems(IEnumerable items)
        {
            PseudoClasses.Set(":empty", items == null || items.Count() == 0);
            PseudoClasses.Set(":singleitem", items != null && items.Count() == 1);
            if (items is INotifyCollectionChanged incc)
                CollectionChangedEventManager.Instance.AddListener(incc, this);
        }
        protected static IInputElement GetNextControl(INavigableContainer container, NavigationDirection direction, IInputElement from, bool wrap)
        {
            IInputElement result;
            var c = from;
            do
            {
                result = container.GetControl(direction, c, wrap);
                from ??= result;
                if (result != null && result.Focusable && result.IsEffectivelyEnabled && result.IsEffectivelyVisible)
                    return result;
                c = result;
            } while (c != null && c != from);
            return null;
        }
    }
}