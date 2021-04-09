using Avalonia.Controls.Extensions.Utils;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using System.Collections;
using System.Collections.Specialized;

namespace Avalonia.Controls.Extensions
{
    public partial class GridView : SplitItemControl
    {
        static GridView()
        {
            ItemsPanelProperty.OverrideDefaultValue<GridView>(DefaultPanel);
            ItemsProperty.Changed.AddClassHandler<GridView>((x, e) => x.ItemsChanged(e));
            ItemTemplateProperty.Changed.AddClassHandler<GridView>((x, e) => x.ItemTemplateChanged(e));
        }
        public GridView()
        {
            PseudoClasses.Add(":empty");
            SubscribeToItems(Items);
        }
        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
        }
        private void SubscribeToItems(IEnumerable items)
        {
            PseudoClasses.Set(":empty", items == null || items.Count() == 0);
            PseudoClasses.Set(":singleitem", items != null && items.Count() == 1);
            if (items is INotifyCollectionChanged incc)
                CollectionChangedEventManager.Instance.AddListener(incc, this);
        }
        private void ItemTemplateChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (ItemContainerGenerator != null)
                ItemContainerGenerator.ItemTemplate = (IDataTemplate)e.NewValue;
        }
        public void ScrollIntoRow(int index) => Presenter?.ScrollIntoRow(index);
        public void ScrollIntoRow(object item) => ScrollIntoRow(IndexOf(Items, item));
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
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            Scroll = e.NameScope.Find<IScrollable>("PART_ScrollViewer");
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
    }
}