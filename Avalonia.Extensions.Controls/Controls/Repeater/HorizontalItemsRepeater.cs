using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Threading;
using System;
using System.Windows.Input;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class HorizontalItemsRepeater : ItemsRepeater
    {
        private ICommand _command;
        /// <summary>
        /// 
        /// </summary>
        public HorizontalItemsRepeater()
        {
            DrawLayout();
        }
        /// <summary>
        /// Defines the <see cref="ItemClick"/> property.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> ItemClickEvent =
            RoutedEvent.Register<HorizontalItemsRepeater, RoutedEventArgs>(nameof(ItemClick), RoutingStrategies.Bubble);
        /// <summary>
        /// Defines the <see cref="SelectedItem"/> property.
        /// </summary>
        public static readonly StyledProperty<ClickableView> SelectedItemProperty =
          AvaloniaProperty.Register<HorizontalItemsRepeater, ClickableView>(nameof(SelectedItem), null);
        /// <summary>
        /// Defines the <see cref="Clickable"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ClickableProperty =
          AvaloniaProperty.Register<HorizontalItemsRepeater, bool>(nameof(Clickable), true);
        /// <summary>
        /// Defines the <see cref="Spacing"/> property.
        /// </summary>
        public static readonly StyledProperty<double> SpacingProperty =
          AvaloniaProperty.Register<HorizontalItemsRepeater, double>(nameof(Spacing));
        /// <summary>
        /// Defines the <see cref="Command"/> property.
        /// </summary>
        public static readonly DirectProperty<HorizontalItemsRepeater, ICommand> CommandProperty =
             AvaloniaProperty.RegisterDirect<HorizontalItemsRepeater, ICommand>(nameof(Command), content => content.Command, (content, command) => content.Command = command, enableDataValidation: true);
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
        /// Gets or sets the clicked child item
        /// </summary>
        public ClickableView SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        /// <summary>
        ///  Gets or sets a uniform distance (in pixels) between stacked items. It is applied
        ///  in the direction of the StackLayout's Orientation.
        /// </summary>
        public double Spacing
        {
            get => GetValue(SpacingProperty);
            set
            {
                SetValue(SpacingProperty, value);
                DrawLayout();
            }
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
        /// <param name="itemsRepeaterContent"></param>
        internal void OnContentClick(ClickableView itemsRepeaterContent)
        {
            if (Clickable == true && itemsRepeaterContent != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var @event = new RoutedEventArgs(ItemClickEvent);
                    this.SelectedItem = itemsRepeaterContent;
                    RaiseEvent(@event);
                    if (!@event.Handled && Command?.CanExecute(itemsRepeaterContent.CommandParameter) == true)
                    {
                        Command.Execute(itemsRepeaterContent.CommandParameter);
                        @event.Handled = true;
                    }
                });
            }
        }
        /// <summary>
        /// when spacing change,draw <seealso cref="Layout"/> again
        /// </summary>
        private void DrawLayout()
        {
            Layout = new StackLayout
            {
                Spacing = Spacing,
                Orientation = Orientation.Horizontal
            };
        }
    }
}