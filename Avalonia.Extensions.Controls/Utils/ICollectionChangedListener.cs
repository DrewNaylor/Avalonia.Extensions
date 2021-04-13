using System.Collections.Specialized;

namespace Avalonia.Extensions.Controls.Utils
{
    internal interface ICollectionChangedListener
    {
        void PreChanged(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e);
        void Changed(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e);
        void PostChanged(INotifyCollectionChanged sender, NotifyCollectionChangedEventArgs e);
    }
}