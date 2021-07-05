using System.IO;

namespace Avalonia.Extensions.Controls
{
    internal partial class DownloadTask
    {
        internal sealed class Result
        {
            public Result(Stream stream)
            {
                Success = true;
                Stream = stream;
            }
            public Result(string message)
            {
                Success = false;
                Message = message;
            }
            public bool Success { get; set; }
            public string Message { get; set; }
            public Stream Stream { get; set; }
        }
    }
}