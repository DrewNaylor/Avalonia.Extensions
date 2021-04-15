using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// fork from https://github.com/jhofinger/Avalonia/tree/listview
    /// </summary>
    internal class GridViewColumnCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
    {
        internal GridViewColumnCollectionChangedEventArgs(GridViewColumn column, string propertyName) : base(NotifyCollectionChangedAction.Reset)
        {
            _column = column;
            _propertyName = propertyName;
        }
        internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn[] clearedColumns) : base(action)
        {
            _clearedColumns = Array.AsReadOnly<GridViewColumn>(clearedColumns);
        }
        internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn changedItem, int index, int actualIndex)
            : base(action, changedItem, index)
        {
            Debug.Assert(action == NotifyCollectionChangedAction.Add || action == NotifyCollectionChangedAction.Remove,
                "This constructor only supports Add/Remove action.");
            Debug.Assert(changedItem != null, "changedItem can't be null");
            Debug.Assert(index >= 0, "index must >= 0");
            Debug.Assert(actualIndex >= 0, "actualIndex must >= 0");
            _actualIndex = actualIndex;
        }
        internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn newItem, GridViewColumn oldItem, int index, int actualIndex) : base(action, newItem, oldItem, index)
        {
            Debug.Assert(newItem != null, "newItem can't be null");
            Debug.Assert(oldItem != null, "oldItem can't be null");
            Debug.Assert(index >= 0, "index must >= 0");
            Debug.Assert(actualIndex >= 0, "actualIndex must >= 0");
            _actualIndex = actualIndex;
        }
        internal GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction action, GridViewColumn changedItem, int index, int oldIndex, int actualIndex) : base(action, changedItem, index, oldIndex)
        {
            Debug.Assert(changedItem != null, "changedItem can't be null");
            Debug.Assert(index >= 0, "index must >= 0");
            Debug.Assert(oldIndex >= 0, "oldIndex must >= 0");
            Debug.Assert(actualIndex >= 0, "actualIndex must >= 0");
            _actualIndex = actualIndex;
        }
        private int _actualIndex = -1;
        internal int ActualIndex
        {
            get => _actualIndex;
        }
        private ReadOnlyCollection<GridViewColumn> _clearedColumns;
        internal ReadOnlyCollection<GridViewColumn> ClearedColumns
        {
            get => _clearedColumns;
        }
        private GridViewColumn _column;
        internal GridViewColumn Column
        {
            get => _column;
        }
        private string _propertyName;
        internal string PropertyName
        {
            get => _propertyName;
        }
    }
}