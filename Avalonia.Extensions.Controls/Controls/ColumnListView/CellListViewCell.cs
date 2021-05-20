using Avalonia.Controls;
using Avalonia.Layout;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// the control:<see cref="CellListView"/> used.
    /// it just a uwp like "GridViewItem"
    /// </summary>
    public sealed class CellListViewCell : ClickableView
    {
        /// <summary>
        /// Defines the <see cref="HorizontalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
            AvaloniaProperty.Register<CellListViewCell, HorizontalAlignment>(nameof(HorizontalContentAlignment));
        /// <summary>
        /// Gets or sets the horizontal alignment of the content within the control.
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get { return GetValue(HorizontalContentAlignmentProperty); }
            set { SetValue(HorizontalContentAlignmentProperty, value); }
        }
        /// <summary>
        /// Defines the <see cref="VerticalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
            AvaloniaProperty.Register<CellListViewCell, VerticalAlignment>(nameof(VerticalContentAlignment));
        /// <summary>
        /// Gets or sets the vertical alignment of the content within the control.
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get { return GetValue(VerticalContentAlignmentProperty); }
            set { SetValue(VerticalContentAlignmentProperty, value); }
        }
        public CellListViewCell() : base()
        {
            ParentProperty.Changed.AddClassHandler<CellListViewCell>(OnParentChanged);
            VerticalAlignmentProperty.Changed.AddClassHandler<CellListViewCell>(OnVerticalAlignmentChange);
            HorizontalAlignmentProperty.Changed.AddClassHandler<CellListViewCell>(OnHorizontalAlignmentChange);
            VerticalContentAlignmentProperty.Changed.AddClassHandler<CellListViewCell>(OnVerticalContentAlignmentChange);
            HorizontalContentAlignmentProperty.Changed.AddClassHandler<CellListViewCell>(OnHorizontalContentAlignmentChange);
        }
        private void OnHorizontalContentAlignmentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (Parent is ListBoxItem item && e.NewValue is HorizontalAlignment horizontal)
                item.HorizontalContentAlignment = horizontal;
        }
        private void OnVerticalContentAlignmentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (Parent is ListBoxItem item && e.NewValue is VerticalAlignment vertical)
                item.VerticalContentAlignment = vertical;
        }
        private void OnParentChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is ListBoxItem item)
            {
                item.VerticalAlignment = this.VerticalAlignment;
                item.HorizontalAlignment = this.HorizontalAlignment;
                item.VerticalContentAlignment = this.VerticalContentAlignment;
                item.HorizontalContentAlignment = this.HorizontalContentAlignment;
            }
        }
        private void OnVerticalAlignmentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (Parent is ListBoxItem item && e.NewValue is VerticalAlignment vertical)
                item.VerticalAlignment = vertical;
        }
        private void OnHorizontalAlignmentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (Parent is ListBoxItem item && e.NewValue is HorizontalAlignment horizontal)
                item.HorizontalAlignment = horizontal;
        }
        /// <summary>
        /// handle click event
        /// </summary>
        protected override void OnClick()
        {
            if (Parent.Parent is CellListView itemView)
                itemView.OnContentClick(this);
            base.OnClick();
        }
    }
}