using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using System;
using System.Linq;
using System.Windows.Input;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// fork from https://github.com/jhofinger/Avalonia/tree/listview
    /// </summary>
    public class ListViewItem : ListBoxItem, ICommandSource, IStyling
    {
        private ICommand _command;
        private bool _commandCanExecute = true;
        public Type StyleKey => typeof(ListBoxItem);
        public ListViewItem()
        {
            IsCancelProperty.Changed.Subscribe(IsCancelChanged);
            IsDefaultProperty.Changed.Subscribe(IsDefaultChanged);
            CommandProperty.Changed.Subscribe(CommandChanged);
            this.InitStyle();
        }
        private void IsDefaultChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var content = e.Sender as ListViewItem;
            var isDefault = (bool)e.NewValue;
            if (content?.VisualRoot is IInputElement inputRoot)
            {
                if (isDefault)
                    content.ListenForDefault(inputRoot);
                else
                    content.StopListeningForDefault(inputRoot);
            }
        }
        private void IsCancelChanged(AvaloniaPropertyChangedEventArgs e)
        {
            var content = e.Sender as ListViewItem;
            var isCancel = (bool)e.NewValue;
            if (content?.VisualRoot is IInputElement inputRoot)
            {
                if (isCancel)
                    content.ListenForCancel(inputRoot);
                else
                    content.StopListeningForCancel(inputRoot);
            }
        }
        private void CommandChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is ClickableView content)
            {
                if (((ILogical)content).IsAttachedToLogicalTree)
                {
                    if (e.OldValue is ICommand oldCommand)
                        oldCommand.CanExecuteChanged -= content.CanExecuteChanged;
                    if (e.NewValue is ICommand newCommand)
                        newCommand.CanExecuteChanged += content.CanExecuteChanged;
                }
                content.CanExecuteChanged(content, EventArgs.Empty);
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether the clickableview is the default clickableview for the window.
        /// </summary>
        public bool IsDefault
        {
            get => GetValue(IsDefaultProperty);
            set => SetValue(IsDefaultProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="IsDefaultProperty"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsDefaultProperty =
            AvaloniaProperty.Register<ClickableView, bool>(nameof(IsDefault));
        /// <summary>
        /// Gets or sets a value indicating whether the clickableview is the Cancel clickableview for the window.
        /// </summary>
        public bool IsCancel
        {
            get => GetValue(IsCancelProperty);
            set => SetValue(IsCancelProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="IsCancelProperty"/> property.
        /// </summary>
        public static readonly StyledProperty<bool> IsCancelProperty =
            AvaloniaProperty.Register<ClickableView, bool>(nameof(IsCancel));
        public bool IsPressed
        {
            get => GetValue(IsPressedProperty);
            private set => SetValue(IsPressedProperty, value);
        }
        public static readonly StyledProperty<bool> IsPressedProperty =
           AvaloniaProperty.Register<ClickableView, bool>(nameof(IsPressed));
        /// <summary>
        /// Gets or sets a value indicating how the <see cref="ClickableView"/> should react to clicks.
        /// </summary>
        public ClickMode ClickMode
        {
            get => GetValue(ClickModeProperty);
            set => SetValue(ClickModeProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="ClickMode"/> property.
        /// </summary>
        public static readonly StyledProperty<ClickMode> ClickModeProperty =
            AvaloniaProperty.Register<ClickableView, ClickMode>(nameof(ClickMode), ClickMode.Press);

        /// <summary>
        /// Gets or sets an <see cref="ICommand"/> to be invoked when the ListViewItem is clicked.
        /// </summary>
        public ICommand Command
        {
            get => _command;
            set => SetAndRaise(CommandProperty, ref _command, value);
        }
        /// <summary>
        /// Gets or sets a parameter to be passed to the <see cref="Command"/>.
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="CommandParameter"/> property.
        /// </summary>
        public static readonly StyledProperty<object> CommandParameterProperty =
           AvaloniaProperty.Register<ListViewItem, object>(nameof(CommandParameter));

        /// <summary>
        /// Defines the <see cref="Command"/> property.
        /// </summary>
        public static readonly DirectProperty<ListViewItem, ICommand> CommandProperty =
          AvaloniaProperty.RegisterDirect<ListViewItem, ICommand>(nameof(Command),
              content => content.Command, (content, command) => content.Command = command, enableDataValidation: true);
        /// <summary>
        /// Raised when the user clicks the ListViewItem.
        /// </summary>
        public event EventHandler<ViewRoutedEventArgs> Click
        {
            add { AddHandler(ClickEvent, value); }
            remove { RemoveHandler(ClickEvent, value); }
        }
        /// <summary>
        /// Defines the <see cref="Click"/> event.
        /// </summary>
        public static readonly RoutedEvent<ViewRoutedEventArgs> ClickEvent =
           RoutedEvent.Register<ListViewItem, ViewRoutedEventArgs>(nameof(Click), RoutingStrategies.Bubble);
        /// <summary>
        /// Invokes the <see cref="Click"/> event.
        /// </summary>
        protected virtual void OnClick(MouseButton mouseButton)
        {
            var e = new ViewRoutedEventArgs(ClickEvent, mouseButton);
            RaiseEvent(e);
            if (!e.Handled && Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
                e.Handled = true;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                OnClick(MouseButton.Left);
                e.Handled = true;
            }
            else if (e.Key == Key.Space)
            {
                if (ClickMode == ClickMode.Press)
                    OnClick(MouseButton.Left);
                IsPressed = true;
                e.Handled = true;
            }
            base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (ClickMode == ClickMode.Release)
                    OnClick(MouseButton.Left);
                IsPressed = false;
                e.Handled = true;
            }
        }
        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            base.OnPointerPressed(e);
            var properties = e.GetCurrentPoint(this).Properties;
            if (properties.IsLeftButtonPressed)
            {
                IsPressed = true;
                e.Handled = true;
                if (ClickMode == ClickMode.Press)
                    OnClick(MouseButton.Left);
            }
            else if (properties.IsRightButtonPressed)
            {
                IsPressed = true;
                e.Handled = true;
                if (ClickMode == ClickMode.Press)
                    OnClick(MouseButton.Right);
            }
        }
        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            base.OnPointerReleased(e);
            if (IsPressed)
            {
                IsPressed = false;
                e.Handled = true;
                if (ClickMode == ClickMode.Release && this.GetVisualsAt(e.GetPosition(this)).Any(c => this == c || this.IsVisualAncestorOf(c)))
                {
                    if (e.InitialPressMouseButton == MouseButton.Left)
                        OnClick(MouseButton.Left);
                    else if (e.InitialPressMouseButton == MouseButton.Right)
                        OnClick(MouseButton.Right);
                }
            }
        }
        protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
        {
            IsPressed = false;
        }
        protected override void UpdateDataValidation<T>(AvaloniaProperty<T> property, BindingValue<T> value)
        {
            base.UpdateDataValidation(property, value);
            if (property == CommandProperty && value.Type == BindingValueType.BindingError && _commandCanExecute)
            {
                _commandCanExecute = false;
                UpdateIsEffectivelyEnabled();
            }
        }
        /// <summary>
        /// Starts listening for the Enter key when the clickableview <see cref="IsDefault"/>.
        /// </summary>
        /// <param name="root">The input root.</param>
        private void ListenForDefault(IInputElement root)
        {
            root.AddHandler(KeyDownEvent, RootDefaultKeyDown);
        }
        /// <summary>
        /// Starts listening for the Escape key when the clickableview <see cref="IsCancel"/>.
        /// </summary>
        /// <param name="root">The input root.</param>
        private void ListenForCancel(IInputElement root)
        {
            root.AddHandler(KeyDownEvent, RootCancelKeyDown);
        }
        /// <summary>
        /// Stops listening for the Enter key when the clickableview is no longer <see cref="IsDefault"/>.
        /// </summary>
        /// <param name="root">The input root.</param>
        private void StopListeningForDefault(IInputElement root)
        {
            root.RemoveHandler(KeyDownEvent, RootDefaultKeyDown);
        }
        /// <summary>
        /// Stops listening for the Escape key when the clickableview is no longer <see cref="IsCancel"/>.
        /// </summary>
        /// <param name="root">The input root.</param>
        private void StopListeningForCancel(IInputElement root)
        {
            root.RemoveHandler(KeyDownEvent, RootCancelKeyDown);
        }
        /// <summary>
        /// Called when a key is pressed on the input root and the clickableview <see cref="IsCancel"/>.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void RootCancelKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && IsVisible && IsEnabled)
                OnClick(MouseButton.Left);
        }
        /// <summary>
        /// Called when a key is pressed on the input root and the clickableview <see cref="IsDefault"/>.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void RootDefaultKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && IsVisible && IsEnabled)
                OnClick(MouseButton.Left);
        }
        public void CanExecuteChanged(object sender, EventArgs e)
        {
            var canExecute = Command == null || Command.CanExecute(CommandParameter);
            if (canExecute != _commandCanExecute)
            {
                _commandCanExecute = canExecute;
                UpdateIsEffectivelyEnabled();
            }
        }
    }
}