using System;

namespace Avalonia.Extensions.Controls
{
    public class ItemClickEventArgs : EventArgs
    {
        public object Item { get; }
        public ItemClickEventArgs(object item)
        {
            this.Item = item;
        }
    }
}