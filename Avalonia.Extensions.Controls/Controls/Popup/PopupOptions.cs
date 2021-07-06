using Avalonia.Layout;

namespace Avalonia.Extensions.Controls
{
    public sealed class PopupOptions
    {
        public int Timeout => (int)Length;
        public float Width { get; set; } = float.NaN;
        public PopupLength Length { get; set; } = PopupLength.Default;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Center;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Center;
    }
}