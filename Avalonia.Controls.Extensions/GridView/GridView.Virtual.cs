using Avalonia.Controls.Extensions.Utils;
using Avalonia.Controls.Generators;
using System.Collections;
using System.Collections.Specialized;

namespace Avalonia.Controls.Extensions
{
    public partial class GridView
    {
        protected virtual void OnContainersMaterialized(ItemContainerEventArgs e)
        {
            foreach (var container in e.Containers)
            {
                if (container.ContainerControl != null && container.ContainerControl != container.Item)
                    LogicalChildren.Add(container.ContainerControl);
            }
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
        protected virtual void OnContainersRecycled(ItemContainerEventArgs e) { }
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
    }
}