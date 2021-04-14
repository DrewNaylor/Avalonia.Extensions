namespace Avalonia.Extensions.Controls
{
    public sealed class ColumnListViewCell : ClickableView
    {
        protected override void OnClick()
        {
            if (Parent is ColumnListView wrapView)
                wrapView.OnContentClick(this);
            base.OnClick();
        }
    }
}