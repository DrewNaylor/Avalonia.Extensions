using Avalonia.Controls;
using Avalonia.Styling;
using System;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// fork from https://github.com/jhofinger/Avalonia/tree/listview
    /// </summary>
    public class ListView : ListBox, IStyleable
    {
        static ListView()
        {
            SelectionModeProperty.OverrideMetadata<ListView>(new StyledPropertyMetadata<SelectionMode>(SelectionMode.Multiple));
            ViewProperty.Changed.AddClassHandler<Grid>(OnViewChanged);
        }
        public static readonly AvaloniaProperty ViewProperty = AvaloniaProperty.Register<ListView, ViewBase>(nameof(View));
        public ViewBase View
        {
            get => (ViewBase)GetValue(ViewProperty);
            set => SetValue(ViewProperty, value);
        }
        private static void OnViewChanged(AvaloniaObject d, AvaloniaPropertyChangedEventArgs e)
        {
            ListView listView = (ListView)d;
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
    }
}