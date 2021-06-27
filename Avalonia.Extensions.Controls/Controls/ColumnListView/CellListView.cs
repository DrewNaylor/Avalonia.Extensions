using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using System;
using System.Collections.Specialized;
using System.Linq;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// the uwp like "GridView", it just define itempanel with wrappanel
    /// you need to set "ColumnNum" for columns count
    /// https://stackoverflow.com/questions/23084576/wpf-combobox-multiple-columns
    /// </summary>
    public class CellListView : ListView
    {
        /// <summary>
        /// The width of each cell.
        /// </summary>
        public double CellWidth { get; private set; }
        /// <summary>
        /// Defines the <see cref="ColumnNum"/> property.
        /// </summary>
        public static readonly StyledProperty<int> ColumnNumProperty =
          AvaloniaProperty.Register<CellListView, int>(nameof(ColumnNum), 1);
        /// <summary>
        /// get or set column number.
        /// default value is 1.if the value smaller than 1 it's means depend layout by item controls
        /// </summary>
        public int ColumnNum
        {
            get => GetValue(ColumnNumProperty);
            set
            {
                if (value > 0)
                    CellWidth = Bounds.Width / value;
                else
                    CellWidth = double.NaN;
                SetValue(ColumnNumProperty, value);
            }
        }
        /// <summary>
        /// Defines the <see cref="ChildHorizontalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<HorizontalAlignment> ChildHorizontalContentAlignmentProperty =
            AvaloniaProperty.Register<CellListView, HorizontalAlignment>(nameof(ChildHorizontalContentAlignment));
        /// <summary>
        /// Gets or sets the horizontal alignment of the content within the control.
        /// </summary>
        public HorizontalAlignment ChildHorizontalContentAlignment
        {
            get { return GetValue(ChildHorizontalContentAlignmentProperty); }
            set { SetValue(ChildHorizontalContentAlignmentProperty, value); }
        }
        /// <summary>
        /// Defines the <see cref="ChildVerticalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<VerticalAlignment> ChildVerticalContentAlignmentProperty =
            AvaloniaProperty.Register<CellListView, VerticalAlignment>(nameof(ChildVerticalContentAlignment));
        /// <summary>
        /// Gets or sets the vertical alignment of the content within the control.
        /// </summary>
        public VerticalAlignment ChildVerticalContentAlignment
        {
            get { return GetValue(ChildVerticalContentAlignmentProperty); }
            set { SetValue(ChildVerticalContentAlignmentProperty, value); }
        }
        /// <summary>
        /// Defines the <see cref="HorizontalAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<HorizontalAlignment> ChildHorizontalAlignmentProperty =
            AvaloniaProperty.Register<CellListView, HorizontalAlignment>(nameof(ChildHorizontalAlignment));
        /// <summary>
        /// Gets or sets the element's preferred horizontal alignment in its parent.
        /// </summary>
        public HorizontalAlignment ChildHorizontalAlignment
        {
            get { return GetValue(HorizontalAlignmentProperty); }
            set { SetValue(HorizontalAlignmentProperty, value); }
        }
        /// <summary>
        /// Defines the <see cref="VerticalAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<VerticalAlignment> ChildVerticalAlignmentProperty =
            AvaloniaProperty.Register<CellListView, VerticalAlignment>(nameof(ChildVerticalAlignment));
        /// <summary>
        /// Gets or sets the element's preferred vertical alignment in its parent.
        /// </summary>
        public VerticalAlignment ChildVerticalAlignment
        {
            get { return GetValue(VerticalAlignmentProperty); }
            set { SetValue(VerticalAlignmentProperty, value); }
        }
        private bool trigger = true;
        private Vector _viewHeight = new Vector();
        /// <summary>
        /// create a instance
        /// </summary>
        public CellListView()
        {
            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
            var target = AvaloniaRuntimeXamlLoader.Parse<ItemsPanelTemplate>(Core.WRAP_TEMPLATE);
            SetValue(ItemsPanelProperty, target);
            LogicalChildren.CollectionChanged += LogicalChildren_CollectionChanged;
            ItemsProperty.Changed.AddClassHandler<CellListView>(OnItemsChanged);
            BoundsProperty.Changed.AddClassHandler<CellListView>(OnBoundsChange);
            ChildVerticalAlignmentProperty.Changed.AddClassHandler<CellListView>(OnChildVerticalAlignmentChange);
            ChildHorizontalAlignmentProperty.Changed.AddClassHandler<CellListView>(OnChildHorizontalAlignmentChange);
            ChildVerticalContentAlignmentProperty.Changed.AddClassHandler<CellListView>(OnChildVerticalContentAlignmentChange);
            ChildHorizontalContentAlignmentProperty.Changed.AddClassHandler<CellListView>(OnChildHorizontalContentAlignmentChange);
        }
        private void OnChildVerticalAlignmentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            for (var index = 0; index < LogicalChildren.Count; index++)
            {
                if (LogicalChildren.ElementAt(index) is ListBoxItem listBoxItem)
                    listBoxItem.VerticalAlignment = this.ChildVerticalAlignment;
            }
        }
        private void OnChildHorizontalAlignmentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            for (var index = 0; index < LogicalChildren.Count; index++)
            {
                if (LogicalChildren.ElementAt(index) is ListBoxItem listBoxItem)
                    listBoxItem.HorizontalAlignment = this.ChildHorizontalAlignment;
            }
        }
        private void OnChildVerticalContentAlignmentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            for (var index = 0; index < LogicalChildren.Count; index++)
            {
                if (LogicalChildren.ElementAt(index) is ListBoxItem listBoxItem)
                    listBoxItem.VerticalContentAlignment = this.ChildVerticalContentAlignment;
            }
        }
        private void OnChildHorizontalContentAlignmentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            for (var index = 0; index < LogicalChildren.Count; index++)
            {
                if (LogicalChildren.ElementAt(index) is ListBoxItem listBoxItem)
                    listBoxItem.HorizontalContentAlignment = this.ChildHorizontalContentAlignment;
            }
        }
        private void OnBoundsChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is Rect rect)
            {
                if (CellWidth != double.NaN)
                {
                    CellWidth = rect.Width / ColumnNum;
                    for (var index = 0; index < LogicalChildren.Count; index++)
                    {
                        var item = LogicalChildren.ElementAt(index);
                        SetItemWidth(item);
                    }
                }
            }
        }
        private void SetItemWidth(object item)
        {
            if (item is ListBoxItem obj)
            {
                obj.Width = CellWidth;
                obj.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }
        private void LogicalChildren_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count > 0)
            {
                if (CellWidth != double.NaN)
                {
                    var item = e.NewItems.ElementAt(0);
                    SetItemWidth(item);
                }
            }
        }
        protected override void OnScrollChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            base.OnScrollChange(sender, e);
            if (e.NewValue is ScrollViewer)
                OnItemsChanged(this, e);
        }
        private void OnItemsChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (Scroll is ScrollViewer scrollViewer && scrollViewer.Content is IControl child && child.VisualChildren.FirstOrDefault() is WrapPanel wrapPanel)
            {
                int idx = 0;
                double viewHeight = 0;
                var controls = wrapPanel.Children;
                while (idx * ColumnNum < controls.Count)
                {
                    var array = controls.Skip(idx * ColumnNum).Take(ColumnNum);
                    viewHeight += array.Max(x => x.Bounds.Height);
                    idx++;
                }
                _viewHeight = new Vector(0, viewHeight);
            }
        }
        protected override void ScrollEventHandle(ScrollViewer scrollViewer)
        {
            if (_viewHeight.Equals(default))
                OnItemsChanged(this, default);
            var scrollHieght = scrollViewer.Offset.Y + scrollViewer.Viewport.Height;
            if (scrollHieght == scrollViewer.Viewport.Height && !trigger)
            {
                trigger = true;
                var args = new RoutedEventArgs(ScrollTopEvent);
                RaiseEvent(args);
                if (!args.Handled)
                    args.Handled = true;
            }
            else if (!trigger && scrollHieght > 0 && new Vector(0, scrollHieght).NearlyEquals(_viewHeight))
            {
                trigger = true;
                var args = new RoutedEventArgs(ScrollEndEvent);
                RaiseEvent(args);
                if (!args.Handled)
                    args.Handled = true;
            }
            else
                trigger = false;
        }
    }
}