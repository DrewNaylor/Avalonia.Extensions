using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using Avalonia.Threading;
using System;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// Inherited from <see cref="Image"/>.
    /// Used to display HTTP/HTTPS pictures
    /// </summary>
    public sealed class ImageRemote : Image
    {
        /// <summary>
        /// original image width
        /// </summary>
        public double ImageWidth { get; private set; }
        /// <summary>
        /// original image height
        /// </summary>
        public double ImageHeight { get; private set; }
        private DownloadTask Task { get; }
        private string _address;
        public ImageRemote() : base()
        {
            Task = new DownloadTask();
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
            get => _address;
            set
            {
                SetAndRaise(AddressProperty, ref _address, value);
                LoadBitmap(value);
            }
        }
        private void LoadBitmap(string url)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Task.Create(url, OnDrawBitmap);
            });
        }
        private void OnDrawBitmap(DownloadTask.Result result)
        {
            if (result.Success)
            {
                Bitmap bitmap;
                var width = Width.ToInt32();
                if (double.IsNaN(Width) || width == 0)
                {
                    bitmap = new Bitmap(result.Stream);
                    Width = ImageWidth = bitmap.PixelSize.Width;
                    Height = ImageHeight = bitmap.PixelSize.Height;
                }
                else
                {
                    bitmap = Bitmap.DecodeToWidth(result.Stream, width);
                    ImageWidth = bitmap.PixelSize.Width;
                    ImageHeight = bitmap.PixelSize.Height;
                }
                Source = bitmap;
            }
            else
            {
                FailedMessage = result.Message;
                var @event = new RoutedEventArgs(FailedEvent);
                RaiseEvent(@event);
                if (!@event.Handled)
                    @event.Handled = true;
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
        public void ZoomIn(double percentage)
        {
            Width = ImageWidth * percentage;
            Height = ImageHeight * percentage;
        }
    }
}