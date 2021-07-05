using Avalonia.Controls;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Controls
{
    public sealed class NotifyWindow : Window
    {
        private Options Options { get; }
        private AnimationThread Thread { get; }
        public NotifyWindow() : base()
        {
            CanResize = false;
            ShowInTaskbar = false;
            Thread = new AnimationThread(this);
            Thread.DisposeEvent += Thread_DisposeEvent;
            Options = new Options(ShowPosition.BottomRight);
            this.SystemDecorations = SystemDecorations.None;
        }
        private void Thread_DisposeEvent(object sender, EventArgs e)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                this.Close();
            });
        }
        public void Popup(int timeout)
        {
            Show();
            Dispatcher.UIThread.InvokeAsync(async() =>
            {
                await Task.Delay(timeout);
                this.Close();
            });
        }
        public void Show(Options options)
        {
            if (!options.IsVaidate)
                throw new NotSupportedException("when Position is Top***,the Scroll Way(ScollOrientation) cannot be Vertical!");
            try
            {
                Width = options.Size.Width;
                Height = options.Size.Height;
                Show();
                Options.Update(options);
                HandleAnimation(options.Position);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void HandleAnimation(ShowPosition showOn)
        {
            try
            {
                PixelPoint endPoint = default;
                int h = this.ActualHeight().ToInt32(), w = this.ActualWidth().ToInt32();
                int sw = Screens.Primary.WorkingArea.Width, sh = Screens.Primary.WorkingArea.Height;
                switch (showOn)
                {
                    case ShowPosition.BottomLeft:
                        {
                            var top = sh - h;
                            this.Position = new PixelPoint(0, top);
                            if (Options.ScollOrientation == ScollOrientation.Vertical)
                                endPoint = new PixelPoint(0, 0);
                            else
                                endPoint = new PixelPoint(-w, 0);
                            break;
                        }
                    case ShowPosition.BottomRight:
                        {
                            int left = sw - w, top = sh - h;
                            this.Position = new PixelPoint(left, top);
                            if (Options.ScollOrientation == ScollOrientation.Vertical)
                                endPoint = new PixelPoint(left, 0);
                            else
                                endPoint = new PixelPoint(sw, top);
                            break;
                        }
                    case ShowPosition.TopLeft:
                        {
                            this.Position = new PixelPoint(0, 0);
                            endPoint = new PixelPoint(-w, 0);
                            break;
                        }
                    case ShowPosition.TopRight:
                        {
                            var left = sw - w;
                            this.Position = new PixelPoint(left, 0);
                            endPoint = new PixelPoint(sw, 0);
                            break;
                        }
                }
                Thread.SetPath(Position, endPoint);
                Thread.Start(Options);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}