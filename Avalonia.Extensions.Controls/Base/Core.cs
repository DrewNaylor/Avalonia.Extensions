using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Themes.Fluent;
using System;
using System.Drawing;
using System.Globalization;
using System.Net.Http;
using System.Reflection;

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
        internal Assembly AppAssembly { get; set; }
        private Core()
        {
            FontDefault = new Font("Arial", 16);
        }
        public Font FontDefault { get; private set; }
        public bool IsEnglish => !CultureInfo.CurrentCulture.Name.Contains("zh", StringComparison.CurrentCultureIgnoreCase);
        private IAssetLoader assetLoader;
        public IAssetLoader AssetLoader
        {
            get
            {
                if (assetLoader == null)
                {
                    assetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    if (AppAssembly != null)
                        assetLoader.SetDefaultAssembly(AppAssembly);
                }
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
                    _primaryBrush = new SolidColorBrush(Media.Color.FromRgb(139, 68, 172));
                return _primaryBrush;
            }
        }
    }
}