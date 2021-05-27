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
        private bool Loading = false;
        private HttpClient HttpClient { get; }
        /// <summary>
        /// original image width
        /// </summary>
        public double ImageWidth
        {
            get => imageWidth;
        }
        /// <summary>
        /// original image height
        /// </summary>
        public double ImageHeight
        {
            get => imageHeight;
        }
        private string _address, failedMessage;
        private double imageWidth, imageHeight;
        public ImageRemote() : base()
        {
            HttpClient = Core.Instance.GetClient();
        }
        /// <summary>
        /// error message if loading failed
        /// </summary>
        public string FailedMessage
        {
            get => failedMessage;
        }
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
            set
            {
                SetAndRaise(AddressProperty, ref _address, value);
                LoadBitmap(value);
            }
        }
        private async void LoadBitmap(string url, bool refresh = false)
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                try
                {
                    if (!Loading)
                    {
                        Loading = true;
                        if (!string.IsNullOrEmpty(url))
                        {
                            HttpResponseMessage hr = await HttpClient.GetAsync(url);
                            hr.EnsureSuccessStatusCode();
                            using var stream = await hr.Content.ReadAsStreamAsync();
                            var width = Width.ToInt32();
                            if (double.IsNaN(Width) || width == 0)
                            {
                                var bitmap = new Bitmap(stream);
                                Width = imageWidth = bitmap.PixelSize.Width;
                                Height = imageHeight = bitmap.PixelSize.Height;
                                this.Source = bitmap;
                                MediaChange(true);
                            }
                            else
                            {
                                var bitmap = Bitmap.DecodeToWidth(stream, width);
                                imageWidth = bitmap.PixelSize.Width;
                                imageHeight = bitmap.PixelSize.Height;
                                this.Source = bitmap;
                                MediaChange(true);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    failedMessage = ex.Message;
#if DEBUG
                    Debug.WriteLine(FailedMessage);
#endif
                    MediaChange(false);
                }
                finally
                {
                    Loading = false;
                }
            });
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