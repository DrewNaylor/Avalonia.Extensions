using Avalonia.Controls;
using Avalonia.Input;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// the child item panel/layout for <seealso cref="ItemsRepeater"/>
    /// </summary>
    public sealed class ItemsRepeaterContent : ClickableView
    {
        protected override void OnClick(MouseButton mouseButton)
        {
            if (Parent is HorizontalItemsRepeater h_repeater)
                h_repeater.OnContentClick(this, mouseButton);
            else if (Parent is VerticalItemsRepeater v_repeater)
                v_repeater.OnContentClick(this, mouseButton);
            base.OnClick(mouseButton);
        }
    }
}