using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Metadata;
using Avalonia.Styling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Extensions.Controls
{
    public class ExpandableListView : ListBox, IStyling
    {
        Type IStyleable.StyleKey => typeof(ListBox);
        private IEnumerable<ExpandableObject> _items = new AvaloniaList<ExpandableObject>();
        /// <summary>
        /// Defines the <see cref="Items"/> property.
        /// </summary>
        public static new readonly DirectProperty<ExpandableListView, IEnumerable<ExpandableObject>> ItemsProperty =
            AvaloniaProperty.RegisterDirect<ExpandableListView, IEnumerable<ExpandableObject>>(nameof(Items), o => o.Items, (o, v) => o.Items = v);
        /// <summary>
        /// Gets or sets the items to display.
        /// </summary>
        [Content]
        public new IEnumerable<ExpandableObject> Items
        {
            get => _items;
            set => SetAndRaise(ItemsProperty, ref _items, value);
        }
        public ExpandableListView()
        {

            this.InitStyle();
        }
    }
}