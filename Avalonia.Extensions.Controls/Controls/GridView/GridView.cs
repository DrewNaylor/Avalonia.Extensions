using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System;
using System.ComponentModel;

namespace Avalonia.Extensions.Controls
{
    public class GridView : ViewBase
    {
        protected virtual void AddChild(object column)
        {
            if (column is GridViewColumn c)
                Columns.Add(c);
            else
                throw new InvalidOperationException("column is null");
        }
        protected virtual void AddText(string text)
        {
            AddChild(text);
        }
        public static GridViewColumnCollection GetColumnCollection(AvaloniaObject element)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            return element.GetValue(ColumnCollectionProperty);
        }
        public static void SetColumnCollection(AvaloniaObject element, GridViewColumnCollection collection)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            element.SetValue(ColumnCollectionProperty, collection);
        }
        public static readonly AttachedProperty<GridViewColumnCollection> ColumnCollectionProperty =
            AvaloniaProperty.RegisterAttached<GridView, Control, GridViewColumnCollection>("ColumnCollection");
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public GridViewColumnCollection Columns
        {
            get
            {
                if (_columns == null)
                    _columns = new GridViewColumnCollection { Owner = this, InViewMode = true };
                return _columns;
            }
        }
        public static readonly StyledProperty<IDataTemplate> ColumnHeaderTemplateProperty =
            AvaloniaProperty.Register<GridView, IDataTemplate>("ColumnHeaderTemplate");
        public IDataTemplate ColumnHeaderTemplate
        {
            get => GetValue(ColumnHeaderTemplateProperty);
            set => SetValue(ColumnHeaderTemplateProperty, value);
        }
        public static readonly StyledProperty<string> ColumnHeaderStringFormatProperty =
            AvaloniaProperty.Register<GridView, string>(nameof(ColumnHeaderStringFormat));
        public string ColumnHeaderStringFormat
        {
            get => GetValue(ColumnHeaderStringFormatProperty);
            set => SetValue(ColumnHeaderStringFormatProperty, value);
        }
        public static readonly StyledProperty<bool> AllowsColumnReorderProperty =
            AvaloniaProperty.Register<GridView, bool>(nameof(AllowsColumnReorder), true);
        public bool AllowsColumnReorder
        {
            get => GetValue(AllowsColumnReorderProperty);
            set => SetValue(AllowsColumnReorderProperty, value);
        }
        public static readonly StyledProperty<ContextMenu> ColumnHeaderContextMenuProperty =
            AvaloniaProperty.Register<GridView, ContextMenu>(nameof(ColumnHeaderContextMenu));
        public ContextMenu ColumnHeaderContextMenu
        {
            get => GetValue(ColumnHeaderContextMenuProperty);
            set => SetValue(ColumnHeaderContextMenuProperty, value);
        }
        public static readonly StyledProperty<ToolTip> ColumnHeaderToolTipProperty =
                AvaloniaProperty.Register<GridView, ToolTip>(nameof(ColumnHeaderToolTip));
        public ToolTip ColumnHeaderToolTip
        {
            get => GetValue(ColumnHeaderToolTipProperty);
            set => SetValue(ColumnHeaderToolTipProperty, value);
        }
        protected internal override void PrepareItem(ListViewItem item)
        {
            base.PrepareItem(item);
            SetColumnCollection(item, _columns);
        }
        protected internal override void ClearItem(ListViewItem item)
        {
            item.ClearValue(ColumnCollectionProperty);
            base.ClearItem(item);
        }
        private GridViewColumnCollection _columns;
        internal GridViewHeaderRowPresenter HeaderRowPresenter
        {
            get => _gvheaderRP;
            set => _gvheaderRP = value;
        }
        private GridViewHeaderRowPresenter _gvheaderRP;
    }
}