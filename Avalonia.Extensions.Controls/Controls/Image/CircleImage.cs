using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using Avalonia.Threading;
using System;

namespace Avalonia.Extensions.Controls
{
    public sealed class CircleImage : Ellipse
    {
        private DownloadTask Task { get; }
        /// <summary>
        /// Defines the <see cref="Source"/> property.
        /// </summary>
        public static readonly StyledProperty<Uri> SourceProperty =
            AvaloniaProperty.Register<CircleImage, Uri>(nameof(Source));
        /// <summary>
        /// Defines the <see cref="Failed"/> property.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> FailedEvent =
            RoutedEvent.Register<CircleImage, RoutedEventArgs>(nameof(Failed), RoutingStrategies.Bubble);
        static CircleImage()
        {
            AffectsRender<CircleImage>(SourceProperty);
            AffectsMeasure<CircleImage>(SourceProperty);
        }
        public CircleImage() : base()
        {
            Task = new DownloadTask();
            SourceProperty.Changed.AddClassHandler<CircleImage>(OnSourceChange);
        }
        private void OnSourceChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is Uri uri)
            {
                switch (uri.Scheme)
                {
                    case "http":
                    case "https":
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            Task.Create(uri, (result) =>
                            {
                                if (result.Stream != null)
                                {
                                    var bitmap = new Bitmap(result.Stream);
                                    this.Fill = new ImageBrush { Source = bitmap };
                                    DrawAgain();
                                    SetSize(bitmap.Size);
                                }
                            });
                        });
                        break;
                    case "avares":
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            var assets = Core.Instance.AssetLoader;
                            using var bitmap = new Bitmap(assets.Open(uri));
                            this.Fill = new ImageBrush { Source = bitmap };
                            DrawAgain();
                            SetSize(bitmap.Size);
                        });
                        break;
                    default:
                        failedMessage = "unsupport URI scheme.only support HTTP/HTTPS or avares://";
                        var @event = new RoutedEventArgs(FailedEvent);
                        RaiseEvent(@event);
                        if (!@event.Handled)
                            @event.Handled = true;
                        break;
                }
            }
        }
        private void SetSize(Size size)
        {
            if (double.IsNaN(Width) || double.IsNaN(Height))
            {
                var round = Math.Min(size.Width, size.Height);
                Height = Width = round;
            }
            else
            {
                var maxValue = Math.Max(Width, Height);
                Height = Width = maxValue;
            }
        }
        public void DrawAgain()
        {
            InvalidateVisual();
            InvalidateMeasure();
        }
        /// <summary>
        /// Gets or sets the image that will be displayed.
        /// </summary>
        [Content]
        public Uri Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        /// <summary>
        /// Raised when the image load failed.
        /// </summary>
        public event EventHandler<RoutedEventArgs> Failed
        {
            add { AddHandler(FailedEvent, value); }
            remove { RemoveHandler(FailedEvent, value); }
        }
        private string failedMessage;
        /// <summary>
        /// error message if loading failed
        /// </summary>
        public string FailedMessage => failedMessage;
    }
}