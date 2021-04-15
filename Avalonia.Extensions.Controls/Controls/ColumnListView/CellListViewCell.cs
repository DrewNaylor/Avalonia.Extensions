namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// the control:<see cref="CellListView"/> used.
    /// it just a uwp like "GridViewItem"
    /// </summary>
    public sealed class CellListViewCell : ClickableView
    {
        protected override void OnClick()
        {
            if (Parent is CellListView wrapView)
                wrapView.OnContentClick(this);
            base.OnClick();
        }
    }
}