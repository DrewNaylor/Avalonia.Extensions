using Avalonia.Controls.Presenters;
using System.Collections;
using System.Collections.Specialized;

namespace Avalonia.Controls.Extensions.Presenters
{
    public interface IColumnsPresenter : IPresenter
    {
        IPanel Panel { get; }
        int ColumnNum { get; set; }
        IEnumerable Items { get; set; }
        void ScrollIntoRow(int index);
        void ItemsChanged(NotifyCollectionChangedEventArgs e);
    }
}