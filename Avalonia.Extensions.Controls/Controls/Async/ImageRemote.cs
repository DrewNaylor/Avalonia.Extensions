using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using Avalonia.Threading;
using System;
using System.Diagnostics;
using System.Net.Http;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// Inherited from <see cref="Image"/>.
    /// Used to display HTTP/HTTPS pictures
    /// </summary>
    public sealed class ImageRemote : Image
    {
        private HttpClient HttpClient { get; }
        /// <summary>
        /// original  image width
        /// </summary>
        public double ImageWidth { get; private set; }
        /// <summary>
        /// original  image height
        /// </summary>
        public double ImageHeight { get; private set; }
        private string _address;
        public ImageRemote() : base()
        {
            HttpClient = Core.Instance.GetClient();
            AddressProperty.Changed.AddClassHandler<ImageRemote>(OnAddressChange);
        }
        /// <summary>
        /// error message if loading failed
        /// </summary>
        public string FailedMessage { get; private set; }
        /// <summary>
        /// Defines the <see cref="Address"/> property.
        /// </summary>
        public static readonly DirectProperty<ImageRemote, string> AddressProperty =
          AvaloniaProperty.RegisterDirect<ImageRemote, string>(nameof(Address), o => o.Address, (o, v) => o.Address = v);
        /// <summary>
        /// get or set image url address
        /// </summary>
        [Content]
        public string Address
        {
            get => GetValue(AddressProperty);
            set => SetAndRaise(AddressProperty, ref _address, value);
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
        private void OnAddressChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                var address = e.NewValue.ToString();
                try
                {
                    if (!string.IsNullOrEmpty(address))
                    {
                        HttpResponseMessage hr = HttpClient.GetAsync(address).ConfigureAwait(false).GetAwaiter().GetResult();
                        hr.EnsureSuccessStatusCode();
                        using var stream = hr.Content.ReadAsStreamAsync().ConfigureAwait(false).GetAwaiter().GetResult();
                        var bitmap = new Bitmap(stream);
                        ImageWidth = Width = bitmap.PixelSize.Width;
                        ImageHeight = Height = bitmap.PixelSize.Height;
                        this.Source = bitmap;
                        MediaChange(true);
                    }
                }
                catch (Exception ex)
                {
                    FailedMessage = ex.Message;
#if DEBUG
                    Debug.WriteLine(FailedMessage);
#endif
                    MediaChange(false);
                }
            });
        }
        public void ZoomIn(double percentage)
        {
            Width = ImageWidth * percentage;
            Height = ImageHeight * percentage;
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