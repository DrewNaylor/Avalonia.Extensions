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
            get { return typeof(ListBox); }
        }
        protected internal virtual Type ItemContainerDefaultStyleKey
        {
            get { return typeof(ListBoxItem); }
        }
        internal virtual void OnThemeChanged() { }
        internal bool IsUsed
        {
            get { return _isUsed; }
            set { _isUsed = value; }
        }
        private bool _isUsed;
    }
}