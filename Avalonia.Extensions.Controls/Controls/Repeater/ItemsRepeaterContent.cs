using Avalonia.Controls;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// the child item panel/layout for <seealso cref="ItemsRepeater"/>
    /// </summary>
    public sealed class ItemsRepeaterContent : ClickableView
    {
        /// <summary>
        /// handle click event
        /// </summary>
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