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
        private string oldVaule = string.Empty;
        private HttpClient HttpClient { get; }
        /// <summary>
        /// original image width
        /// </summary>
        public double ImageWidth { get; private set; }
        /// <summary>
        /// original image height
        /// </summary>
        public double ImageHeight { get; private set; }
        private string _address;
        private ImageStretch _stretch = ImageStretch.None;
        private bool _mandatory = false;
        public ImageRemote() : base()
        {
            HttpClient = Core.Instance.GetClient();
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
        /// Defines the <see cref="Stretch"/> property.
        /// </summary>
        public static new readonly DirectProperty<ImageRemote, ImageStretch> StretchProperty =
          AvaloniaProperty.RegisterDirect<ImageRemote, ImageStretch>(nameof(Stretch), o => o.Stretch, (o, v) => o.Stretch = v);
        /// <summary>
        /// Defines the <see cref="Mandatory"/> property.
        /// </summary>
        public static readonly DirectProperty<ImageRemote, bool> MandatoryProperty =
          AvaloniaProperty.RegisterDirect<ImageRemote, bool>(nameof(Mandatory), o => o.Mandatory, (o, v) => o.Mandatory = v);
        /// <summary>
        /// get or set image url address
        /// </summary>
        [Content]
        public string Address
        {
            get => GetValue(AddressProperty);
            set
            {
                oldVaule = _address;
                SetAndRaise(AddressProperty, ref _address, value);
                LoadBitmap(value);
            }
        }
        [Content]
        public new ImageStretch Stretch
        {
            get => GetValue(StretchProperty);
            set
            {
                SetAndRaise(StretchProperty, ref _stretch, value);
                base.Stretch = value == ImageStretch.None ? Media.Stretch.None : Media.Stretch.UniformToFill;
                LoadBitmap(oldVaule, true);
            }
        }
        /// <summary>
        /// force refresh of the picture resource or not,
        /// regardless of whether the <see cref="Address"/> is the same
        /// </summary>
        [Content]
        public bool Mandatory
        {
            get => GetValue(MandatoryProperty);
            set => SetAndRaise(MandatoryProperty, ref _mandatory, value);
        }
        private async void LoadBitmap(string url, bool refresh = false)
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                if (refresh || oldVaule != url || _mandatory)
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
                                var bitmap = new Bitmap(stream);
                                ImageWidth = bitmap.PixelSize.Width;
                                ImageHeight = bitmap.PixelSize.Height;
                                switch (_stretch)
                                {
                                    case ImageStretch.None:
                                        {
                                            Width = ImageWidth;
                                            Height = ImageHeight;
                                            break;
                                        }
                                    case ImageStretch.UniformToFill:
                                        {
                                            bitmap = Bitmap.DecodeToWidth(stream, Width.ToInt32());
                                            break;
                                        }
                                }
                                this.Source = bitmap;
                                MediaChange(true);
                            }
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
                    finally
                    {
                        Loading = false;
                    }
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