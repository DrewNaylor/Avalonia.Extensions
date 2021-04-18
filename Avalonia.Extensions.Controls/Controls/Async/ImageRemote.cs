using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace Avalonia.Extensions.Controls
{
    public sealed class ImageRemote : Image
    {
        private HttpClient HttpClient { get; }
        public ImageRemote() : base()
        {
            HttpClient = Core.Instance.GetClient();
        }
        /// <summary>
        /// Defines the <see cref="Address"/> property.
        /// </summary>
        public static readonly StyledProperty<string> AddressProperty =
          AvaloniaProperty.Register<ImageRemote, string>(nameof(Address), string.Empty);
        /// <summary>
        /// get or set image url address
        /// </summary>
        public string Address
        {
            get => GetValue(AddressProperty);
            set
            {
                SetValue(AddressProperty, value);
                LoadBitmap();
            }
        }
        /// <summary>
        /// Defines the <see cref="Failed"/> property.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> FailedEvent =
            RoutedEvent.Register<ImageRemote, RoutedEventArgs>(nameof(Failed), RoutingStrategies.Bubble);
        /// <summary>
        /// Raised when the image load failed.
        /// </summary>
        public event EventHandler<RoutedEventArgs> Failed
        {
            add { AddHandler(FailedEvent, value); }
            remove { RemoveHandler(FailedEvent, value); }
        }
        /// <summary>
        /// Defines the <see cref="Opened"/> property.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> OpenedEvent =
            RoutedEvent.Register<ImageRemote, RoutedEventArgs>(nameof(Opened), RoutingStrategies.Bubble);
        /// <summary>
        /// Raised when the image load failed.
        /// </summary>
        public event EventHandler<RoutedEventArgs> Opened
        {
            add { AddHandler(OpenedEvent, value); }
            remove { RemoveHandler(OpenedEvent, value); }
        }
        private void LoadBitmap()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                try
                {
                    HttpResponseMessage hr = HttpClient.GetAsync(Address).ConfigureAwait(false).GetAwaiter().GetResult();
                    hr.EnsureSuccessStatusCode();
                    using var stream = hr.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                    this.Source = new Bitmap(stream);
                    MediaChange(true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    MediaChange(false);
                }
            });
        }
        private void MediaChange(bool isSuccess)
        {
            var @event = isSuccess ? new RoutedEventArgs(OpenedEvent) : new RoutedEventArgs(FailedEvent);
            RaiseEvent(@event);
            if (!@event.Handled)
                @event.Handled = true;
        }
    }
}