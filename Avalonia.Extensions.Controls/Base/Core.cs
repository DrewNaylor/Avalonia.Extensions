﻿using Avalonia.Media;
using Avalonia.Platform;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net.Http;
using System.Reflection;

namespace Avalonia.Extensions.Controls
{
    public sealed class Core : IDisposable
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
            InnerClasses = new List<Uri>();
            FontDefault = new Font("Arial", 16);
            Transparent = new SolidColorBrush(Colors.Transparent);
        }
        public void Init()
        {
            AssetLoader = AvaloniaLocator.Current.GetService<IAssetLoader>();
            var assets = AssetLoader.GetAssets(new Uri("avares://Avalonia.Extensions.Controls/Styles/Xaml"),
                  new Uri("avares://Avalonia.Extensions.Controls"));
            var enumerator = assets.GetEnumerator();
            while (enumerator.MoveNext())
                InnerClasses.Add(enumerator.Current);
        }
        public Font FontDefault { get; }
        public List<Uri> InnerClasses { get; private set; }
        public bool IsEnglish => !CultureInfo.CurrentCulture.Name.Contains("zh", StringComparison.CurrentCultureIgnoreCase);
        public IAssetLoader AssetLoader { get; private set; }
        private HttpClient HttpClient { get; set; }
        public const string WRAP_TEMPLATE = "<ItemsPanelTemplate xmlns='https://github.com/avaloniaui'><WrapPanel Orientation=\"Horizontal\"/></ItemsPanelTemplate>";
        public HttpClient GetClient()
        {
            if (HttpClient == null)
            {
                var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback += (_, _, _, _) => { return true; };
                HttpClient = new HttpClient(clientHandler);
            }
            return HttpClient;
        }
        public void Dispose()
        {
            try
            {
                InnerClasses.Clear();
                HttpClient.Dispose();
                InnerClasses = null;
                _primaryBrush = null;
            }
            catch { }
        }
        internal SolidColorBrush Transparent { get; }
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