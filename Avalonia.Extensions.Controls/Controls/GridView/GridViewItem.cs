﻿using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Layout;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// the control:<see cref="GridView"/> used.
    /// it just a uwp like "GridViewItem"
    /// </summary>
    public sealed class GridViewItem : ClickableView
    {
        /// <summary>
        /// Defines the <see cref="HorizontalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
            AvaloniaProperty.Register<GridViewItem, HorizontalAlignment>(nameof(HorizontalContentAlignment));
        /// <summary>
        /// Gets or sets the horizontal alignment of the content within the control.
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="VerticalContentAlignment"/> property.
        /// </summary>
        public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
            AvaloniaProperty.Register<GridViewItem, VerticalAlignment>(nameof(VerticalContentAlignment));
        /// <summary>
        /// Gets or sets the vertical alignment of the content within the control.
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get => GetValue(VerticalContentAlignmentProperty);
            set => SetValue(VerticalContentAlignmentProperty, value);
        }
        public GridViewItem() : base()
        {
            ParentProperty.Changed.AddClassHandler<GridViewItem>(OnParentChanged);
            VerticalAlignmentProperty.Changed.AddClassHandler<GridViewItem>(OnVerticalAlignmentChange);
            HorizontalAlignmentProperty.Changed.AddClassHandler<GridViewItem>(OnHorizontalAlignmentChange);
            VerticalContentAlignmentProperty.Changed.AddClassHandler<GridViewItem>(OnVerticalContentAlignmentChange);
            HorizontalContentAlignmentProperty.Changed.AddClassHandler<GridViewItem>(OnHorizontalContentAlignmentChange);
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
        protected override void OnClick(MouseButton mouseButton)
        {
            if (Parent.Parent is GridView itemView)
                itemView.OnContentClick(this, mouseButton);
            base.OnClick(mouseButton);
        }
    }
}