using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace Avalonia.Extensions.Controls
{
    //https://github.com/AndreiMisiukevich/ExpandableView/blob/master/ExpandableView/ExpandableView.cs
    public class ExpandableView : Panel
    {
        public ExpandableView()
        {
            ExpandModeProperty.Changed.AddClassHandler<ExpandableView>(OnExpandModeChange);
            PrimaryViewProperty.Changed.AddClassHandler<ExpandableView>(OnPrimaryViewChange);
            SecondViewProperty.Changed.AddClassHandler<ExpandableView>(OnSecondViewChange);
        }
        private void OnExpandModeChange(ExpandableView sender, AvaloniaPropertyChangedEventArgs e)
        {

        }
        private void OnSecondViewChange(ExpandableView sender, AvaloniaPropertyChangedEventArgs e)
        {

        }
        private void OnPrimaryViewChange(ExpandableView sender, AvaloniaPropertyChangedEventArgs e)
        {

        }
        /// <summary>
        /// Defines the <see cref="ExpandMode"/> property.
        /// </summary>
        public static readonly StyledProperty<ExpandMode> ExpandModeProperty =
          AvaloniaProperty.Register<ExpandableView, ExpandMode>(nameof(ExpandMode));
        public ExpandMode ExpandMode
        {
            get => GetValue(ExpandModeProperty);
            set => SetValue(ExpandModeProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="PrimaryView"/> property.
        /// </summary>
        public static readonly StyledProperty<Panel> PrimaryViewProperty =
          AvaloniaProperty.Register<ExpandableView, Panel>(nameof(PrimaryView));
        public Panel PrimaryView
        {
            get => GetValue(PrimaryViewProperty);
            set => SetValue(PrimaryViewProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="SecondView"/> property.
        /// </summary>
        public static readonly StyledProperty<Panel> SecondViewProperty =
          AvaloniaProperty.Register<ExpandableView, Panel>(nameof(SecondView));
        public Panel SecondView
        {
            get => GetValue(SecondViewProperty);
            set => SetValue(SecondViewProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="StatusChange"/> property.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> StatusChangeEvent =
            RoutedEvent.Register<ExpandableView, RoutedEventArgs>(nameof(StatusChange), RoutingStrategies.Bubble);
        public event EventHandler<RoutedEventArgs> StatusChange
        {
            add { AddHandler(StatusChangeEvent, value); }
            remove { RemoveHandler(StatusChangeEvent, value); }
        }
        public void Collapse()
        {

        }
        public void Expand()
        {

        }
    }
}