﻿using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Extensions.Styles;
using Avalonia.Input;
using Avalonia.Layout;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// the control:<see cref="GridView"/> used.
    /// it just a uwp like "GridViewItem"
    /// </summary>
    [PseudoClasses(":pressed", ":selected")]
    public sealed class GridViewItem : ListViewItem, IStyling
    {
        public GridViewItem() : base()
        {
            ParentProperty.Changed.AddClassHandler<GridViewItem>(OnParentChanged);
            VerticalAlignmentProperty.Changed.AddClassHandler<GridViewItem>(OnVerticalAlignmentChange);
            HorizontalAlignmentProperty.Changed.AddClassHandler<GridViewItem>(OnHorizontalAlignmentChange);
            VerticalContentAlignmentProperty.Changed.AddClassHandler<GridViewItem>(OnVerticalContentAlignmentChange);
            HorizontalContentAlignmentProperty.Changed.AddClassHandler<GridViewItem>(OnHorizontalContentAlignmentChange);
            this.InitStyle();
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
                item.VerticalAlignment = VerticalAlignment;
                item.HorizontalAlignment = HorizontalAlignment;
                item.VerticalContentAlignment = VerticalContentAlignment;
                item.HorizontalContentAlignment = HorizontalContentAlignment;
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
        protected override void OnClick(MouseButton mouseButton)
        {
            if (Parent.Parent is GridView itemView)
                itemView.OnContentClick(this, mouseButton);
            base.OnClick(mouseButton);
        }
    }
}