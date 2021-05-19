using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Avalonia.Extensions.Controls.Animations
{
    public sealed class DoubleAnimation : Animation.Animation
    {
        public DoubleAnimation() : base() { }
        public DoubleAnimation(int durationMilliseconds) : this()
        {
            Duration = TimeSpan.FromMilliseconds(durationMilliseconds);
        }
        public DoubleAnimation(int durationMilliseconds, int delayMilliseconds) : this()
        {
            Duration = TimeSpan.FromMilliseconds(durationMilliseconds);
            Delay = TimeSpan.FromMilliseconds(delayMilliseconds);
        }
        public void Show(Window window, Direction direction)
        {
            var width = window.Bounds.Width;
            var height = window.Bounds.Height;
            switch (direction)
            {
                case Direction.ToBottom:
                    {
                        break;
                    }
                case Direction.ToLeft:
                    {
                        var startKeyFrame = new KeyFrame { Cue = new Cue(0) };
                        startKeyFrame.Setters.Add(new Setter(Layoutable.WidthProperty, 0));
                        this.Children.Add(startKeyFrame);
                        var endKeyFrame = new KeyFrame { Cue = new Cue(width) };
                        endKeyFrame.Setters.Add(new Setter(Layoutable.WidthProperty, width));
                        this.Children.Add(endKeyFrame);
                        break;
                    }
                case Direction.ToRight:
                    {
                        break;
                    }
                case Direction.ToTop:
                    {
                        var startKeyFrame = new KeyFrame { Cue = new Cue(0) };
                        startKeyFrame.Setters.Add(new Setter(Layoutable.HeightProperty, 0));
                        this.Children.Add(startKeyFrame);
                        var endKeyFrame = new KeyFrame { Cue = new Cue(height) };
                        endKeyFrame.Setters.Add(new Setter(Layoutable.HeightProperty, height));
                        this.Children.Add(endKeyFrame);
                        break;
                    }
            }
            var windowStyle = new Style();
            windowStyle.Animations.Add(this);
            window.Styles.Add(windowStyle);
        }
    }
}