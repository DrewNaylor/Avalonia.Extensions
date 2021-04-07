using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Avalonia.Controls.ItemsRepeaterEx
{
    public sealed class HorizontalItemsRepeater : ItemsRepeater
    {
        private ICommand _command;
        public static readonly RoutedEvent<RoutedEventArgs> ClickEvent = 
            RoutedEvent.Register<HorizontalItemsRepeater, RoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble); 
        public static readonly DirectProperty<HorizontalItemsRepeater, ICommand> CommandProperty =
             AvaloniaProperty.RegisterDirect<HorizontalItemsRepeater, ICommand>(nameof(Command),
                 content => content.Command, (content, command) => content.Command = command, enableDataValidation: true);
        public event EventHandler<RoutedEventArgs> Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
        public ICommand Command
        {
            get { return _command; }
            set { SetAndRaise(CommandProperty, ref _command, value); }
        }
        public HorizontalItemsRepeater() : base()
        {
            Layout = new StackLayout { Orientation = Orientation.Horizontal };
            this.Children.CollectionChanged += Children_CollectionChanged;
        }
        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                if (e.NewItems != null && e.NewItems.Count > 0)
                    Binding(e.NewItems.GetEnumerator(), false);
                if (e.OldItems != null && e.OldItems.Count > 0)
                    Binding(e.OldItems.GetEnumerator(), true);
            });
        }
        private void Binding(IEnumerator enumerator, bool unbind)
        {
            try
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current is ItemsRepeaterContent content)
                    {
                        if (!unbind)
                            content.Click += Button_Click;
                        else
                            content.Click -= Button_Click;
                    }
                }
            }
            catch { }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ICommandSource commandControl)
            {
                var @event = new RoutedEventArgs(ClickEvent);
                RaiseEvent(@event);
                if (!@event.Handled && Command?.CanExecute(commandControl.CommandParameter) == true)
                {
                    Command.Execute(commandControl.CommandParameter);
                    @event.Handled = true;
                }
            }
        }
    }
}