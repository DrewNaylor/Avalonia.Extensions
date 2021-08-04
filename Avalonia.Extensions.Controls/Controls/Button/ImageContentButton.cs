using Avalonia.Controls;
using Avalonia.Metadata;
using System;

namespace Avalonia.Extensions.Controls
{
    public class ImageContentButton : Button
    {
        public ImageContentButton() : base()
        {
            ContentProperty.Changed.AddClassHandler<ImageContentButton>(OnContentChange);
            ImageSourceProperty.Changed.AddClassHandler<ImageContentButton>(OnImageSourceChange);
        }
        private void OnContentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is string chars)
                base.Content = chars;
        }
        private void OnImageSourceChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {

        }
        /// <summary>
        /// Defines the <see cref="Source"/> property.
        /// </summary>
        public static readonly DirectProperty<ImageContentButton, Uri> ImageSourceProperty =
          AvaloniaProperty.RegisterDirect<ImageContentButton, Uri>(nameof(ImageSource), o => o.ImageSource, (o, v) => o.ImageSource = v);
        private Uri _source;
        /// <summary>
        /// get or set image url address
        /// </summary>
        [Content]
        public Uri ImageSource
        {
            get => _source;
            set => SetAndRaise(ImageSourceProperty, ref _source, value);
        }
        public new string Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="IsDefaultProperty"/> property.
        /// </summary>
        public static new readonly StyledProperty<string> ContentProperty =
            AvaloniaProperty.Register<ImageContentButton, string>(nameof(Content));
    }
}