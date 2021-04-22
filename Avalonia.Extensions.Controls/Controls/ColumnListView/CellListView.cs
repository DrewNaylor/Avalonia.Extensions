using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Extensions.Controls.Utils;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Threading;
using System;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// the uwp like "GridView", it just define itempanel with wrappanel
    /// you need to set "ColumnNum" for columns count
    /// https://stackoverflow.com/questions/23084576/wpf-combobox-multiple-columns
    /// </summary>
    public class CellListView : ListView
    {
        private ICommand _command;
        /// <summary>
        /// The width of each cell.
        /// </summary>
        public double CellWidth { get; private set; }
        /// <summary>
        /// Defines the <see cref="Clickable"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ClickableProperty =
          AvaloniaProperty.Register<CellListView, bool>(nameof(Clickable), true);
        /// <summary>
        /// Defines the <see cref="ColumnNum"/> property.
        /// </summary>
        public static readonly StyledProperty<int> ColumnNumProperty =
          AvaloniaProperty.Register<CellListView, int>(nameof(ColumnNum), 1);
        /// <summary>
        /// Defines the <see cref="ItemClick"/> property.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> ItemClickEvent =
            RoutedEvent.Register<CellListView, RoutedEventArgs>(nameof(ItemClick), RoutingStrategies.Bubble);
        /// <summary>
        /// Defines the <see cref="Command"/> property.
        /// </summary>
        public static readonly DirectProperty<CellListView, ICommand> CommandProperty =
             AvaloniaProperty.RegisterDirect<CellListView, ICommand>(nameof(Command), content => content.Command, (content, command) => content.Command = command, enableDataValidation: true);
        /// <summary>
        /// Raised when the user clicks the child item.
        /// </summary>
        public event EventHandler<RoutedEventArgs> ItemClick
        {
            add { AddHandler(ItemClickEvent, value); }
            remove { RemoveHandler(ItemClickEvent, value); }
        }
        /// <summary>
        /// Gets or sets an <see cref="ICommand"/> to be invoked when the child item is clicked.
        /// </summary>
        public ICommand Command
        {
            get => _command;
            set => SetAndRaise(CommandProperty, ref _command, value);
        }
        /// <summary>
        /// is item clickenable,default value is true
        /// </summary>
        public bool Clickable
        {
            get => GetValue(ClickableProperty);
            set => SetValue(ClickableProperty, value);
        }
        /// <summary>
        /// get or set column number.
        /// default value is 1.if the value smaller than 1 it's means depend layout by item controls
        /// </summary>
        public int ColumnNum
        {
            get => GetValue(ColumnNumProperty);
            set
            {
                if (value > 0)
                    CellWidth = Bounds.Width / value;
                else
                    CellWidth = double.NaN;
                SetValue(ColumnNumProperty, value);
            }
        }
        /// <summary>
        /// create a instance
        /// </summary>
        public CellListView()
        {
            ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Disabled);
            var target = AvaloniaRuntimeXamlLoader.Parse<ItemsPanelTemplate>(Core.WRAP_TEMPLATE);
            SetValue(ItemsPanelProperty, target);
            BoundsProperty.Changed.AddClassHandler<CellListView>(OnBoundsChange);
            LogicalChildren.CollectionChanged += LogicalChildren_CollectionChanged;
        }
        private void OnBoundsChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is Rect rect)
            {
                if (CellWidth != double.NaN)
                {
                    CellWidth = rect.Width / ColumnNum;
                    for (var index = 0; index < LogicalChildren.Count; index++)
                    {
                        var item = LogicalChildren.ElementAt(index);
                        SetItemWidth(item);
                    }
                }
            }
        }
        private void SetItemWidth(object item)
        {
            if (item is ListBoxItem obj)
            {
                obj.Width = CellWidth;
                obj.HorizontalAlignment = Layout.HorizontalAlignment.Center;
            }
        }
        private void LogicalChildren_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems.Count > 0)
            {
                if (CellWidth != double.NaN)
                {
                    var item = e.NewItems.ElementAt(0);
                    SetItemWidth(item);
                }
            }
        }
        /// <summary>
        /// handle clild item click event,
        /// trigger the <seealso cref="Command"/> and <seealso cref="ItemClickEvent"/>
        /// when child item has been click
        /// </summary>
        /// <param name="viewCell"></param>
        internal void OnContentClick(CellListViewCell viewCell)
        {
            if (Clickable == true && viewCell != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var @event = new RoutedEventArgs(ItemClickEvent);
                    this.SelectedItem = viewCell;
                    RaiseEvent(@event);
                    if (!@event.Handled && Command?.CanExecute(viewCell.CommandParameter) == true)
                    {
                        Command.Execute(viewCell.CommandParameter);
                        @event.Handled = true;
                    }
                });
            }
        }
    }
}