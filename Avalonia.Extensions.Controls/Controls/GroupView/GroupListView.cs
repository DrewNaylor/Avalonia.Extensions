using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Extensions.Model;
using Avalonia.Extensions.Styles;
using Avalonia.Metadata;
using Avalonia.Styling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Avalonia.Extensions.Controls
{
    public class GroupListView : ListView, IStyling
    {
        Type IStyleable.StyleKey => typeof(ListBox);
        private IEnumerable _items = new AvaloniaList<object>();
        public GroupListView()
        {
            SelectorProperty.Changed.AddClassHandler<GroupListView>(OnSelectorChange);
            ItemSourcesProperty.Changed.AddClassHandler<GroupListView>(OnItemSourcesChange);
            this.InitStyle();
        }
        private void OnSelectorChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is Func<dynamic, string> func)
                GroupView(Items, func);
        }
        private void OnItemSourcesChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            GroupView(e.NewValue, Selector);
        }
        private void GroupView(object enums, Func<dynamic, string> selector)
        {
            if (enums is IList<dynamic> list)
            {
                var arr = list.GroupBy(selector).Select(x => new GroupItem(x.Key, x));
                SetValue(ItemsProperty, arr);
            }
            else if (enums is IEnumerable<dynamic> array)
            {
                var arr = array.GroupBy(selector).Select(x => new GroupItem(x.Key, x));
                SetValue(ItemsProperty, arr);
            }
        }
        public Func<dynamic, string> Selector
        {
            get => GetValue(SelectorProperty);
            set => SetValue(SelectorProperty, value);
        }
        public static readonly StyledProperty<Func<dynamic, string>> SelectorProperty =
            AvaloniaProperty.Register<RunLabel, Func<dynamic, string>>(nameof(Selector));
        protected override IItemContainerGenerator CreateItemContainerGenerator() => new ItemsGenerator(this, ContentControl.ContentProperty, ContentControl.ContentTemplateProperty);
        /// <summary>
        /// Defines the <see cref="Items"/> property.
        /// </summary>
        public static readonly DirectProperty<GroupListView, IEnumerable> ItemSourcesProperty =
            AvaloniaProperty.RegisterDirect<GroupListView, IEnumerable>(nameof(ItemSources), o => o.ItemSources, (o, v) => o.ItemSources = v);
        /// <summary>
        /// Gets or sets the items to display.
        /// </summary>
        [Content]
        public IEnumerable ItemSources
        {
            get => _items;
            set => SetAndRaise(ItemSourcesProperty, ref _items, value);
        }
    }
}