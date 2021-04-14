namespace Avalonia.Extensions.Controls.Controls.Repeater
{
    public sealed class ItemsRepeaterContent : ClickableView
    {
        protected override void OnClick()
        {
            if (Parent is HorizontalItemsRepeater h_repeater)
                h_repeater.OnContentClick(this);
            else if (Parent is VerticalItemsRepeater v_repeater)
                v_repeater.OnContentClick(this);
            base.OnClick();
        }
    }
}