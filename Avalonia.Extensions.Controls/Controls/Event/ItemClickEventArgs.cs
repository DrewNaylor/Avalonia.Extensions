using System;

namespace Avalonia.Extensions.Controls
{
    public class ItemClickEventArgs : EventArgs
    {
        public object Item { get; }
        public int ItemIndex { get; }
        public ItemClickEventArgs(object item)
        {
            this.Item = item;
        }
        public ItemClickEventArgs(object item, int index)
        {
            this.Item = item;
            this.ItemIndex = index;
        }
    }
}