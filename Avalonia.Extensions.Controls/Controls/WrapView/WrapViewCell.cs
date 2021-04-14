namespace Avalonia.Extensions.Controls
{
    public sealed class WrapViewCell : ClickableView
    {
        protected override void OnClick()
        {
            if (Parent is WrapView wrapView)
                wrapView.OnContentClick(this);
            base.OnClick();
        }
    }
}