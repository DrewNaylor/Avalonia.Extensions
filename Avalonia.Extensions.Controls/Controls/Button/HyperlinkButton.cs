﻿using Avalonia.Controls;
using Avalonia.Extensions.Styles;
using Avalonia.Media;
using Avalonia.Styling;
using System;

namespace Avalonia.Extensions.Controls
{
    public class HyperlinkButton : Button, IStyling
    {
        private TextBlock TextContent { get; set; }
        Type IStyleable.StyleKey => typeof(Button);
        public HyperlinkButton() : base()
        {
            SetValue(ForegroundProperty, new SolidColorBrush(Colors.Blue));
            SetValue(BackgroundProperty, new SolidColorBrush(Colors.Transparent));
            SetValue(VerticalAlignmentProperty, Layout.VerticalAlignment.Center);
            SetValue(HorizontalAlignmentProperty, Layout.HorizontalAlignment.Center);
            Initialize();
            ContentProperty.Changed.AddClassHandler<HyperlinkButton>(OnContentChange);
        }
        private void Initialize()
        {
            TextContent = new TextBlock();
            TextContent.Foreground = this.Foreground;
            TextContent.Background = this.Background;
            base.Content = TextContent;
        }
        private void OnContentChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is string chars)
            {
                TextContent.Text = chars;
                TextContent.InvalidateMeasure();
                this.InvalidateMeasure();
            }
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
            AvaloniaProperty.Register<HyperlinkButton, string>(nameof(Content));
        public Uri NavigateUri
        {
            get => GetValue(NavigateUriProperty);
            set => SetValue(NavigateUriProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="IsDefaultProperty"/> property.
        /// </summary>
        public static readonly StyledProperty<Uri> NavigateUriProperty =
            AvaloniaProperty.Register<HyperlinkButton, Uri>(nameof(NavigateUri));
    }
}