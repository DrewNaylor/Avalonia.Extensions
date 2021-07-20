﻿using Avalonia.Markup.Xaml.MarkupExtensions;
using Avalonia.Themes.Fluent;
using System;

namespace Avalonia.Extensions.Controls
{
    public sealed class Themes
    {
        internal static FluentThemeMode theme;
        private const string DARK_URL = "avares://Avalonia.Extensions.Theme/Themes/Dark.xaml";
        private const string LIGHT_URL = "avares://Avalonia.Extensions.Theme/Themes/Light.xaml";
        public static void AddTheme(FluentThemeMode theme)
        {
            Themes.theme = theme;
            var xaml = theme == FluentThemeMode.Dark ? DARK_URL : LIGHT_URL;
            Application.Current.Resources.MergedDictionaries.Add(xaml.AsResource());
        }
        public static void SwitchDark()
        {
            if (theme != FluentThemeMode.Dark)
            {
                theme = FluentThemeMode.Dark;
                var resourceInclude = Application.Current.Resources.MergedDictionaries.GetItem<ResourceInclude>(x => x.Source != null && x.Source.ToString().Contains(LIGHT_URL));
                if (resourceInclude != null)
                    resourceInclude.Source = new Uri(DARK_URL);
                else
                    Application.Current.Resources.MergedDictionaries.Add(DARK_URL.AsResource());
            }
        }
        public static void SwitchLight()
        {
            if (theme != FluentThemeMode.Light)
            {
                theme = FluentThemeMode.Light;
                var resourceInclude = Application.Current.Resources.MergedDictionaries.GetItem<ResourceInclude>(x => x.Source != null && x.Source.ToString().Contains(DARK_URL));
                if (resourceInclude != null)
                    resourceInclude.Source = new Uri(LIGHT_URL);
                else
                    Application.Current.Resources.MergedDictionaries.Add(LIGHT_URL.AsResource());
            }
        }
    }
}