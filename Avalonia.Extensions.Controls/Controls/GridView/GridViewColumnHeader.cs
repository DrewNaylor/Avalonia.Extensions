using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Utilities;
using System;
using System.Diagnostics;

namespace Avalonia.Extensions.Controls
{
    public enum GridViewColumnHeaderRole
    {
        Normal,
        Floating,
        Padding
    }
#if OLD_AUTOMATION
    [Automation(AccessibilityControlType = "Button")]
#endif
    public class GridViewColumnHeader : Button, IStyleable
#if OLD_AUTOMATION
    , IInvokeProvider
#endif
    {
        Type IStyleable.StyleKey => typeof(GridViewColumnHeader);
        static GridViewColumnHeader()
        {
            FocusableProperty.OverrideMetadata<GridViewColumnHeader>(new StyledPropertyMetadata<bool>(false));
            ContentTemplateProperty.Changed.AddClassHandler<GridViewColumnHeader>(PropertyChanged);
            ContextMenuProperty.Changed.AddClassHandler<GridViewColumnHeader>(PropertyChanged);
            ToolTip.TipProperty.Changed.AddClassHandler<GridViewColumn>(PropertyChanged);
        }
        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);
            GridViewColumnHeaderRole role = Role;
            if (role == GridViewColumnHeaderRole.Normal)
                HookupGripperEvents();
        }

        public static readonly AvaloniaProperty ColumnProperty = AvaloniaProperty.Register<GridViewColumnHeader, GridViewColumn>(nameof(Column), null);
        public GridViewColumn Column
        {
            get { return (GridViewColumn)GetValue(ColumnProperty); }
        }
        public static readonly AvaloniaProperty RoleProperty = AvaloniaProperty.Register<GridViewColumnHeader, GridViewColumnHeaderRole>(nameof(Role), GridViewColumnHeaderRole.Normal);
        public GridViewColumnHeaderRole Role
        {
            get { return (GridViewColumnHeaderRole)GetValue(RoleProperty); }
        }
#if OLD_AUTOMATION
        void IInvokeProvider.Invoke()
        {
            IsAccessKeyOrAutomation = true;
            OnClick();
        }
