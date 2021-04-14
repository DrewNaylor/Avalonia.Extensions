using Avalonia.Controls;
using Avalonia.Styling;
using System;

namespace Avalonia.Extensions.Controls
{
    public class ListViewItem : ListBoxItem, IStyleable
    {
        public Type StyleKey { get; private set; }
        Type IStyleable.StyleKey
        {
            get
            {
                if (StyleKey != null)
                    return StyleKey;
                return typeof(ListViewItem);
            }
        }
        internal void SetDefaultStyleKey(Type key)
        {
            this.StyleKey = key;
        }
        internal void ClearDefaultStyleKey()
        {
            StyleKey = null;
        }
    }
}