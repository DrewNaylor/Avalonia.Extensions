using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Controls
{
    public class ProgressRing : Canvas
    {
        private readonly SolidColorBrush fillBrush;
        private double centerRound, innerRound, horizontalSeek, verticalSeek;
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
                innerRound = d * 0.8;
                horizontalSeek = d / 80;
                verticalSeek = d / 40;
            }
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            IsVisible = false;
            DrawBase();
            DrawAnimation();
        }
        private Ellipse _moving;
        private double _movingLeft, _movingTop,_movingCenter;
        private void DrawAnimation()
        {
            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                while (IsVisible)
                {
                    double round = (Width - innerRound) / 2;
                    if (_moving == null)
                    {
                        _moving = new Ellipse
                        {
                            ZIndex = 2,
                            Width = round,
                            Height = round,
                            Fill = new SolidColorBrush(Colors.Red)
                        };
                        _moving.Measure(new Size(round, round));
                        _moving.Arrange(new Rect(0, 0, round, round));
                        this.Children.Add(_moving);
                    }
                    if (_movingLeft == 0 && _movingTop == 0)
                    {
                        _movingTop = 0;
                        _movingCenter = _movingLeft = (Width / 2) - (round / 2);
                        SetTop(_moving, _movingTop);
                        SetLeft(_moving, _movingLeft);
                    }
                    else
                    {
                        if (_movingLeft >= _movingCenter)
                        {
                            if (_movingTop >= _movingCenter)
                            {
                                //right-bottom circle
                                _movingTop += 1;
                                _movingLeft -= 1;
                            }
                            else
                            {
                                var radian = 1D.ToRadians();
                                var pointX = centerRound * Math.Cos(radian);
                                var pointY = centerRound * Math.Sin(radian);
                                //right-top circle
                                _movingTop += pointY;
                                _movingLeft += pointX;
                            }
                        }
                        else
                        {
                            if (_movingTop >= _movingCenter)
                            {
                                //left-bottom circle
                                _movingTop -= 1;
                                _movingLeft -= 1;
                            }
                            else
                            {
                                //left-top circle
                                _movingTop -= 1;
                                _movingLeft += 1;
                            }
                        }
                        SetTop(_moving, _movingTop);
                        SetLeft(_moving, _movingLeft);
                    }
                    await Task.Delay(60);
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
                    Width = Width,
                    Height = Width,
                    Fill = Core.Instance.PrimaryBrush
                };
                target.Measure(new Size(Width, Width));
                target.Arrange(new Rect(0, 0, Width, Width));
                this.Children.Add(target);
                circleBounds = centerRound - Width / 2;
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