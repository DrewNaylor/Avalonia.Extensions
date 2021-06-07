using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Controls
{
    public class ProgressRing : Canvas
    {
        private SolidColorBrush fillBrush;
        public ProgressRing() : base()
        {
            fillBrush = new SolidColorBrush(Colors.White);
            if (double.IsNaN(Width) && double.IsNaN(Height))
            {
                Width = 128;
                Height = 128;
            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            DrawBase();
            DrawAnimation();
        }
        private Ellipse _scroll;
        private void DrawAnimation()
        {
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                while (IsVisible)
                {
                    double centerLength = Height / 2, round = Width * 0.2;
                    if (_scroll == null)
                    {
                        _scroll = new Ellipse
                        {
                            ZIndex = 2,
                            Width = round,
                            Height = round,
                            Fill = fillBrush
                        };
                        _scroll.Measure(new Size(round, round));
                        _scroll.Arrange(new Rect(0, 0, round, round));
                        this.Children.Add(_scroll);
                    }
                    var top = (centerLength - round) / 2;
                    SetLeft(_scroll, top);
                    SetTop(_scroll, top);
                    await Task.Delay(80);
                }
            });
        }
        private void DrawBase()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                double centerLength = Height / 2, innerRound = Width * 0.6, outterRound = Width * 0.8;
                var target = new Ellipse
                {
                    ZIndex = 1,
                    Width = innerRound,
                    Height = innerRound,
                    Fill = fillBrush
                };
                target.Measure(new Size(innerRound, innerRound));
                target.Arrange(new Rect(0, 0, innerRound, innerRound));
                this.Children.Add(target);
                var top = (centerLength - innerRound) / 2;
                SetLeft(target, top);
                SetTop(target, top);

                target = new Ellipse
                {
                    Width = outterRound,
                    Height = outterRound,
                    Fill = Core.Instance.PrimaryBrush
                };
                target.Measure(new Size(outterRound, outterRound));
                target.Arrange(new Rect(0, 0, outterRound, outterRound));
                this.Children.Add(target);
                top = (centerLength - outterRound) / 2;
                SetLeft(target, top);
                SetTop(target, top);
            });
        }
        public void Show()
        {
            IsVisible = true;
            DrawAnimation();
        }
        public void Hidden()
        {
            IsVisible = false;
        }
    }
}