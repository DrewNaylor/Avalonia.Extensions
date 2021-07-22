using ReactiveUI;
using System;

namespace Avalonia.Extensions.Controls
{
    public class ViewModelBase : ReactiveObject
    {
        public Guid ViewID { get; }
        public ViewModelBase()
        {
            ViewID = Guid.NewGuid();
        }
    }
}