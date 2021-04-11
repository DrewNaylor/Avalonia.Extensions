using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using System;

namespace Avalonia.Controls.Extensions
{
    public partial class GridView : GridLayout
    {
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
        public void ScrollIntoRow(int index) => Presenter?.ScrollIntoRow(index);
        public void ScrollIntoRow(object item) => ScrollIntoRow(IndexOf(Items, item));
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            Scroll = e.NameScope.Find<IScrollable>("PART_ScrollViewer");
        }
        internal void OnContentClick(ClickablePanel gridItem)
        {
            if (gridItem != null)
            {

            }
        }
    }
}