#endif
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            e.Handled = false;
        }
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            e.Handled = false;
        }
        protected override void OnPointerMoved(PointerEventArgs e)
        {
            base.OnPointerMoved(e);
            if ((IsPointerOver && e.GetCurrentPoint(null).Properties.IsLeftButtonPressed))
                SetValue(IsPressedProperty, true);
            e.Handled = false;
        }
        protected override void OnClick()
        {
            if (!SuppressClickEvent)
            {
                if (IsAccessKeyOrAutomation || !IsMouseOutside())
                {
                    IsAccessKeyOrAutomation = false;
                    MakeParentGotFocus();
                }
            }
        }
        internal void AutomationClick()
        {
            IsAccessKeyOrAutomation = true;
            OnClick();
        }
        internal void OnColumnHeaderKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && _headerGripper != null) { }
        }
        internal void CheckWidthForPreviousHeaderGripper()
        {
            bool hideGripperRightHalf = false;
            if (_headerGripper != null)
                hideGripperRightHalf = Bounds.Width < _headerGripper.Width;
            if (_previousHeader != null)
                _previousHeader.HideGripperRightHalf(hideGripperRightHalf);
            UpdateGripperCursor();
        }
        internal void ResetFloatingHeaderCanvasBackground()
        {
            if (_floatingHeaderCanvas != null)
                _floatingHeaderCanvas.Background = null;
        }
        internal void UpdateProperty(AvaloniaProperty dp, object value)
        {
            Flags ignoreFlag = Flags.None;
            if (!IsInternalGenerated)
            {
                PropertyToFlags(dp, out Flags flag, out ignoreFlag);
                Debug.Assert(flag != Flags.None && ignoreFlag != Flags.None, "Invalid parameter dp.");
                if (GetFlag(flag))
                    return;
                else
                    SetFlag(ignoreFlag, true);
            }
            if (value != null)
                SetValue(dp, value);
            else
                ClearValue(dp);
            SetFlag(ignoreFlag, false);
        }
        internal GridViewColumnHeader PreviousVisualHeader
        {
            get { return _previousHeader; }
            set { _previousHeader = value; }
        }
        private GridViewColumnHeader _previousHeader;
        internal bool SuppressClickEvent
        {
            get { return GetFlag(Flags.SuppressClickEvent); }
            set { SetFlag(Flags.SuppressClickEvent, value); }
        }
        internal GridViewColumnHeader FloatSourceHeader
        {
            get { return _srcHeader; }
            set { _srcHeader = value; }
        }
        internal bool IsInternalGenerated
        {
            get { return GetFlag(Flags.IsInternalGenerated); }
            set { SetFlag(Flags.IsInternalGenerated, value); }
        }
        private static void PropertyChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            GridViewColumnHeader header = (GridViewColumnHeader)d;
            if (!header.IsInternalGenerated)
            {
                PropertyToFlags(e.Property, out Flags flag, out Flags ignoreFlag);
                if (!header.GetFlag(ignoreFlag))
                {
                    header.SetFlag(flag, false);
                    if (header.Parent is GridViewHeaderRowPresenter headerRowPresenter)
                        headerRowPresenter.UpdateHeaderProperty(header, e.Property);
                }
            }
        }
        private static void PropertyToFlags(AvaloniaProperty dp, out Flags flag, out Flags ignoreFlag)
        {
            if (dp == ContentTemplateProperty)
            {
                flag = Flags.ContentTemplateSetByUser;
                ignoreFlag = Flags.IgnoreContentTemplate;
            }
            else if (dp == ContextMenuProperty)
            {
                flag = Flags.ContextMenuSetByUser;
                ignoreFlag = Flags.IgnoreContextMenu;
            }
            else if (dp == ToolTip.TipProperty)
            {
                flag = Flags.ToolTipSetByUser;
                ignoreFlag = Flags.IgnoreToolTip;
            }
            else
            {
                flag = ignoreFlag = Flags.None;
            }
        }
        private void HideGripperRightHalf(bool hide)
        {
            if (_headerGripper != null)
            {
                if (_headerGripper.Parent is Control gripperContainer)
                    gripperContainer.ClipToBounds = hide;
            }
        }
        private void OnColumnHeaderGripperDragStarted(object sender, VectorEventArgs e)
        {
            MakeParentGotFocus();
            _originalWidth = ColumnActualWidth;
            e.Handled = true;
        }
        private void MakeParentGotFocus()
        {
            if (Parent is GridViewHeaderRowPresenter headerRP)
                headerRP.MakeParentItemsControlGotFocus();
        }
        private void OnColumnHeaderResize(object sender, VectorEventArgs e)
        {
            double width = ColumnActualWidth + e.Vector.X;
            if (MathUtilities.LessThanOrClose(width, 0.0))
                width = 0.0;
            UpdateColumnHeaderWidth(width);
            e.Handled = true;
        }
        private void OnColumnHeaderGripperDragCompleted(object sender, VectorEventArgs e)
        {
            if (e.Handled)
                UpdateColumnHeaderWidth(_originalWidth);
            UpdateGripperCursor();
            e.Handled = true;
        }
        private void HookupGripperEvents()
        {
            UnhookGripperEvents();
            if (this.FindNameScope().Find(HeaderGripperTemplateName) is Thumb _headerGripper)
            {
                _headerGripper.DragStarted += OnColumnHeaderGripperDragStarted;
                _headerGripper.DragDelta += OnColumnHeaderResize;
                _headerGripper.DragCompleted += OnColumnHeaderGripperDragCompleted;
                _headerGripper.DoubleTapped += OnGripperDoubleClicked;
                _headerGripper.PointerEnter += OnGripperMouseEnterLeave;
                _headerGripper.PointerLeave += OnGripperMouseEnterLeave;
                _headerGripper.Cursor = SplitCursor;
            }
        }
        private void OnGripperDoubleClicked(object sender, RoutedEventArgs e)
        {
            if (Column != null)
            {
                if (double.IsNaN(Column.Width))
                    Column.Width = Column.ActualWidth;
                Column.Width = double.NaN;
                e.Handled = true;
            }
        }
        private void UnhookGripperEvents()
        {
            if (_headerGripper != null)
            {
                _headerGripper.DragStarted -= OnColumnHeaderGripperDragStarted;
                _headerGripper.DragDelta -= OnColumnHeaderResize;
                _headerGripper.DragCompleted -= OnColumnHeaderGripperDragCompleted;
                _headerGripper.DoubleTapped -= OnGripperDoubleClicked;
                _headerGripper.PointerEnter -= OnGripperMouseEnterLeave;
                _headerGripper.PointerLeave -= OnGripperMouseEnterLeave;
                _headerGripper = null;
            }
        }
        private Cursor GetCursor(int cursorID)
        {
            Debug.Assert(cursorID == c_SPLIT || cursorID == c_SPLITOPEN, "incorrect cursor type");
            Cursor cursor = null;
            if (cursorID == c_SPLIT)
                cursor = new Cursor(StandardCursorType.SizeWestEast);
            return cursor;
        }
        private void UpdateGripperCursor()
        {
            if (_headerGripper != null)
            {
                Cursor gripperCursor;
                if (ColumnActualWidth == 0)
                    gripperCursor = SplitOpenCursor;
                else
                    gripperCursor = SplitCursor;
                Debug.Assert(gripperCursor != null, "gripper cursor is null");
                if (gripperCursor != null)
                    _headerGripper.Cursor = gripperCursor;
            }
        }
        private void UpdateColumnHeaderWidth(double width)
        {
            if (Column != null)
                Column.Width = width;
            else
                Width = width;
        }
        private bool IsMouseOutside() => true;
        private bool GetFlag(Flags flag)
        {
            return (_flags & flag) == flag;
        }
        private void SetFlag(Flags flag, bool set)
        {
            if (set)
                _flags |= flag;
            else
                _flags &= (~flag);
        }
        private void UpdateFloatingHeaderCanvas()
        {
            if (_floatingHeaderCanvas != null && FloatSourceHeader != null)
            {
                VisualBrush visualBrush = new VisualBrush(FloatSourceHeader);
                _floatingHeaderCanvas.Background = visualBrush;
                FloatSourceHeader = null;
            }
        }
        private bool HandleIsMouseOverChanged() => false;
        private void OnGripperMouseEnterLeave(object sender, PointerEventArgs e)
        {
            HandleIsMouseOverChanged();
        }
        private Cursor SplitCursor
        {
            get
            {
                if (_splitCursorCache == null)
                    _splitCursorCache = GetCursor(c_SPLIT);
                return _splitCursorCache;
            }
        }
        static private Cursor _splitCursorCache = null;
        private Cursor SplitOpenCursor
        {
            get
            {
                if (_splitOpenCursorCache == null)
                    _splitOpenCursorCache = GetCursor(c_SPLITOPEN);
                return _splitOpenCursorCache;
            }
        }
        static private Cursor _splitOpenCursorCache = null;
        private bool IsAccessKeyOrAutomation
        {
            get { return GetFlag(Flags.IsAccessKeyOrAutomation); }
            set { SetFlag(Flags.IsAccessKeyOrAutomation, value); }
        }
        private double ColumnActualWidth
        {
            get { return (Column != null ? Column.ActualWidth : Bounds.Width); }
        }
        [Flags]
        private enum Flags
        {
            None = 0,
            StyleSetByUser = 0x00000001,
            IgnoreStyle = 0x00000002,
            ContentTemplateSetByUser = 0x00000004,
            IgnoreContentTemplate = 0x00000008,
            ContentTemplateSelectorSetByUser = 0x00000010,
            IgnoreContentTemplateSelector = 0x00000020,
            ContextMenuSetByUser = 0x00000040,
            IgnoreContextMenu = 0x00000080,
            ToolTipSetByUser = 0x00000100,
            IgnoreToolTip = 0x00000200,
            SuppressClickEvent = 0x00000400,
            IsInternalGenerated = 0x00000800,
            IsAccessKeyOrAutomation = 0x00001000,
            ContentStringFormatSetByUser = 0x00002000,
            IgnoreContentStringFormat = 0x00004000,
        }
        private Flags _flags;
        private Thumb _headerGripper;
        private double _originalWidth;
        private Canvas _floatingHeaderCanvas;
        private GridViewColumnHeader _srcHeader;
        private const int c_SPLIT = 100;
        private const int c_SPLITOPEN = 101;
        private const string HeaderGripperTemplateName = "PART_HeaderGripper";
    }
}