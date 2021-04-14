using Avalonia.Controls;
using System;

namespace Avalonia.Extensions.Controls
{
    public abstract class ViewBase : AvaloniaObject
    {
        protected internal virtual void PrepareItem(ListViewItem item) { }
        protected internal virtual void ClearItem(ListViewItem item) { }
        protected internal virtual Type DefaultStyleKey
        {
            get => typeof(ListBox);
        }
        protected internal virtual Type ItemContainerDefaultStyleKey
        {
            get => typeof(ListBoxItem);
        }
        internal virtual void OnThemeChanged() { }
        private bool _isUsed;
        internal bool IsUsed
        {
            get => _isUsed;
            set => _isUsed = value;
        }
    }
}