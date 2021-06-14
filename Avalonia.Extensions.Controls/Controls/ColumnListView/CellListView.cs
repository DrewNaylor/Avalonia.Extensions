using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using System.Collections.Specialized;

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
        /// <summary>
        /// create a instance
        /// </summary>
        public CellListView()
        {
            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
            var target = AvaloniaRuntimeXamlLoader.Parse<ItemsPanelTemplate>(Core.WRAP_TEMPLATE);
            SetValue(ItemsPanelProperty, target);
            LogicalChildren.CollectionChanged += LogicalChildren_CollectionChanged;
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
    }
}