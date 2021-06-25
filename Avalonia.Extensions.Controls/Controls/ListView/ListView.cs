using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
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
        public static readonly RoutedEvent<ViewRoutedEventArgs> ItemClickEvent =
            RoutedEvent.Register<ListView, ViewRoutedEventArgs>(nameof(ItemClick), RoutingStrategies.Bubble);
        /// <summary>
        /// Defines the <see cref="Clickable"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ClickableProperty =
          AvaloniaProperty.Register<ListView, bool>(nameof(Clickable), true);
        /// <summary>
        /// Defines the <see cref="ScrollTop"/> event.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> ScrollTopEvent =
           RoutedEvent.Register<ListView, RoutedEventArgs>(nameof(ScrollTop), RoutingStrategies.Bubble);
        /// <summary>
        /// Defines the <see cref="ScrollEnd"/> event.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> ScrollEndEvent =
           RoutedEvent.Register<ListView, RoutedEventArgs>(nameof(ScrollEnd), RoutingStrategies.Bubble);
        /// <summary>
        /// Defines the <see cref="Command"/> property.
        /// </summary>
        public static readonly DirectProperty<ListView, ICommand> CommandProperty =
             AvaloniaProperty.RegisterDirect<ListView, ICommand>(nameof(Command), content => content.Command, (content, command) => content.Command = command, enableDataValidation: true);
        /// <summary>
        /// Raised when the user clicks the child item.
        /// </summary>
        public event EventHandler<ViewRoutedEventArgs> ItemClick
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
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<RoutedEventArgs> ScrollTop
        {
            add { AddHandler(ScrollTopEvent, value); }
            remove { RemoveHandler(ScrollTopEvent, value); }
        }
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<RoutedEventArgs> ScrollEnd
        {
            add { AddHandler(ScrollEndEvent, value); }
            remove { RemoveHandler(ScrollEndEvent, value); }
        }
        private ICommand _command;
        private MouseButton _mouseButton;
        public MouseButton MouseClickButton => _mouseButton;
        static ListView()
        {
            ViewProperty.Changed.AddClassHandler<Grid>(OnViewChanged);
            SelectionModeProperty.OverrideMetadata<ListView>(new StyledPropertyMetadata<SelectionMode>(SelectionMode.Multiple));
        }
        private void OnScrollChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is ScrollViewer scrollViewer)
                scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
        }
        private bool trigger = false;
        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.Source is ScrollViewer scrollViewer)
            {
                if (scrollViewer.Content is IControl child && child.VisualChildren.FirstOrDefault() is VirtualizingStackPanel virtualizing)
                {
                    if (virtualizing.Children.FirstOrDefault() is ListBoxItem firstItem && Items.IsFirst(firstItem.Content))
                    {
                        if (!trigger)
                        {
                            trigger = true;
                            var args = new RoutedEventArgs(ScrollTopEvent);
                            RaiseEvent(args);
                            if (!args.Handled)
                                args.Handled = true;
                        }
                        else
                            trigger = false;
                    }
                    else if (virtualizing.Children.LastOrDefault() is ListBoxItem lastItem && Items.IsLast(lastItem.Content))
                    {
                        if (!trigger)
                        {
                            trigger = true;
                            var args = new RoutedEventArgs(ScrollEndEvent);
                            RaiseEvent(args);
                            if (!args.Handled)
                                args.Handled = true;
                        }
                        else
                            trigger = false;
                    }
                }
            }
        }
        public ListView()
        {
            SelectionChangedEvent.Raised.Subscribe(OnSelectionChanged);
            ScrollProperty.Changed.AddClassHandler<ListView>(OnScrollChange);
            LogicalChildren.CollectionChanged += LogicalChildren_CollectionChanged;
        }
        private void Item_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (sender is ListBoxItem item && Clickable && (e.GetCurrentPoint(this).Properties.IsRightButtonPressed || e.GetCurrentPoint(this).Properties.IsLeftButtonPressed))
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var @event = new ViewRoutedEventArgs(ItemClickEvent, MouseButton.Right, item);
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
                OnContentClick(listBoxItem, MouseButton.Left);
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
        internal virtual void OnContentClick(object control, MouseButton mouseButton)
        {
            if (Clickable == true && control != null)
            {
                _mouseButton = mouseButton;
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var listItem = (control as Control)?.Parent;
                    var @event = new ViewRoutedEventArgs(ItemClickEvent, mouseButton, listItem);
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