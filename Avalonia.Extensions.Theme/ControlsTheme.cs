using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using System;
using System.Collections.Generic;

namespace Avalonia.Extensions.Theme
{
    public sealed class ControlsTheme : IStyle, IResourceProvider
    {
        private bool _isLoading;
        private IStyle[]? _loaded;
        private readonly Uri _baseUri;
        public ControlsTheme(Uri baseUri)
        {
            _baseUri = baseUri;
        }
        public ControlsTheme(IServiceProvider serviceProvider)
        {
            Contract.Requires<ArgumentNullException>(serviceProvider != null);
            _baseUri = ((IUriContext)serviceProvider.GetService(typeof(IUriContext))).BaseUri;
        }
        public IResourceHost? Owner => (Loaded as IResourceProvider)?.Owner;
        public IStyle Loaded
        {
            get
            {
                if (_loaded == null)
                {
                    _isLoading = true;
                    var loaded = (IStyle)AvaloniaXamlLoader.Load(new Uri("avares://Avalonia.Extensions.Theme/ThemeDictionary.xaml", UriKind.Absolute), _baseUri);
                    _loaded = new[] { loaded };
                    _isLoading = false;
                }
                return _loaded?[0]!;
            }
        }
        bool IResourceNode.HasResources => (Loaded as IResourceProvider)?.HasResources ?? false;
        IReadOnlyList<IStyle> IStyle.Children => _loaded ?? Array.Empty<IStyle>();
        public event EventHandler OwnerChanged
        {
            add
            {
                if (Loaded is IResourceProvider rp)
                    rp.OwnerChanged += value;
            }
            remove
            {
                if (Loaded is IResourceProvider rp)
                    rp.OwnerChanged -= value;
            }
        }
        public SelectorMatchResult TryAttach(IStyleable target, IStyleHost? host) => Loaded.TryAttach(target, host);
        public bool TryGetResource(object key, out object? value)
        {
            if (!_isLoading && Loaded is IResourceProvider p)
                return p.TryGetResource(key, out value);
            value = null;
            return false;
        }
        void IResourceProvider.AddOwner(IResourceHost owner) => (Loaded as IResourceProvider)?.AddOwner(owner);
        void IResourceProvider.RemoveOwner(IResourceHost owner) => (Loaded as IResourceProvider)?.RemoveOwner(owner);
    }
}