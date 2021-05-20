using Avalonia.Controls;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Controls
{
    internal sealed class AnimationThread
    {
        private Thread Thread { get; }
        private PixelPoint Next { get; }
        private Window Window { get; }
        private PixelPoint StopPosition { get; set; }
        private PixelPoint StartPosition { get; set; }
        public AnimationThread(Window window)
        {
            this.Window = window;
            Next = new PixelPoint(0, -1);
            Thread = new Thread(RunJob) { IsBackground = true };
        }
        public void Start()
        {
            Thread.Start();
        }
        public void SetPath(PixelPoint startPosition, PixelPoint endPosition)
        {
            this.StartPosition = startPosition;
            this.StopPosition = endPosition;
        }
        private async void RunJob()
        {
            while (true)
            {
                try
                {
                    StartPosition -= Next;
                    Window.Position = StartPosition;
                    if (StartPosition == StopPosition)
                    {
                        Thread.Abort();
                        await Task.Delay(20);
                        Window.Close();
                    }
                    else
                        await Task.Delay(20);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("AnimationThread RunJob ERROR : " + ex.Message);
                }
            }
        }
    }
}