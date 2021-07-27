using System;

namespace Avalonia.Extensions.Controls
{
    public sealed class ExceptionEventArgs : EventArgs
    {
        public Exception ErrorException { get; }
        internal ExceptionEventArgs(Exception errorException) : base()
        {
            ErrorException = errorException ?? throw new ArgumentNullException("errorException");
        }
    }
}