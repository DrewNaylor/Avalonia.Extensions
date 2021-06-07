using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using AvaloniaGif;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Avalonia.Extensions.Controls
{
    public class ProgressRing : GifImage
    {
        public ProgressRing() : base()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                byte[] bytes = Convert.FromBase64String(Core.LOADING_IMAGE_CODE);
                using MemoryStream ms = new MemoryStream();
                ms.Write(bytes, 0, bytes.Length);
                this.SourceStream = ms;
            });
        }
        public void Show()
        {
            IsVisible = true;
        }
        public void Hidden()
        {
            IsVisible = false;
        }
    }
}