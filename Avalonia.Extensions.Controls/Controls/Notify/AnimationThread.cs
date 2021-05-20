using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Avalonia.Extensions.Controls
{
    internal sealed class AnimationThread
    {
        private Thread Thread { get; }
        private Window Window { get; }
        public bool Running { get; set; } = false;
        public PixelPoint StartPosition { get; set; }
        public PixelPoint StopPosition { get; set; }
        public AnimationThread(Window window)
        {
            this.Window = window;
            Thread = new Thread(RunJob);
            Thread.Start();
        }
        private void RunJob()
        {
            while (Running)
            {

            }
        }
    }
}