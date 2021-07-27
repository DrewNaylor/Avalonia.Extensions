using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Controls
{
    public abstract class WeakEventManager
    {
        static WeakEventManager()
        {
            ThreadPool.SetMaxThreads(10, 10);
        }
        public static void AddHandler(Uri source, EventHandler<ExceptionEventArgs> handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

        }
        public static void RemoveHandler(Uri source, EventHandler<ExceptionEventArgs> handler)
        {
            if (handler == null)
                throw new ArgumentNullException("handler");

        }
    }
}