using Avalonia.Controls;
using Avalonia.Extensions.Controls.Utils;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Windows.Input;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// fork from https://github.com/jhofinger/Avalonia/tree/listview
    /// </summary>
    public class ListView : ListBox, IStyleable
    {
        /// <summary>
        /// Defines the <see cref="ItemClick"/> property.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> ItemClickEvent =
            RoutedEvent.Register<ListView, RoutedEventArgs>(nameof(ItemClick), RoutingStrategies.Bubble);
        /// <summary>
        /// Defines the <see cref="Clickable"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ClickableProperty =
          AvaloniaProperty.Register<ListView, bool>(nameof(Clickable), true);
        /// <summary>
        /// Defines the <see cref="Command"/> property.
        /// </summary>
        public static readonly DirectProperty<ListView, ICommand> CommandProperty =
             AvaloniaProperty.RegisterDirect<ListView, ICommand>(nameof(Command), content => content.Command, (content, command) => content.Command = command, enableDataValidation: true);
        /// <summary>
        /// Defines the <see cref="ItemRightClick"/> property.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> ItemRightClickEvent =
            RoutedEvent.Register<ListView, RoutedEventArgs>(nameof(ItemRightClick), RoutingStrategies.Bubble);
        /// <summary>
        /// Raised when the user clicks the child item.
        /// </summary>
        public event EventHandler<RoutedEventArgs> ItemRightClick
        {
            add { AddHandler(ItemRightClickEvent, value); }
            remove { RemoveHandler(ItemRightClickEvent, value); }
        }
        /// <summary>
        /// Raised when the user clicks the child item.
        /// </summary>
        public event EventHandler<RoutedEventArgs> ItemClick
        {
            add { AddHandler(ItemClickEvent, value); }
            remove { RemoveHandler(ItemClickEvent, value); }
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
        /// Gets or sets an <see cref="ICommand"/> to be invoked when the child item is clicked.
        /// </summary>
        public ICommand Command
        {
            get => _command;
            set => SetAndRaise(CommandProperty, ref _command, value);
        }
        private ICommand _command;
        static ListView()
        {
            ViewProperty.Changed.AddClassHandler<Grid>(OnViewChanged);
            SelectionModeProperty.OverrideMetadata<ListView>(new StyledPropertyMetadata<SelectionMode>(SelectionMode.Multiple));
        }
        public ListView()
        {
            SelectionChangedEvent.Raised.Subscribe(OnSelectionChanged);
            LogicalChildren.CollectionChanged += LogicalChildren_CollectionChanged;
        }
        private void Item_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (sender is ListBoxItem item && Clickable && e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var @event = new RoutedEventArgs(ItemRightClickEvent);
                    this.SelectedItem = item;
                    RaiseEvent(@event);
                    if (!@event.Handled)
                        @event.Handled = true;
                });
            }
        }
        private void LogicalChildren_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            AddListen(e.NewItems);
            ClearListen(e.OldItems);
        }
        private void AddListen(IList arr)
        {
            try
            {
                if (arr != null && arr.Count > 0)
                {
                    for (var idx = 0; idx < arr.Count; idx++)
                    {
                        var item = arr.ElementAt(idx);
                        if (item is ListBoxItem obj)
                            obj.PointerPressed += Item_PointerPressed;
                    }
                }
            }
            catch { }
        }
        private void ClearListen(IList arr)
        {
            try
            {
                if (arr != null && arr.Count > 0)
                {
                    for (var idx = 0; idx < arr.Count; idx++)
                    {
                        var item = arr.ElementAt(idx);
                        if (item is ListBoxItem obj)
                            obj.PointerPressed -= Item_PointerPressed;
                    }
                }
            }
            catch { }
        }
        private void OnSelectionChanged((object, RoutedEventArgs) obj)
        {
            if (obj.Item2.Source is ListBoxItem listBoxItem)
                OnContentClick(listBoxItem);
        }
        public static readonly AvaloniaProperty ViewProperty = AvaloniaProperty.Register<ListView, ViewBase>(nameof(View));
        public ViewBase View
        {
            get => (ViewBase)GetValue(ViewProperty);
            set => SetValue(ViewProperty, value);
        }
        private static void OnViewChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            if (d is ListView listView)
            {
                ViewBase oldView = (ViewBase)e.OldValue;
                ViewBase newView = (ViewBase)e.NewValue;
                if (newView != null)
                {
                    if (newView.IsUsed)
                        throw new InvalidOperationException("View cannot be shared between multiple instances of ListView");
                    newView.IsUsed = true;
                }
                listView.PreviousView = oldView;
                listView.ApplyNewView();
                listView.PreviousView = newView;
                if (oldView != null)
                    oldView.IsUsed = false;
            }
        }
        public Type Defaultstyle { get; private set; }
        Type IStyleable.StyleKey => typeof(ListBox);
        private void ApplyNewView()
        {
            ViewBase newView = View;
            if (newView != null)
                Defaultstyle = newView.DefaultStyleKey;
            else
                Defaultstyle = null;
        }
        public ViewBase PreviousView { get; private set; }
        /// <summary>
        /// handle clild item click event,
        /// trigger the <seealso cref="Command"/> and <seealso cref="ItemClickEvent"/>
        /// when child item has been click
        /// </summary>
        /// <param name="viewCell"></param>
        internal virtual void OnContentClick(object control)
        {
            if (Clickable == true && control != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var @event = new RoutedEventArgs(ItemClickEvent);
                    RaiseEvent(@event);
                    if (control is CellListViewCell viewCell)
                    {
                        this.SelectedItem = viewCell;
                        if (!@event.Handled && Command?.CanExecute(viewCell.CommandParameter) == true)
                        {
                            Command.Execute(viewCell.CommandParameter);
                            @event.Handled = true;
                        }
                    }
                });
            }
        }
    }
}