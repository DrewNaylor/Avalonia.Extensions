using Avalonia.Layout;
using Avalonia.Media;

namespace Avalonia.Extensions.Controls
{
    public sealed class PopupOptions
    {
        public int Timeout => (int)Length;
        public float Width { get; set; } = float.NaN;
        public Color ForegroundColor { get; set; } = Colors.Black;
        public PopupLength Length { get; set; } = PopupLength.Default;
        public IBrush Foreground => new SolidColorBrush(ForegroundColor);
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;
    }
}