using Avalonia.Controls;
using Avalonia.Controls.Templates;
using System.Collections.Generic;

namespace Avalonia.Extensions.Controls
{
    public partial class ExpandableListView : ScrollViewer
    {
        private VerticalItemsRepeater Repeater { get; }
        private IEnumerable<ExpandableArray> _items;
        public ExpandableListView()
        {
            Repeater = new VerticalItemsRepeater();
            Init();
            ItemsProperty.Changed.AddClassHandler<ExpandableListView>(OnItemsChange);
        }
        private void Init()
        {
            Repeater.Items = _items;
            Repeater.ItemTemplate = new FuncDataTemplate<ExpandableArray>((item, _) =>
            {
                var view = new ExpandableView();
                view.PrimaryView = new ExpandableListHeader(item.Header);
                view.SecondView = new ClickableView { };
                return view;
            });
            this.Content = Repeater;
        }
        /// <summary>
        /// Identifies the ItemsSource dependency property.
        /// </summary>
        public static readonly DirectProperty<ExpandableListView, IEnumerable<ExpandableArray>> ItemsProperty =
            AvaloniaProperty.RegisterDirect<ExpandableListView, IEnumerable<ExpandableArray>>(nameof(Items), o => o.Items, (o, v) => o.Items = v);
        /// <summary>
        /// Gets or sets a collection that is used to generate the content of the control.
        /// </summary>
        public IEnumerable<ExpandableArray> Items
        {
            get => _items;
            set => SetAndRaise(ItemsProperty, ref _items, value);
        }
        private void OnItemsChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {

        }
    }
}