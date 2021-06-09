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
        private double centerRound, innerRound, outterRound;
        public ProgressRing() : base()
        {
            WidthProperty.Changed.AddClassHandler<ProgressRing>(OnWidthChange);
            fillBrush = new SolidColorBrush(Colors.White);
            if (double.IsNaN(Width) && double.IsNaN(Height))
            {
                Width = 128;
                Height = 128;
            }
        }
        private void OnWidthChange(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is double d)
            {
                centerRound = d / 2;
                innerRound = d * 0.6;
                outterRound = d * 0.8;
            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            DrawBase();
            DrawAnimation();
        }
        private Ellipse _moving;
        private double _movingLeft, _movingTop;
        private void DrawAnimation()
        {
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                while (IsVisible)
                {
                    double round = (outterRound - innerRound) / 2, r = round / 4;
                    if (_moving == null)
                    {
                        _moving = new Ellipse
                        {
                            ZIndex = 2,
                            Width = round,
                            Height = round,
                            Fill = fillBrush
                        };
                        _moving.Measure(new Size(round, round));
                        _moving.Arrange(new Rect(0, 0, round, round));
                        this.Children.Add(_moving);
                    }
                    if (_movingLeft == 0 && _movingTop == 0)
                    {
                        _movingTop = circleBounds + r;
                        _movingLeft = circleBounds + (outterRound / 2) - r;
                        SetTop(_moving, _movingTop);
                        SetLeft(_moving, _movingLeft);
                    }
                    else
                    {


                        SetTop(_moving, _movingTop);
                        SetLeft(_moving, _movingLeft);
                    }
                    await Task.Delay(80);
                }
            });
        }
        private double circleBounds;
        private void DrawBase()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
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
                var top = centerRound - innerRound / 2;
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
                circleBounds = centerRound - outterRound / 2;
                SetLeft(target, circleBounds);
                SetTop(target, circleBounds);
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