using Avalonia.Controls.Extensions.Utils;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Avalonia.Controls.Extensions
{
    public partial class GridView
    {
        private void SubscribeToItems(IEnumerable items)
        {
            PseudoClasses.Set(":empty", items == null || items.Count() == 0);
            PseudoClasses.Set(":singleitem", items != null && items.Count() == 1);
            if (items is INotifyCollectionChanged incc)
                CollectionChangedEventManager.Instance.AddListener(incc, this);
        }
        private void ItemTemplateChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (_itemContainerGenerator != null)
                _itemContainerGenerator.ItemTemplate = (IDataTemplate)e.NewValue;
        }
        private void UpdateItemView()
        {

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
    }
}