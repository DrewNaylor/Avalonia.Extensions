using System.Collections;
using System.Collections.Generic;

namespace Avalonia.Controls.Extensions
{
    internal sealed class ViewManager
    {
        private GridViewItem GridView { get; }
        public ViewManager(GridViewItem gridView)
        {
            GridView = gridView;

        }
        public void AddControlItemsToLogicalChildren(IEnumerable items)
        {
            if (items != null)
            {
                foreach (var i in items)
                {
                    if (i is IControl control && !GridView.Children.Contains(control))
                    {
                        GridView.Children.Add(control);
                        Grid.SetColumn(SetColumnIndex(control), GridView.LastAncor);
                    }
                    else
                    {
                        control = GridView.ItemTemplate.Build(i);
                        if (control != null)
                        {
                            GridView.Children.Add(control);
                            Grid.SetColumn(SetColumnIndex(control), GridView.LastAncor);
                        }
                    }
                }
            }
        }
        public Control SetColumnIndex(IControl control)
        {
            if (GridView.LastAncor < GridView.ColumnNum)
                GridView.LastAncor++;
            else
                GridView.LastAncor = 0;
            control.SetValue(Grid.ColumnProperty, GridView.LastAncor);
            return control as Control;
        }
        public void RemoveControlItemsFromLogicalChildren(IEnumerable items)
        {
            if (items != null)
            {
                var toRemove = new List<IControl>();
                foreach (var i in items)
                {
                    if (i is IControl control)
                        toRemove.Add(control);
                }
                GridView.Children.RemoveAll(toRemove);
            }
        }
    }
}