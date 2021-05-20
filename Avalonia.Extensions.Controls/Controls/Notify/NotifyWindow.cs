using Avalonia.Controls;
using System;

namespace Avalonia.Extensions.Controls
{
    public sealed class NotifyWindow : Window
    {
        private AnimationThread Thread { get; }
        public NotifyWindow() : base()
        {
            Thread = new AnimationThread(this);
        }
        public void Show(Postion showOn, Size? size = null)
        {
            try
            {
                if (size == null)
                    size = new Size(160, 60);
                Width = size.Value.Width;
                Height = size.Value.Height;
                base.Show();
                HandleAnimation(showOn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void HandleAnimation(Postion showOn)
        {
            try
            {
                PixelPoint endPoint = default;
                int h = this.ActualHeight().ToInt32(), w = this.ActualWidth().ToInt32();
                switch (showOn)
                {
                    case Postion.BottomLeft:
                        {
                            var top = Screens.Primary.WorkingArea.Height - w;
                            this.Position = new PixelPoint(0, top);
                            endPoint = new PixelPoint(0, 0);
                            break;
                        }
                    case Postion.BottomRight:
                        {
                            var left = Screens.Primary.WorkingArea.Width - w;
                            var top = Screens.Primary.WorkingArea.Height - h;
                            this.Position = new PixelPoint(left, top);
                            endPoint = new PixelPoint(left, 0);
                            break;
                        }
                }
                Thread.SetPath(Position, endPoint);
                Thread.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}