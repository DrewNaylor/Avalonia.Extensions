using System;

namespace Avalonia.Extensions.Controls
{
    public sealed class StatusChangedEventArgs : EventArgs
    {
        public StatusChangedEventArgs(ExpandStatus status)
        {
            Status = status;
        }
        public ExpandStatus Status { get; }
    }
}