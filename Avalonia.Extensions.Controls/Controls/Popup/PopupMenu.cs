using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Metadata;
using Avalonia.Threading;
using System;
using System.Collections;

namespace Avalonia.Extensions.Controls
{
    public class PopupMenu : Window
    {
        private ListBox ListBox { get; }
        /// <summary>
        /// Defines the <see cref="Items"/> property.
        /// </summary>
        public static readonly DirectProperty<PopupMenu, IList> ItemsProperty =
          AvaloniaProperty.RegisterDirect<PopupMenu, IList>(nameof(Items), o => o.Items, (o, v) => o.Items = v);
        /// <summary>
        /// Defines the <see cref="ItemTemplate"/> property.
        /// </summary>
        public static readonly StyledProperty<IDataTemplate> ItemTemplateProperty =
            AvaloniaProperty.Register<PopupMenu, IDataTemplate>(nameof(ItemTemplate));
        /// <summary>
        /// Gets or sets the items to display.
        /// </summary>
        [Content]
        public IList Items
        {
            get => _items;
            set => SetAndRaise(ItemsProperty, ref _items, value);
        }
        /// <summary>
        /// Gets or sets the data template used to display the items in the control.
        /// </summary>
        public IDataTemplate ItemTemplate
        {
            get { return GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }
        private IList _items;
        private bool _isFocus = true;
        public event EventHandler<ItemClickEventArgs> ItemClick;
        public PopupMenu()
        {
            this.Width = 80;
            this.Height = 60;
            ListBox = new ListBox();
            this.ShowInTaskbar = false;
            this.SystemDecorations = SystemDecorations.None;
            ListBox.VirtualizationMode = ItemVirtualizationMode.None;
            ItemsProperty.Changed.AddClassHandler<PopupMenu>(OnItemsChange);
            ItemTemplateProperty.Changed.AddClassHandler<PopupMenu>(OnItemTemplateChanged);
        }
        private void OnItemTemplateChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is IDataTemplate template)
                ListBox.ItemTemplate = template;
        }
        private void OnItemsChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is IList array)
                ListBox.Items = array;
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            ListBox.SelectionChanged += ListBox_SelectionChanged;
            this.Content = ListBox;
        }
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is ListBox listBox)
            {
                try
                {
                    var item = listBox.SelectedItem;
                    var args = new ItemClickEventArgs(item);
                    ItemClick?.Invoke(sender, args);
                }
                finally
                {
                    Close();
                }
            }
        }
        protected override void OnPointerEnter(PointerEventArgs e)
        {
            _isFocus = true;
            base.OnPointerEnter(e);
        }
        protected override void OnPointerLeave(PointerEventArgs e)
        {
            _isFocus = false;
            base.OnPointerLeave(e);
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            if (!_isFocus)
                Close();
        }
        public void Show(IControl control)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var window = control.GetWindow();
                if ((control.TransformedBounds as dynamic).Clip is Rect rect)
                {
                    int x = (rect.X + window.Position.X).ToInt32(),
                        y = (rect.Y + window.Position.Y).ToInt32();
                    Position = new PixelPoint(x, y);
                }
            });
            base.Show();
        }
        public new void Close()
        {
            ListBox.SelectionChanged -= ListBox_SelectionChanged;
            base.Close();
        }
    }
}