using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Extensions.Styles;
using Avalonia.Styling;
using System;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// https://github.com/AvaloniaUI/Avalonia/tree/feature/icons/src
    /// </summary>
    public class FontIcon : TemplatedControl, IStyling
    {
        public static readonly StyledProperty<string> GlyphProperty =
             AvaloniaProperty.Register<FontIcon, string>(nameof(Glyph));
        public string Glyph
        {
            get => GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }
        Type IStyleable.StyleKey => typeof(TextBlock);
        public FontIcon()
        {
            this.InitStyle();
        }
    }
}