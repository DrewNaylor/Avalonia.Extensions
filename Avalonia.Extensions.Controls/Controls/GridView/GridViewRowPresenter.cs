using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Utilities;
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using ControlArray = Avalonia.Controls.Controls;

namespace Avalonia.Extensions.Controls
{
    public class GridViewRowPresenter : GridViewRowPresenterBase
    {
        public GridViewRowPresenter()
        {
            ContentProperty.Changed.AddClassHandler<GridViewRowPresenter>(OnContentChanged);
            AffectsMeasure<GridViewRowPresenter>(ContentProperty);
        }
        public static readonly StyledProperty<object> ContentProperty =
        ContentControl.ContentProperty.AddOwner<GridViewRowPresenter>();
        public object Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        private static void OnContentChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            GridViewRowPresenter gvrp = (GridViewRowPresenter)d;
            Type oldType = e.OldValue?.GetType();
            Type newType = e.NewValue?.GetType();
            if (oldType != newType)
                gvrp.NeedUpdateVisualTree = true;
            else
                gvrp.UpdateCells();
        }
        protected override Size MeasureOverride(Size constraint)
        {
            GridViewColumnCollection columns = Columns;
            if (columns == null)
                return new Size();
            ControlArray children = InternalChildren;
            double maxHeight = 0.0;
            double accumulatedWidth = 0.0;
            double constraintHeight = constraint.Height;
            bool desiredWidthListEnsured = false;
            foreach (GridViewColumn column in columns)
            {
                IControl child = children[column.ActualIndex];
                if (child == null) continue;
                double childConstraintWidth = Math.Max(0.0, constraint.Width - accumulatedWidth);
                if (column.State == ColumnMeasureState.Init || column.State == ColumnMeasureState.Headered)
                {
                    if (!desiredWidthListEnsured)
                    {
                        EnsureDesiredWidthList();
                        LayoutUpdated += new EventHandler(OnLayoutUpdated);
                        desiredWidthListEnsured = true;
                    }
                    child.Measure(new Size(childConstraintWidth, constraintHeight));
                    if (IsOnCurrentPage)
                        column.EnsureWidth(child.DesiredSize.Width);
                    DesiredWidthList[column.ActualIndex] = column.DesiredWidth;
                    accumulatedWidth += column.DesiredWidth;
                }
                else if (column.State == ColumnMeasureState.Data)
                {
                    childConstraintWidth = Math.Min(childConstraintWidth, column.DesiredWidth);
                    child.Measure(new Size(childConstraintWidth, constraintHeight));
                    accumulatedWidth += column.DesiredWidth;
                }
                else
                {
                    childConstraintWidth = Math.Min(childConstraintWidth, column.Width);
                    child.Measure(new Size(childConstraintWidth, constraintHeight));
                    accumulatedWidth += column.Width;
                }
                maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
            }
            _isOnCurrentPageValid = false;
            accumulatedWidth += c_PaddingHeaderMinWidth;
            return (new Size(accumulatedWidth, maxHeight));
        }
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            GridViewColumnCollection columns = Columns;
            if (columns == null)
                return arrangeSize;
            ControlArray children = InternalChildren;
            double accumulatedWidth = 0.0;
            double remainingWidth = arrangeSize.Width;
            foreach (GridViewColumn column in columns)
            {
                IControl child = children[column.ActualIndex];
                if (child == null) continue;
                double childArrangeWidth = Math.Min(remainingWidth, (column.State == ColumnMeasureState.SpecificWidth) ? column.Width : column.DesiredWidth);
                child.Arrange(new Rect(accumulatedWidth, 0, childArrangeWidth, arrangeSize.Height));
                remainingWidth -= childArrangeWidth;
                accumulatedWidth += childArrangeWidth;
            }
            return arrangeSize;
        }
        internal override void OnColumnPropertyChanged(GridViewColumn column, string propertyName)
        {
            Debug.Assert(column != null);
            int index;
            if (GridViewColumn.c_ActualWidthName.Equals(propertyName))
                return;
            if (((index = column.ActualIndex) >= 0) && (index < InternalChildren.Count))
            {
                if (GridViewColumn.WidthProperty.Name.Equals(propertyName))
                    InvalidateMeasure();
                else
                {
                    if (InternalChildren[index] is ContentPresenter cp)
                    {
                        if (GridViewColumn.CellTemplateProperty.Name.Equals(propertyName))
                        {
                            IDataTemplate dt;
                            if ((dt = column.CellTemplate) == null)
                                cp.ClearValue(ContentControl.ContentTemplateProperty);
                            else
                                cp.ContentTemplate = dt;
                        }
                    }
                }
            }
        }
        internal override void OnColumnCollectionChanged(GridViewColumnCollectionChangedEventArgs e)
        {
            base.OnColumnCollectionChanged(e);
            if (e.Action == NotifyCollectionChangedAction.Move)
                InvalidateArrange();
            else
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        InternalChildren.Add(CreateCell((GridViewColumn)e.NewItems[0]));
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        InternalChildren.RemoveAt(e.ActualIndex);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        InternalChildren.RemoveAt(e.ActualIndex);
                        InternalChildren.Add(CreateCell((GridViewColumn)e.NewItems[0]));
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        InternalChildren.Clear();
                        break;
                    default:
                        break;
                }
                InvalidateMeasure();
            }
        }
        private bool CheckVisibleOnCurrentPage()
        {
            bool result = true;
            return result;
        }
        private void OnLayoutUpdated(object sender, EventArgs e)
        {
            bool desiredWidthChanged = false;
            GridViewColumnCollection columns = Columns;
            if (columns != null)
            {
                foreach (GridViewColumn column in columns)
                {
                    if ((column.State != ColumnMeasureState.SpecificWidth))
                    {
                        column.State = ColumnMeasureState.Data;
                        if (DesiredWidthList == null || column.ActualIndex >= DesiredWidthList.Count)
                        {
                            desiredWidthChanged = true;
                            break;
                        }
                        if (!MathUtilities.AreClose(column.DesiredWidth, DesiredWidthList[column.ActualIndex]))
                        {
                            DesiredWidthList[column.ActualIndex] = column.DesiredWidth;
                            desiredWidthChanged = true;
                        }
                    }
                }
            }
            if (desiredWidthChanged)
                InvalidateMeasure();
            LayoutUpdated -= new EventHandler(OnLayoutUpdated);
        }
        private Control CreateCell(GridViewColumn column)
        {
            Debug.Assert(column != null, "column shouldn't be null");
            Control cell;
            {
                ContentPresenter cp = new ContentPresenter { Content = Content };
                IDataTemplate dt;
                if ((dt = column.CellTemplate) != null)
                    cp.ContentTemplate = dt;
                cell = cp;
            }
            if (TemplatedParent is ContentControl parent)
            {
                cell.VerticalAlignment = parent.VerticalContentAlignment;
                cell.HorizontalAlignment = parent.HorizontalContentAlignment;
            }
            cell.Margin = _defalutCellMargin;
            return cell;
        }
        private void UpdateCells()
        {
            ContentPresenter cellAsCP;
            Control cell;
            ControlArray children = InternalChildren;
            for (int i = 0; i < children.Count; i++)
            {
                cell = (Control)children[i];
                if ((cellAsCP = cell as ContentPresenter) != null)
                    cellAsCP.Content = Content;
                else
                {
                    Debug.Assert(cell is TextBlock, "cells are either TextBlocks or ContentPresenters");
                    cell.DataContext = Content;
                }
                if (TemplatedParent is ContentControl parent)
                {
                    cell.VerticalAlignment = parent.VerticalContentAlignment;
                    cell.HorizontalAlignment = parent.HorizontalContentAlignment;
                }
            }
        }
        private bool IsOnCurrentPage
        {
            get
            {
                if (!_isOnCurrentPageValid)
                {
                    _isOnCurrentPage = IsVisible && CheckVisibleOnCurrentPage();
                    _isOnCurrentPageValid = true;
                }
                return _isOnCurrentPage;
            }
        }
        private bool _isOnCurrentPage = false;
        private bool _isOnCurrentPageValid = false;
        private static readonly Thickness _defalutCellMargin = new Thickness(6, 0, 6, 0);
    }
}