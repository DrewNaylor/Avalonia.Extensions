using System;

namespace Avalonia.Extensions.Controls
{
    public static class AvaloniaUtils
    {
        public static IDisposable AddClassHandler<TTarget, TValue>(
        this IObservable<AvaloniaPropertyChangedEventArgs<TValue>> observable,
        Action<TTarget, AvaloniaPropertyChangedEventArgs<TValue>> action) where TTarget : AvaloniaObject
        {
            return observable.Subscribe(e =>
            {
                if (e.Sender is TTarget target)
                    action?.Invoke(target, e);
            });
        }
    }
}
