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
        private Window Window { get; }
        private PixelPoint Next { get; set; }
        private Options Options { get; set; }
        private PixelPoint StopPosition { get; set; }
        private PixelPoint StartPosition { get; set; }
        public AnimationThread(Window window)
        {
            this.Window = window;
            Thread = new Thread(RunJob) { IsBackground = true };
        }
        public void Start(Options options)
        {
            this.Options = options;
            Next = new PixelPoint(0, -Options.MovePixel);
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
                    switch (Options.Position)
                    {
                        case ShowPosition.BottomLeft:
                            {
                                if (Options.ScollOrientation == ScollOrientation.Vertical)
                                    await BottomVertical();
                                else
                                {

                                }
                                break;
                            }
                        case ShowPosition.BottomRight:
                            {
                                if (Options.ScollOrientation == ScollOrientation.Vertical)
                                    await BottomVertical();
                                else
                                {

                                }
                                break;
                            }
                        case ShowPosition.TopLeft:
                            {

                                break;
                            }
                        case ShowPosition.TopRight:
                            {

                                break;
                            }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("AnimationThread RunJob ERROR : " + ex.Message);
                }
            }
        }
        private async Task BottomVertical()
        {
            StartPosition -= Next;
            Window.Position = StartPosition;
            if (StartPosition == StopPosition)
            {
                Thread.Abort();
                await Task.Delay(Options.MoveDelay);
                Window.Close();
            }
            else
                await Task.Delay(Options.MoveDelay);
        }
    }
}