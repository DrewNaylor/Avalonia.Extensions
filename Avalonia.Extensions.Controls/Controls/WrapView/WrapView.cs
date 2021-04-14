using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using System.Windows.Input;

namespace Avalonia.Extensions.Controls
{
    public class WrapView : ListView
    {
        /// <summary>
        /// Defines the <see cref="ItemClick"/> property.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> ItemClickEvent =
            RoutedEvent.Register<WrapView, RoutedEventArgs>(nameof(ItemClick), RoutingStrategies.Bubble);
        /// <summary>
        /// Raised when the user clicks the child item.
        /// </summary>
        public event EventHandler<RoutedEventArgs> ItemClick
        {
            add { AddHandler(ItemClickEvent, value); }
            remove { RemoveHandler(ItemClickEvent, value); }
        }
        /// <summary>
        /// Defines the <see cref="Clickable"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> ClickableProperty =
          AvaloniaProperty.Register<WrapView, bool>(nameof(Clickable), true);
        /// <summary>
        /// is item clickenable,default value is true
        /// </summary>
        public bool Clickable
        {
            get => GetValue(ClickableProperty);
            set => SetValue(ClickableProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="Command"/> property.
        /// </summary>
        public static readonly DirectProperty<WrapView, ICommand> CommandProperty =
             AvaloniaProperty.RegisterDirect<WrapView, ICommand>(nameof(Command), content => content.Command, (content, command) => content.Command = command, enableDataValidation: true);
        private ICommand _command;
        /// <summary>
        /// Gets or sets an <see cref="ICommand"/> to be invoked when the child item is clicked.
        /// </summary>
        public ICommand Command
        {
            get => _command;
            set => SetAndRaise(CommandProperty, ref _command, value);
        }
        internal void OnContentClick(WrapViewCell wrapViewCell)
        {
            if (Clickable == true && wrapViewCell != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    var @event = new RoutedEventArgs(ItemClickEvent);
                    this.SelectedItem = wrapViewCell;
                    RaiseEvent(@event);
                    if (!@event.Handled && Command?.CanExecute(wrapViewCell.CommandParameter) == true)
                    {
                        Command.Execute(wrapViewCell.CommandParameter);
                        @event.Handled = true;
                    }
                });
            }
        }
    }
}