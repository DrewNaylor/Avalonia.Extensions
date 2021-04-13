using Avalonia.Controls;
using Avalonia.Styling;
using System;

namespace Avalonia.Extensions.Controls
{
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
            get { return (ViewBase)GetValue(ViewProperty); }
            set { SetValue(ViewProperty, value); }
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
            listView._previousView = oldView;
            listView.ApplyNewView();
            listView._previousView = newView;
            if (oldView != null)
                oldView.IsUsed = false;
        }
        Type _defaultstyle;
        Type IStyleable.StyleKey => typeof(ListBox);
        private void ApplyNewView()
        {
            ViewBase newView = View;
            if (newView != null)
                _defaultstyle = newView.DefaultStyleKey;
            else
                _defaultstyle = null;
        }
        private ViewBase _previousView;
    }
}