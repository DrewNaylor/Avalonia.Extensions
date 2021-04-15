using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using ControlArray = Avalonia.Controls.Controls;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// fork from https://github.com/jhofinger/Avalonia/tree/listview
    /// </summary>
    public class GridViewHeaderRowPresenter : GridViewRowPresenterBase
    {
        public static readonly StyledProperty<IDataTemplate> ColumnHeaderTemplateProperty =
            GridView.ColumnHeaderTemplateProperty.AddOwner<GridViewHeaderRowPresenter>();
        public IDataTemplate ColumnHeaderTemplate
        {
            get => GetValue(ColumnHeaderTemplateProperty);
            set => SetValue(ColumnHeaderTemplateProperty, value);
        }
        public static readonly StyledProperty<string> ColumnHeaderStringFormatProperty =
            GridView.ColumnHeaderStringFormatProperty.AddOwner<GridViewHeaderRowPresenter>();
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ColumnHeaderStringFormat
        {
            get => GetValue(ColumnHeaderStringFormatProperty);
            set => SetValue(ColumnHeaderStringFormatProperty, value);
        }
        public static readonly StyledProperty<bool> AllowsColumnReorderProperty =
            GridView.AllowsColumnReorderProperty.AddOwner<GridViewHeaderRowPresenter>();
        public bool AllowsColumnReorder
        {
            get => GetValue(AllowsColumnReorderProperty);
            set => SetValue(AllowsColumnReorderProperty, value);
        }
        public static readonly StyledProperty<ContextMenu> ColumnHeaderContextMenuProperty =
            GridView.ColumnHeaderContextMenuProperty.AddOwner<GridViewHeaderRowPresenter>();
        public ContextMenu ColumnHeaderContextMenu
        {
            get => GetValue(ColumnHeaderContextMenuProperty);
            set => SetValue(ColumnHeaderContextMenuProperty, value);
        }
        public static readonly StyledProperty<ToolTip> ColumnHeaderToolTipProperty =
            GridView.ColumnHeaderToolTipProperty.AddOwner<GridViewHeaderRowPresenter>();
        public object ColumnHeaderToolTip
        {
            get => GetValue(ColumnHeaderToolTipProperty);
            set => SetValue(ColumnHeaderToolTipProperty, value);
        }
        protected override Size MeasureOverride(Size constraint)
        {
            GridViewColumnCollection columns = Columns;
            ControlArray children = InternalChildren;
            double maxHeight = 0.0;
            double accumulatedWidth = 0.0;
            double constraintHeight = constraint.Height;
            bool desiredWidthListEnsured = false;
            if (columns != null)
            {
                for (int i = 0; i < columns.Count; ++i)
                {
                    IControl child = children[GetVisualIndex(i)];
                    if (child == null) continue;
                    double childConstraintWidth = Math.Max(0.0, constraint.Width - accumulatedWidth);
                    GridViewColumn column = columns[i];
                    if (column.State == ColumnMeasureState.Init)
                    {
                        if (!desiredWidthListEnsured)
                        {
                            EnsureDesiredWidthList();
                            LayoutUpdated += new EventHandler(OnLayoutUpdated);
                            desiredWidthListEnsured = true;
                        }
                        child.Measure(new Size(childConstraintWidth, constraintHeight));
                        DesiredWidthList[column.ActualIndex] = column.EnsureWidth(child.DesiredSize.Width);
                        accumulatedWidth += column.DesiredWidth;
                    }
                    else if (column.State == ColumnMeasureState.Headered || column.State == ColumnMeasureState.Data)
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
            }
            Debug.Assert(_paddingHeader != null, "padding header is null");
            _paddingHeader.Measure(new Size(0.0, constraintHeight));
            maxHeight = Math.Max(maxHeight, _paddingHeader.DesiredSize.Height);
            accumulatedWidth += c_PaddingHeaderMinWidth;
            if (_isHeaderDragging)
            {
                Debug.Assert(_indicator != null, "_indicator is null");
                Debug.Assert(_floatingHeader != null, "_floatingHeader is null");
                _indicator.Measure(constraint);
                _floatingHeader.Measure(constraint);
            }
            return new Size(accumulatedWidth, maxHeight);
        }
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            GridViewColumnCollection columns = Columns;
            ControlArray children = InternalChildren;
            double accumulatedWidth = 0.0;
            double remainingWidth = arrangeSize.Width;
            Rect rect;
            HeadersPositionList.Clear();
            if (columns != null)
            {
                for (int i = 0; i < columns.Count; ++i)
                {
                    IControl child = children[GetVisualIndex(i)];
                    if (child == null) continue;
                    GridViewColumn column = columns[i];
                    double childArrangeWidth = Math.Min(remainingWidth, (column.State == ColumnMeasureState.SpecificWidth) ? column.Width : column.DesiredWidth);
                    rect = new Rect(accumulatedWidth, 0.0, childArrangeWidth, arrangeSize.Height);
                    child.Arrange(rect);
                    HeadersPositionList.Add(rect);
                    remainingWidth -= childArrangeWidth;
                    accumulatedWidth += childArrangeWidth;
                }
                if (_isColumnChangedOrCreated)
                {
                    for (int i = 0; i < columns.Count; ++i)
                    {
                        GridViewColumnHeader header = children[GetVisualIndex(i)] as GridViewColumnHeader;
                        header.CheckWidthForPreviousHeaderGripper();
                    }
                    _paddingHeader.CheckWidthForPreviousHeaderGripper();
                    _isColumnChangedOrCreated = false;
                }
            }
            Debug.Assert(_paddingHeader != null, "padding header is null");
            rect = new Rect(accumulatedWidth, 0.0, Math.Max(remainingWidth, 0.0), arrangeSize.Height);
            _paddingHeader.Arrange(rect);
            HeadersPositionList.Add(rect);
            if (_isHeaderDragging)
            {
                _floatingHeader.Arrange(new Rect(new Point(_currentPos.X - _relativeStartPos.X, 0), HeadersPositionList[_startColumnIndex].Size));
                Point pos = FindPositionByIndex(_desColumnIndex);
                _indicator.Arrange(new Rect(pos, new Size(_indicator.DesiredSize.Width, arrangeSize.Height)));
            }
            return arrangeSize;
        }
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                if (e.Source is GridViewColumnHeader header && AllowsColumnReorder)
                {
                    PrepareHeaderDrag(header, e.GetPosition(this), e.GetPosition(header), false);
                    MakeParentItemsControlGotFocus();
                }
                e.Handled = true;
            }
            base.OnPointerPressed(e);
        }
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            if (e.GetCurrentPoint(null).Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                _prepareDragging = false;
                if (_isHeaderDragging)
                    FinishHeaderDrag(false);
                e.Handled = true;
            }
            base.OnPointerReleased(e);
        }
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if (e.GetCurrentPoint(null).Properties.IsLeftButtonPressed)
            {
                if (_prepareDragging)
                {
                    Debug.Assert(_draggingSrcHeader != null, "_draggingSrcHeader is null");
                    _currentPos = e.GetPosition(this);
                    _desColumnIndex = FindIndexByPosition(_currentPos, true);
                    if (!_isHeaderDragging)
                    {
                        if (CheckStartHeaderDrag(_currentPos, _startPos))
                        {
                            StartHeaderDrag();
                            InvalidateMeasure();
                        }
                    }
                    else
                    {
                        bool isDisplayingFloatingHeader = IsMousePositionValid(_floatingHeader, _currentPos, 2.0);
                        _indicator.IsVisible = _floatingHeader.IsVisible = isDisplayingFloatingHeader;
                        InvalidateArrange();
                    }
                }
            }
            e.Handled = true;
        }
        internal override void OnColumnPropertyChanged(GridViewColumn column, string propertyName)
        {
            Debug.Assert(column != null);
            if (column.ActualIndex >= 0)
            {
                GridViewColumnHeader header = FindHeaderByColumn(column);
                if (header != null)
                {
                    if (GridViewColumn.WidthProperty.Name.Equals(propertyName) || GridViewColumn.c_ActualWidthName.Equals(propertyName))
                        InvalidateMeasure();
                    else if (GridViewColumn.HeaderProperty.Name.Equals(propertyName))
                    {
                        if (!header.IsInternalGenerated || column.Header is GridViewColumnHeader)
                        {
                            InternalChildren.IndexOf(header);
                            RemoveHeader(header, -1);
                            BuildHeaderLinks();
                        }
                        else
                        {
                            UpdateHeaderContent(header);
                        }
                    }
                    else
                    {
                        AvaloniaProperty columnDP = GetColumnDPFromName(propertyName);
                        if (columnDP != null)
                            UpdateHeaderProperty(header, columnDP);
                    }
                }
            }
        }
        internal override void OnColumnCollectionChanged(GridViewColumnCollectionChangedEventArgs e)
        {
            base.OnColumnCollectionChanged(e);
            int index;
            GridViewColumnHeader header;
            ControlArray children = Children;
            GridViewColumn column;
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Move:
                    int start = GetVisualIndex(e.OldStartingIndex);
                    int end = GetVisualIndex(e.NewStartingIndex);
                    header = (GridViewColumnHeader)children[start];
                    children.RemoveAt(start);
                    children.Insert(end, header);
                    break;
                case NotifyCollectionChangedAction.Add:
                    index = GetVisualIndex(e.NewStartingIndex);
                    column = (GridViewColumn)e.NewItems[0];
                    CreateAndInsertHeader(column, index + 1);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveHeader(null, GetVisualIndex(e.OldStartingIndex));
                    break;
                case NotifyCollectionChangedAction.Replace:
                    index = GetVisualIndex(e.OldStartingIndex);
                    RemoveHeader(null, index);
                    column = (GridViewColumn)e.NewItems[0];
                    CreateAndInsertHeader(column, index);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    int count = e.ClearedColumns.Count;
                    for (int i = 0; i < count; i++)
                        RemoveHeader(null, 1);
                    break;
            }
            BuildHeaderLinks();
            _isColumnChangedOrCreated = true;
        }
        internal void MakeParentItemsControlGotFocus() { }
        internal void UpdateHeaderProperty(GridViewColumnHeader header, AvaloniaProperty property)
        {
            GetMatchingDPs(property, out AvaloniaProperty gvDP, out AvaloniaProperty columnDP, out AvaloniaProperty headerDP);
            UpdateHeaderProperty(header, headerDP, columnDP, gvDP);
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
                        if (column.State == ColumnMeasureState.Init)
                            column.State = ColumnMeasureState.Headered;
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
        private int GetVisualIndex(int columnIndex)
        {
            int index = InternalChildren.Count - 3 - columnIndex;
            Debug.Assert(index >= 0 && index < InternalChildren.Count, "Error index when GetVisualIndex");
            return index;
        }
        private void BuildHeaderLinks()
        {
            GridViewColumnHeader lastHeader = null;
            if (Columns != null)
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    GridViewColumnHeader header = (GridViewColumnHeader)InternalChildren[GetVisualIndex(i)];
                    header.PreviousVisualHeader = lastHeader;
                    lastHeader = header;
                }
            }
            if (_paddingHeader != null)
                _paddingHeader.PreviousVisualHeader = lastHeader;
        }
        private GridViewColumnHeader CreateAndInsertHeader(GridViewColumn column, int index)
        {
            object header = column.Header;
            GridViewColumnHeader headerContainer = header as GridViewColumnHeader;
            if (header != null)
            {
                if (header is AvaloniaObject d)
                {
                    if (d is Visual headerAsVisual)
                    {
                        if (headerAsVisual.Parent is Visual parent)
                        {
                            if (headerContainer != null)
                            {
                                if (parent is GridViewHeaderRowPresenter parentAsGVHRP)
                                    parentAsGVHRP.InternalChildren.Remove(headerContainer);
                                else
                                    Debug.Assert(false, "Head is container for itself, but parent is neither GridViewHeaderRowPresenter nor null.");
                            }
                            else
                            {
                                if (parent is GridViewColumnHeader parentAsGVCH)
                                    parentAsGVCH.ClearValue(ContentControl.ContentProperty);
                            }
                        }
                    }
                }
            }
            if (headerContainer == null)
                headerContainer = new GridViewColumnHeader { IsInternalGenerated = true };
            headerContainer.SetValue(GridViewColumnHeader.ColumnProperty, column);
            HookupItemsControlKeyboardEvent(headerContainer);
            InternalChildren.Insert(index, headerContainer);
            UpdateHeader(headerContainer);
            _gvHeadersValid = false;
            return headerContainer;
        }
        private void RemoveHeader(GridViewColumnHeader header, int index)
        {
            Debug.Assert(header != null || index != -1);
            _gvHeadersValid = false;
            if (header != null)
                InternalChildren.Remove(header);
            else
            {
                header = (GridViewColumnHeader)InternalChildren[index];
                InternalChildren.RemoveAt(index);
            }
            UnhookItemsControlKeyboardEvent(header);
        }
        private void UnhookItemsControlKeyboardEvent(GridViewColumnHeader header)
        {
            Debug.Assert(header != null);
            if (_itemsControl != null)
                _itemsControl.KeyDown -= header.OnColumnHeaderKeyDown;
        }
        private void HookupItemsControlKeyboardEvent(GridViewColumnHeader header)
        {
            Debug.Assert(header != null);
            if (_itemsControl != null)
                _itemsControl.KeyDown += header.OnColumnHeaderKeyDown;
        }
        private void OnMasterScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_headerSV != null && _mainSV == e.Source) { }
        }
        private void OnHeaderScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (_mainSV != null && _headerSV == e.Source) { }
        }
        private void AddFloatingHeader(GridViewColumnHeader srcHeader)
        {
            GridViewColumnHeader header;
            Type headerType = srcHeader != null ? srcHeader.GetType() : typeof(GridViewColumnHeader);
            try
            {
                header = Activator.CreateInstance(headerType) as GridViewColumnHeader;
            }
            catch
            {
                throw new ArgumentException("Missing parameterless constructor for header type");
            }
            Debug.Assert(header != null, "Cannot instantiate GridViewColumnHeader in AddFloatingHeader");
            header.IsInternalGenerated = true;
            header.SetValue(GridViewColumnHeader.RoleProperty, GridViewColumnHeaderRole.Floating);
            header.IsVisible = false;
            InternalChildren.Add(header);
            _floatingHeader = header;
        }
        private void UpdateFloatingHeader(GridViewColumnHeader srcHeader)
        {
            Debug.Assert(srcHeader != null, "srcHeader is null");
            Debug.Assert(_floatingHeader != null, "floating header is null");
            _floatingHeader.Classes = srcHeader.Classes;
            _floatingHeader.FloatSourceHeader = srcHeader;
            _floatingHeader.Width = srcHeader.Bounds.Width;
            _floatingHeader.Height = srcHeader.Bounds.Height;
            _floatingHeader.SetValue(GridViewColumnHeader.ColumnProperty, srcHeader.Column);
            _floatingHeader.IsVisible = false;
            _floatingHeader.MinWidth = srcHeader.MinWidth;
            _floatingHeader.MinHeight = srcHeader.MinHeight;
            object template = srcHeader.GetValue(GridViewColumnHeader.ContentTemplateProperty);
            if ((template != AvaloniaProperty.UnsetValue) && (template != null))
                _floatingHeader.ContentTemplate = srcHeader.ContentTemplate;
            if (!(srcHeader.Content is Visual))
                _floatingHeader.Content = srcHeader.Content;
        }
        private bool CheckStartHeaderDrag(Point currentPos, Point originalPos)
        {
            return (MathUtilities.GreaterThan(Math.Abs(currentPos.X - originalPos.X), c_thresholdX));
        }
        private void OnColumnHeadersPresenterKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && _isHeaderDragging)
            {
                GridViewColumnHeader srcHeader = _draggingSrcHeader;
                FinishHeaderDrag(true);
                PrepareHeaderDrag(srcHeader, _currentPos, _relativeStartPos, true);
                InvalidateArrange();
            }
        }
        private GridViewColumnHeader FindHeaderByColumn(GridViewColumn column)
        {
            GridViewColumnCollection columns = Columns;
            ControlArray children = InternalChildren;
            if (columns != null && children.Count > columns.Count)
            {
                int index = columns.IndexOf(column);
                if (index != -1)
                {
                    int visualIndex = GetVisualIndex(index);
                    GridViewColumnHeader header = children[visualIndex] as GridViewColumnHeader;
                    if (header.Column != column)
                    {
                        for (int i = 1; i < children.Count; i++)
                        {
                            header = children[i] as GridViewColumnHeader;
                            if (header != null && header.Column == column)
                                return header;
                        }
                    }
                    else
                        return header;
                }
            }
            return null;
        }
        private int FindIndexByPosition(Point startPos, bool findNearestColumn)
        {
            int index = -1;
            if (startPos.X < 0.0)
                return 0;
            for (int i = 0; i < HeadersPositionList.Count; i++)
            {
                index++;
                Rect rect = HeadersPositionList[i];
                double startX = rect.X;
                double endX = startX + rect.Width;
                if (MathUtilities.GreaterThanOrClose(startPos.X, startX) && MathUtilities.LessThanOrClose(startPos.X, endX))
                {
                    if (findNearestColumn)
                    {
                        double midX = (startX + endX) * 0.5;
                        if (MathUtilities.GreaterThanOrClose(startPos.X, midX))
                        {
                            if (i != HeadersPositionList.Count - 1)
                                index++;
                        }
                    }
                    break;
                }
            }
            return index;
        }
        private Point FindPositionByIndex(int index)
        {
            Debug.Assert(index >= 0 && index < HeadersPositionList.Count, "wrong index");
            return new Point(HeadersPositionList[index].X, 0);
        }
        private void UpdateHeader(GridViewColumnHeader header)
        {
            UpdateHeaderContent(header);
            for (int i = 0, n = s_DPList[0].Length; i < n; i++)
                UpdateHeaderProperty(header, s_DPList[2][i], s_DPList[1][i], s_DPList[0][i]);
        }
        private void UpdateHeaderContent(GridViewColumnHeader header)
        {
            if (header != null && header.IsInternalGenerated)
            {
                GridViewColumn column = header.Column;
                if (column != null)
                {
                    if (column.Header == null)
                        header.ClearValue(ContentControl.ContentProperty);
                    else
                        header.Content = column.Header;
                }
            }
        }
        private void UpdateHeaderProperty(GridViewColumnHeader header, AvaloniaProperty targetDP, AvaloniaProperty columnDP, AvaloniaProperty gvDP)
        {
            GridViewColumn column = header.Column;
            object value = null;
            if (column != null && columnDP != null)
                value = column.GetValue(columnDP);
            if (value == null)
                value = this.GetValue(gvDP);
            header.UpdateProperty(targetDP, value);
        }
        private void PrepareHeaderDrag(GridViewColumnHeader header, Point pos, Point relativePos, bool cancelInvoke)
        {
            if (header.Role == GridViewColumnHeaderRole.Normal)
            {
                _prepareDragging = true;
                _isHeaderDragging = false;
                _draggingSrcHeader = header;
                _startPos = pos;
                _relativeStartPos = relativePos;
                if (!cancelInvoke)
                    _startColumnIndex = FindIndexByPosition(_startPos, false);
            }
        }
        private void StartHeaderDrag()
        {
            _startPos = _currentPos;
            _isHeaderDragging = true;
            _draggingSrcHeader.SuppressClickEvent = true;
            if (Columns != null)
                Columns.BlockWrite();
            InternalChildren.Remove(_floatingHeader);
            AddFloatingHeader(_draggingSrcHeader);
            UpdateFloatingHeader(_draggingSrcHeader);
        }
        private void FinishHeaderDrag(bool isCancel)
        {
            _prepareDragging = false;
            _isHeaderDragging = false;
            _draggingSrcHeader.SuppressClickEvent = false;
            _floatingHeader.IsVisible = false;
            _floatingHeader.ResetFloatingHeaderCanvasBackground();
            _indicator.IsVisible = false;
            if (Columns != null)
                Columns.UnblockWrite();
            if (!isCancel)
            {
                bool isMoveHeader = IsMousePositionValid(_floatingHeader, _currentPos, 2.0);
                Debug.Assert(Columns != null, "Columns is null in OnHeaderDragCompleted");
                int newColumnIndex = (_startColumnIndex >= _desColumnIndex) ? _desColumnIndex : _desColumnIndex - 1;
                if (isMoveHeader)
                    Columns.Move(_startColumnIndex, newColumnIndex);
            }
        }
        private static bool IsMousePositionValid(Control floatingHeader, Point currentPos, double arrange)
        {
            return MathUtilities.LessThanOrClose(-floatingHeader.Height * arrange, currentPos.Y) &&
                   MathUtilities.LessThanOrClose(currentPos.Y, floatingHeader.Height * (arrange + 1));
        }
        internal List<GridViewColumnHeader> ActualColumnHeaders
        {
            get
            {
                if (_gvHeaders == null || !_gvHeadersValid)
                {
                    _gvHeadersValid = true;
                    _gvHeaders = new List<GridViewColumnHeader>();
                    if (Columns != null)
                    {
                        ControlArray children = InternalChildren;
                        for (int i = 0, count = Columns.Count; i < count; ++i)
                        {
                            GridViewColumnHeader header = children[GetVisualIndex(i)] as GridViewColumnHeader;
                            if (header != null)
                                _gvHeaders.Add(header);
                        }
                    }
                }
                return _gvHeaders;
            }
        }
        private bool _gvHeadersValid;
        private List<GridViewColumnHeader> _gvHeaders;
        private List<Rect> HeadersPositionList
        {
            get
            {
                if (_headersPositionList == null)
                    _headersPositionList = new List<Rect>();
                return _headersPositionList;
            }
        }
        private List<Rect> _headersPositionList;
        private ScrollViewer _mainSV;
        private ScrollViewer _headerSV;
        private GridViewColumnHeader _paddingHeader;
        private GridViewColumnHeader _floatingHeader;
        private Separator _indicator;
        private ItemsControl _itemsControl;
        private GridViewColumnHeader _draggingSrcHeader;
        private Point _startPos;
        private Point _relativeStartPos;
        private Point _currentPos;
        private int _startColumnIndex;
        private int _desColumnIndex;
        private bool _isHeaderDragging;
        private bool _isColumnChangedOrCreated;
        private bool _prepareDragging;
        private const double c_thresholdX = 4.0;
        private static AvaloniaProperty GetColumnDPFromName(string dpName)
        {
            foreach (AvaloniaProperty dp in s_DPList[1])
            {
                if ((dp != null) && dpName.Equals(dp.Name))
                    return dp;
            }
            return null;
        }
        private static void GetMatchingDPs(AvaloniaProperty indexDP, out AvaloniaProperty gvDP, out AvaloniaProperty columnDP, out AvaloniaProperty headerDP)
        {
            for (int i = 0; i < s_DPList.Length; i++)
            {
                for (int j = 0; j < s_DPList[i].Length; j++)
                {
                    if (indexDP == s_DPList[i][j])
                    {
                        gvDP = s_DPList[0][j];
                        columnDP = s_DPList[1][j];
                        headerDP = s_DPList[2][j];
                        goto found;
                    }
                }
            }
            gvDP = columnDP = headerDP = null;
            found:;
        }
        private static readonly AvaloniaProperty[][] s_DPList = new AvaloniaProperty[][]
        {
            new AvaloniaProperty[] {
                ColumnHeaderTemplateProperty,
                ColumnHeaderStringFormatProperty,
                ColumnHeaderContextMenuProperty,
                ColumnHeaderToolTipProperty,
            },
            new AvaloniaProperty[] {
                GridViewColumn.HeaderTemplateProperty,
                GridViewColumn.HeaderStringFormatProperty,
                null,
                null,
            },
            new AvaloniaProperty[] {
                ContentControl.ContentTemplateProperty,
                ContextMenuProperty,
                ToolTip.TipProperty,
            }
        };
    }
}