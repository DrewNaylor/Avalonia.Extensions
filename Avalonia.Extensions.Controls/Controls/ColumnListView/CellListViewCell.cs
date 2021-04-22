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
        public CellListViewCell() : base()
        {
            HorizontalAlignmentProperty.Changed.AddClassHandler<CellListViewCell>(OnHorizontalAlignmentChange);
            VerticalAlignmentProperty.Changed.AddClassHandler<CellListViewCell>(OnVerticalAlignmentChange);
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