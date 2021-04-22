namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// the control:<see cref="CellListView"/> used.
    /// it just a uwp like "GridViewItem"
    /// </summary>
    public sealed class CellListViewCell : ClickableView
    {
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