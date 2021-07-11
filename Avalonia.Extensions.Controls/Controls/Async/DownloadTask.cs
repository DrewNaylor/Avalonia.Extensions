using System;
using System.Net.Http;

namespace Avalonia.Extensions.Controls
{
    internal partial class DownloadTask
    {
        private bool Loading = false;
        private HttpClient HttpClient { get; }
        public DownloadTask()
        {
            HttpClient = Core.Instance.GetClient();
        }
        public void Create(Uri uri, Action<Result> callBack)
        {
            Create(uri.ToString(), callBack);
        }
        public async void Create(string url, Action<Result> callBack)
        {
            if (!Loading)
            {
                Loading = true;
                try
                {
                    if (!string.IsNullOrEmpty(url))
                    {
                        HttpResponseMessage hr = await HttpClient.GetAsync(url);
                        hr.EnsureSuccessStatusCode();
                        using var stream = await hr.Content.ReadAsStreamAsync();
                        callBack?.Invoke(new Result(stream));
                    }
                }
                catch (Exception ex)
                {
                    callBack?.Invoke(new Result(ex.Message));
                }
                Loading = false;
            }
        }
    }
}