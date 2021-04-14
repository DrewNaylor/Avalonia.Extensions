using Avalonia.Controls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using ControlArray = Avalonia.Controls.Controls;

namespace Avalonia.Extensions.Controls
{
    public abstract class GridViewRowPresenterBase : Panel
    {
        public GridViewRowPresenterBase()
        {
            ColumnsProperty.Changed.AddClassHandler<GridViewRowPresenterBase>(ColumnsPropertyChanged);
            AffectsMeasure<GridViewRowPresenterBase>(ColumnsProperty);
        }
        public static readonly AvaloniaProperty ColumnsProperty =
            AvaloniaProperty.Register<GridViewRowPresenterBase, GridViewColumnCollection>(nameof(Columns), null);
        public GridViewColumnCollection Columns
        {
            get => (GridViewColumnCollection)GetValue(ColumnsProperty); 
            set => SetValue(ColumnsProperty, value); 
        }
        protected new internal IEnumerator LogicalChildren
        {
            get
            {
                if (Children.Count == 0)
                    return Enumerable.Empty<IControl>().GetEnumerator();
                return Children.GetEnumerator();
            }
        }
        protected int VisualChildrenCount => _uiElementCollection == null ? 0 : _uiElementCollection.Count;
        internal virtual void OnColumnCollectionChanged(GridViewColumnCollectionChangedEventArgs e)
        {
            if (DesiredWidthList != null)
            {
                if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
                {
                    if (DesiredWidthList.Count > e.ActualIndex)
                        DesiredWidthList.RemoveAt(e.ActualIndex);
                }
                else if (e.Action == NotifyCollectionChangedAction.Reset)
                    DesiredWidthList = null;
            }
        }
        internal abstract void OnColumnPropertyChanged(GridViewColumn column, string propertyName);
        internal void EnsureDesiredWidthList()
        {
            GridViewColumnCollection columns = Columns;
            if (columns != null)
            {
                int count = columns.Count;
                if (DesiredWidthList == null)
                    DesiredWidthList = new List<double>(count);
                int c = count - DesiredWidthList.Count;
                for (int i = 0; i < c; i++)
                    DesiredWidthList.Add(double.NaN);
            }
        }
        internal List<double> DesiredWidthList
        {
            get => _desiredWidthList; 
            private set => _desiredWidthList = value; 
        }
        internal bool NeedUpdateVisualTree
        {
            get => _needUpdateVisualTree; 
            set => _needUpdateVisualTree = value; 
        }
        internal ControlArray InternalChildren
        {
            get
            {
                if (_uiElementCollection == null)
                    _uiElementCollection = new ControlArray();
                return _uiElementCollection;
            }
        }
        internal const double c_PaddingHeaderMinWidth = 2.0;
        private static void ColumnsPropertyChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            GridViewRowPresenterBase c = (GridViewRowPresenterBase)d;
            GridViewColumnCollection oldCollection = (GridViewColumnCollection)e.OldValue;
            if (oldCollection != null)
            {
                if (!oldCollection.InViewMode && oldCollection.Owner == c.GetStableAncester())
                    oldCollection.Owner = null;
            }
            GridViewColumnCollection newCollection = (GridViewColumnCollection)e.NewValue;
            if (newCollection != null)
            {
                if (!newCollection.InViewMode && newCollection.Owner == null)
                    newCollection.Owner = c.GetStableAncester();
            }
            c.NeedUpdateVisualTree = true;
            c.InvalidateMeasure();
        }
        private Control GetStableAncester() => this;
        private bool IsPresenterVisualReady
        {
            get => IsInitialized && !NeedUpdateVisualTree; 
        }
        private void ColumnCollectionChanged(object sender, NotifyCollectionChangedEventArgs arg)
        {
            if (arg is GridViewColumnCollectionChangedEventArgs e && IsPresenterVisualReady)
            {
                if (e.Column != null)
                    OnColumnPropertyChanged(e.Column, e.PropertyName);
                else
                    OnColumnCollectionChanged(e);
            }
        }
        private ControlArray _uiElementCollection;
        private bool _needUpdateVisualTree = true;
        private List<double> _desiredWidthList;
    }
}