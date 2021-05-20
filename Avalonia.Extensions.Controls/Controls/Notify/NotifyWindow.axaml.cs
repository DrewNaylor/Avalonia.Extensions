using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Threading;
using System.Timers;

namespace Avalonia.Extensions.Controls
{
    public partial class NotifyWindow : Window
    {
        private AnimationThread Thread { get; }
        public NotifyWindow()
        {
            AvaloniaXamlLoader.Load(this);
            Thread = new AnimationThread(this);
            InitializeComponent();
        }
        private void InitializeComponent()
        {

        }
        public void Show(Postion showOn)
        {
            switch (showOn)
            {
                case Postion.BottomLeft:
                    {
                        var h = Screens.Primary.WorkingArea.Height - Convert.ToInt32(Height);
                        this.Position = new PixelPoint(0, h);

                        break;
                    }
                case Postion.BottomRight:
                    {
                        var w = Screens.Primary.WorkingArea.Width - Convert.ToInt32(Width);
                        var h = Screens.Primary.WorkingArea.Height - Convert.ToInt32(Height);
                        this.Position = new PixelPoint(w, h);

                        break;
                    }
                case Postion.TopLeft:
                    {
                        this.Position = new PixelPoint(0, 0);

                        break;
                    }
                case Postion.TopRight:
                    {
                        var w = Screens.Primary.WorkingArea.Width - Convert.ToInt32(Width);
                        this.Position = new PixelPoint(w, 0);

                        break;
                    }
            }
            Thread.StartPosition = Position;
            Thread.Running = true;
        }
    }
}