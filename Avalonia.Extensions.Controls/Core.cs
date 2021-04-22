using System.Net.Http;

namespace Avalonia.Extensions.Controls
{
    public sealed class Core
    {
        private static Core instance;
        public static Core Instance
        {
            get
            {
                if (instance == null)
                    instance = new Core();
                return instance;
            }
        }
        private Core() { }
        private HttpClient HttpClient { get; set; }
        public const string WRAP_TEMPLATE = "<ItemsPanelTemplate xmlns='https://github.com/avaloniaui'><WrapPanel Orientation=\"Horizontal\"/></ItemsPanelTemplate>";
        public HttpClient GetClient()
        {
            if (HttpClient == null)
            {
                var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chaun, ssl) => { return true; };
                HttpClient = new HttpClient(clientHandler);
            }
            return HttpClient;
        }
    }
}