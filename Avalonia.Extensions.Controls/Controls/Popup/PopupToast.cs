using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Controls
{
    public class PopupToast : Window
    {
        public PopupToast() : base()
        {
            Topmost = true;
            CanResize = false;
            ShowInTaskbar = false;
            this.SystemDecorations = SystemDecorations.None;
        }
        public void Popup(string content)
        {
            PopupOptions options = new PopupOptions();
            Popup(content, options);
        }
        public void Popup(string content, PopupOptions options = default)
        {
            TextWrapping wrapping = TextWrapping.NoWrap;
            if (double.IsNaN(options.Width))
            {
                var size = PlatformImpl.MeasureString(content);
                this.Width = size.Width;
                this.Height = size.Height;
            }
            else
            {
                var size = PlatformImpl.MeasureString(content, options.Width);
                this.Width = options.Width;
                this.Height = size.Height;
                if (PlatformImpl.MeasureString(content).Width > size.Width)
                    wrapping = TextWrapping.WrapWithOverflow;
            }
            this.Content = new TextBlock
            {
                Text = content,
                TextWrapping = wrapping,
                VerticalAlignment = options.VerticalAlignment,
                HorizontalAlignment = options.HorizontalAlignment
            };
            this.Background = new SolidColorBrush(Color.Parse("#333"), 0.6);
            SetLoaction();
            Show();
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await Task.Delay(options.Timeout);
                this.Close();
            });
        }
        public virtual void SetLoaction()
        {
            var workSize = Screens.Primary.WorkingArea.Size;
            var y = (workSize.Height / 8) * 7;
            var x = (workSize.Width - this.Width) / 2;
            Position = new PixelPoint(Convert.ToInt32(x), y);
        }
    }
}