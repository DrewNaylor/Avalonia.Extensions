using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace Avalonia.Extensions.Controls
{
    public class GridViewColumnCollection : ObservableCollection<GridViewColumn>
    {
        protected override void ClearItems()
        {
            VerifyAccess();
            _internalEventArg = ClearPreprocess();
            base.ClearItems();
        }
        protected override void RemoveItem(int index)
        {
            VerifyAccess();
            _internalEventArg = RemoveAtPreprocess(index);
            base.RemoveItem(index);
        }
        protected override void InsertItem(int index, GridViewColumn column)
        {
            VerifyAccess();
            _internalEventArg = InsertPreprocess(index, column);
            base.InsertItem(index, column);
        }
        protected override void SetItem(int index, GridViewColumn column)
        {
            VerifyAccess();
            _internalEventArg = SetPreprocess(index, column);
            if (_internalEventArg != null)
                base.SetItem(index, column);
        }
        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (oldIndex != newIndex)
            {
                VerifyAccess();
                _internalEventArg = MovePreprocess(oldIndex, newIndex);
                base.MoveItem(oldIndex, newIndex);
            }
        }
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            OnInternalCollectionChanged();
            base.OnCollectionChanged(e);
        }
        internal event NotifyCollectionChangedEventHandler InternalCollectionChanged
        {
            add { _internalCollectionChanged += value; }
            remove { _internalCollectionChanged -= value; }
        }
        internal void BlockWrite()
        {
            Debug.Assert(IsImmutable != true, "IsImmutable is true before BlockWrite");
            IsImmutable = true;
        }
        internal void UnblockWrite()
        {
            Debug.Assert(IsImmutable != false, "IsImmutable is flase before UnblockWrite");
            IsImmutable = false;
        }
        internal List<GridViewColumn> ColumnCollection
        {
            get => _columns;
        }
        internal List<int> IndexList
        {
            get => _actualIndices;
        }
        internal AvaloniaObject Owner
        {
            get => _owner;
            set
            {
                if (value != _owner)
                    _owner = value;
            }
        }
        [NonSerialized]
        private AvaloniaObject _owner = null;
        internal bool InViewMode
        {
            get => _inViewMode;
            set => _inViewMode = value;
        }
        private bool _inViewMode;
        private void OnInternalCollectionChanged()
        {
            if (_internalCollectionChanged != null && _internalEventArg != null)
            {
                _internalCollectionChanged(this, _internalEventArg);
                _internalEventArg = null;
            }
        }
        private void ColumnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_internalCollectionChanged != null && sender is GridViewColumn column)
                _internalCollectionChanged(this, new GridViewColumnCollectionChangedEventArgs(column, e.PropertyName));
        }
        private GridViewColumnCollectionChangedEventArgs MovePreprocess(int oldIndex, int newIndex)
        {
            Debug.Assert(oldIndex != newIndex, "oldIndex==newIndex when perform move action.");
            VerifyIndexInRange(oldIndex, "oldIndex");
            VerifyIndexInRange(newIndex, "newIndex");
            int actualIndex = _actualIndices[oldIndex];
            if (oldIndex < newIndex)
            {
                for (int targetIndex = oldIndex; targetIndex < newIndex; targetIndex++)
                    _actualIndices[targetIndex] = _actualIndices[targetIndex + 1];
            }
            else
            {
                for (int targetIndex = oldIndex; targetIndex > newIndex; targetIndex--)
                    _actualIndices[targetIndex] = _actualIndices[targetIndex - 1];
            }
            _actualIndices[newIndex] = actualIndex;
            return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Move, _columns[actualIndex], newIndex, oldIndex, actualIndex);
        }
        private GridViewColumnCollectionChangedEventArgs ClearPreprocess()
        {
            GridViewColumn[] list = new GridViewColumn[Count];
            if (Count > 0)
                CopyTo(list, 0);
            foreach (GridViewColumn c in _columns)
            {
                c.ResetPrivateData();
                ((INotifyPropertyChanged)c).PropertyChanged -= new PropertyChangedEventHandler(ColumnPropertyChanged);
            }
            _columns.Clear();
            _actualIndices.Clear();
            return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset, list);
        }
        private GridViewColumnCollectionChangedEventArgs RemoveAtPreprocess(int index)
        {
            VerifyIndexInRange(index, "index");
            int actualIndex = _actualIndices[index];
            GridViewColumn column = _columns[actualIndex];
            column.ResetPrivateData();
            ((INotifyPropertyChanged)column).PropertyChanged -= new PropertyChangedEventHandler(ColumnPropertyChanged);
            _columns.RemoveAt(actualIndex);
            UpdateIndexList(actualIndex, index);
            UpdateActualIndexInColumn(actualIndex);
            return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, column, index, actualIndex);
        }
        private void UpdateIndexList(int actualIndex, int index)
        {
            for (int sourceIndex = 0; sourceIndex < index; sourceIndex++)
            {
                int i = _actualIndices[sourceIndex];
                if (i > actualIndex)
                    _actualIndices[sourceIndex] = i - 1;
            }
            for (int sourceIndex = index + 1; sourceIndex < _actualIndices.Count; sourceIndex++)
            {
                int i = _actualIndices[sourceIndex];
                if (i < actualIndex)
                    _actualIndices[sourceIndex - 1] = i;
                else if (i > actualIndex)
                    _actualIndices[sourceIndex - 1] = i - 1;
            }
            _actualIndices.RemoveAt(_actualIndices.Count - 1);
        }
        private void UpdateActualIndexInColumn(int iStart)
        {
            for (int i = iStart; i < _columns.Count; i++)
                _columns[i].ActualIndex = i;
        }
        private GridViewColumnCollectionChangedEventArgs InsertPreprocess(int index, GridViewColumn column)
        {
            int count = _columns.Count;
            if (index < 0 || index > count)
                throw new ArgumentOutOfRangeException("index");
            ValidateColumnForInsert(column);
            _columns.Add(column);
            column.ActualIndex = count;
            _actualIndices.Insert(index, count);
            ((INotifyPropertyChanged)column).PropertyChanged += new PropertyChangedEventHandler(ColumnPropertyChanged);
            return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, column, index, count);
        }
        private GridViewColumnCollectionChangedEventArgs SetPreprocess(int index, GridViewColumn newColumn)
        {
            VerifyIndexInRange(index, "index");
            GridViewColumn oldColumn = this[index];
            if (oldColumn != newColumn)
            {
                int oldColumnActualIndex = _actualIndices[index];
                RemoveAtPreprocess(index);
                InsertPreprocess(index, newColumn);
                return new GridViewColumnCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newColumn, oldColumn, index, oldColumnActualIndex);
            }
            return null;
        }
        private void VerifyIndexInRange(int index, string indexName)
        {
            Contract.Requires<ArgumentOutOfRangeException>(index > 0 && index < _actualIndices.Count);
        }
        private void ValidateColumnForInsert(GridViewColumn column)
        {
            Contract.Requires<ArgumentNullException>(column != null);
            Contract.Requires<InvalidOperationException>(column.ActualIndex < 0);
        }
        private void VerifyAccess()
        {
            Contract.Requires<InvalidOperationException>(!IsImmutable);
            CheckReentrancy();
        }
        private List<GridViewColumn> _columns = new List<GridViewColumn>();
        private List<int> _actualIndices = new List<int>();
        private bool IsImmutable
        {
            get => _isImmutable;
            set => _isImmutable = value;
        }
        private bool _isImmutable;
        private event NotifyCollectionChangedEventHandler _internalCollectionChanged;
        [NonSerialized]
        private GridViewColumnCollectionChangedEventArgs _internalEventArg;
    }
}