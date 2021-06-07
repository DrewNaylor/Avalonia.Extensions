using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Metadata;
using System;

namespace Avalonia.Extensions.Controls
{
    public sealed class PersonPicture : Ellipse
    {
        private ImageBrush Pencil { get; }
        /// <summary>
        /// Defines the <see cref="Source"/> property.
        /// </summary>
        public static readonly StyledProperty<Uri> SourceProperty =
            AvaloniaProperty.Register<PersonPicture, Uri>(nameof(Source));
        /// <summary>
        /// Defines the <see cref="StretchDirection"/> property.
        /// </summary>
        public static readonly StyledProperty<StretchDirection> StretchDirectionProperty =
            AvaloniaProperty.Register<PersonPicture, StretchDirection>(
                nameof(StretchDirection),
                StretchDirection.Both);
        static PersonPicture()
        {
            AffectsRender<PersonPicture>(SourceProperty, StretchProperty, StretchDirectionProperty);
            AffectsMeasure<PersonPicture>(SourceProperty, StretchProperty, StretchDirectionProperty);
        }
        public PersonPicture()
        {
            Pencil = new ImageBrush();
            this.Fill = Pencil;
            SourceProperty.Changed.AddClassHandler<PersonPicture>(OnSourceChange);
        }
        private void OnSourceChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is Uri uri)
            {
                Pencil.Source = new Bitmap("");
                InvalidateMeasure();
            }
        }
        /// <summary>
        /// Gets or sets the image that will be displayed.
        /// </summary>
        [Content]
        public Uri Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        /// <summary>
        /// Gets or sets a value controlling in what direction the image will be stretched.
        /// </summary>
        public StretchDirection StretchDirection
        {
            get { return GetValue(StretchDirectionProperty); }
            set { SetValue(StretchDirectionProperty, value); }
        }
    }
}