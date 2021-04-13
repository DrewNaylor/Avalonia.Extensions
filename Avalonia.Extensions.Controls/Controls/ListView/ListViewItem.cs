using Avalonia.Controls;
using Avalonia.Styling;
using System;

namespace Avalonia.Extensions.Controls
{
    public class ListViewItem : ListBoxItem, IStyleable
    {
        Type _styleKey;
        Type IStyleable.StyleKey
        {
            get
            {
                if (_styleKey != null)
                    return _styleKey;
                return typeof(ListViewItem);
            }
        }
        internal void SetDefaultStyleKey(Type key)
        {
            this._styleKey = key;
        }
        internal void ClearDefaultStyleKey()
        {
            _styleKey = null;
        }
    }
}