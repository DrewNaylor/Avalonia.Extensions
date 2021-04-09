using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Avalonia.Controls.Extensions
{
    public partial class HorizontalItemsRepeater : ItemsRepeater
    {
        private ICommand _command;
        internal static readonly Rect InvalidRect = new Rect(-1, -1, -1, -1);
        public static readonly RoutedEvent<RoutedEventArgs> ItemClickEvent =
            RoutedEvent.Register<HorizontalItemsRepeater, RoutedEventArgs>(nameof(ItemClick), RoutingStrategies.Bubble);
        public static readonly StyledProperty<bool> ClickEnableEvent =
          AvaloniaProperty.Register<StackLayout, bool>(nameof(ClickEnable), true);
        public static readonly StyledProperty<double> SpacingProperty =
          AvaloniaProperty.Register<StackLayout, double>(nameof(Spacing));
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
        public bool ClickEnable
        {
            get => GetValue(ClickEnableEvent);
            set => SetValue(ClickEnableEvent, value);
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
            get { return _command; }
            set { SetAndRaise(CommandProperty, ref _command, value); }
        }
        public HorizontalItemsRepeater() : base()
        {
            DrawLayout();
            this.Children.CollectionChanged += Children_CollectionChanged;
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
        /// <summary>
        /// add event linsten while child item has been clicked
        /// </summary>
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
        /// <summary>
        /// handle BINDING event
        /// </summary>
        /// <param name="unbind">unbind when there are "olds"</param>
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
        /// <summary>
        /// when the child item trigger the <seealso cref="ItemsRepeaterContent.Click"/>  event. do the same thing in this parent control,but named <seealso cref="ItemClick"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is ICommandSource commandControl)
            {
                var @event = new RoutedEventArgs(ItemClickEvent);
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