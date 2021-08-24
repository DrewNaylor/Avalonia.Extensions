using Avalonia.Controls;
using Avalonia.Extensions.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Avalonia.Extensions.Threading
{
    internal class BitmapThread : IDisposable
    {
        private int HashCode;
        private IBitmapSource Owner { get; }
        public BitmapThread(IBitmapSource source)
        {
            Owner = source;
        }
        public void Update()
        {
            if (Owner.Source != null)
            {
                var hashCode = Owner.Source.GetHashCode();
                if (HashCode != hashCode)
                {
                    HashCode = hashCode;
                    switch (Owner.Source.Scheme)
                    {
                        case "avares":
                            {

                                break;
                            }
                        case "http":
                        case "https":
                            {
                                CancellationTokenSource cancellationToken = new CancellationTokenSource();
                                ThreadPool.QueueUserWorkItem(new WaitCallback(OnHttpHandle), new object[] { cancellationToken.Token, Owner.Source });
                                break;
                            }
                    }
                }
            }
        }
        private void OnHttpHandle(object state)
        {
            if (state is object[] objs)
            {
                var uri = objs.ElementAt(1) as Uri;
                var cancelToken = (CancellationToken)objs.ElementAt(0);
                cancelToken.Register(() =>
                {

                });

            }
        }
        public void Dispose()
        {

        }
    }
}