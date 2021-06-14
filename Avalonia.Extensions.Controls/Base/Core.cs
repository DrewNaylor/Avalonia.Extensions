using Avalonia.Media;
using Avalonia.Platform;
using System.Globalization;
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
        public bool IsEnglish => !CultureInfo.CurrentCulture.Name.Contains("zh", System.StringComparison.CurrentCultureIgnoreCase);
        private IAssetLoader assetLoader;
        public IAssetLoader AssetLoader
        {
            get
            {
                if (assetLoader == null)
                    assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
                return assetLoader;
            }
        }
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
        private SolidColorBrush _primaryBrush;
        public SolidColorBrush PrimaryBrush
        {
            get
            {
                if (_primaryBrush == null)
                    _primaryBrush = new SolidColorBrush(Color.FromRgb(139, 68, 172));
                return _primaryBrush;
            }
        }
    }
}