using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Metadata;
using System;

namespace Avalonia.Extensions.Controls
{
    public sealed class TipLabel : Panel
    {
        private string _content;
        private Border DockPanel { get; }
        private TextBlock TextBlock { get; }
        public TipLabel() : base()
        {
            TextBlock = new TextBlock
            {
                ZIndex = 1,
                Text = this.Content,
                Foreground = this.Foreground,
                VerticalAlignment = Layout.VerticalAlignment.Center,
                HorizontalAlignment = Layout.HorizontalAlignment.Center
            };
            DockPanel = new Border
            {
                Padding = this.Padding,
                Background = this.Background,
                BorderBrush = this.BorderBrush,
                BorderThickness = this.BorderThickness
            };
            DockPanel.Child = TextBlock;
            Children.Add(DockPanel);
            ContentProperty.Changed.AddClassHandler<TipLabel>(OnContentChange);
            PaddingProperty.Changed.AddClassHandler<TipLabel>(OnPaddingChange);
            ForegroundProperty.Changed.AddClassHandler<TipLabel>(OnForegroundChange);
            BackgroundProperty.Changed.AddClassHandler<TipLabel>(OnBackgroundChange);
            BorderBrushProperty.Changed.AddClassHandler<TipLabel>(OnBorderBrushChange);
            BorderThicknessProperty.Changed.AddClassHandler<TipLabel>(OnBorderThicknessChange);
        }
        [Content]
        public string Content
        {
            get => _content;
            set => SetAndRaise(ContentProperty, ref _content, value);
        }
        /// <summary>
        /// Gets or sets a brush with which to paint the border.
        /// </summary>
        public IBrush BorderBrush
        {
            get { return GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }
        /// <summary>
        /// Gets or sets the padding to place around the <see cref="Child"/> control.
        /// </summary>
        public Thickness Padding
        {
            get => GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        /// <summary>
        /// Gets or sets the thickness of the border.
        /// </summary>
        public Thickness BorderThickness
        {
            get => GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }
        public IBrush Foreground
        {
            get => GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }
        public static readonly StyledProperty<IBrush> ForegroundProperty =
            AvaloniaProperty.Register<TipLabel, IBrush>(nameof(Foreground));
        /// <summary>
        /// Defines the <see cref="BorderBrush"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush> BorderBrushProperty =
            AvaloniaProperty.Register<TipLabel, IBrush>(nameof(BorderBrush));
        /// <summary>
        /// Defines the <see cref="BorderThickness"/> property.
        /// </summary>
        public static readonly StyledProperty<Thickness> BorderThicknessProperty =
            AvaloniaProperty.Register<TipLabel, Thickness>(nameof(BorderThickness));
        public static readonly StyledProperty<Thickness> PaddingProperty =
         AvaloniaProperty.Register<TipLabel, Thickness>(nameof(Padding));
        /// <summary>
        /// Defines the <see cref="ContentProperty"/> property.
        /// </summary>
        public static readonly DirectProperty<TipLabel, string> ContentProperty =
               AvaloniaProperty.RegisterDirect<TipLabel, string>(nameof(Content), o => o.Content, (o, v) => o.Content = v);
        private void OnContentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is string chars)
            {
                TextBlock.Text = chars;
                var size = chars.MeasureString(TextBlock.GetFont(), 0);
                if (size != null)
                {
                    var width = Convert.ToDouble(size.Width).Upper() + (Padding.Left + Padding.Right).Upper();
                    this.Width = DockPanel.Width = width;
                }
            }
        }
        private void OnBackgroundChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is IBrush brush)
                DockPanel.Background = brush;
        }
        private void OnPaddingChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is Thickness thickness)
            {
                DockPanel.Padding = thickness;
                this.Height += thickness.Bottom + thickness.Top;
            }
        }
        private void OnBorderThicknessChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is Thickness thickness)
                DockPanel.BorderThickness = thickness;
        }
        private void OnBorderBrushChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is IBrush brush)
                DockPanel.BorderBrush = brush;
        }
        private void OnForegroundChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is IBrush brush)
                TextBlock.Foreground = brush;
        }
    }
}