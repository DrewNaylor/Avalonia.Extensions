using Avalonia.Controls.Extensions.Utils;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using System.Collections.Specialized;

namespace Avalonia.Controls.Extensions
{
    public partial class GridView : TemplatedControl, ICollectionChangedListener
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
            SubscribeToItems(_items);
        }
        public void PreChanged(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e) { }
        public void Changed(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e) { }
        public void PostChanged(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e)
        {
            ItemsCollectionChanged(sender, e);
        }
        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);

        }
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);

        }
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            Scroll = e.NameScope.Find<IScrollable>("PART_GridView");
        }
    }
}