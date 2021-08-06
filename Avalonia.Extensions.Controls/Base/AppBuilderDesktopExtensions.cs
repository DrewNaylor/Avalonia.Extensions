﻿using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Avalonia.Extensions.Controls
{
    public static class AppBuilderDesktopExtensions
    {
        public static TAppBuilder UseDoveExtensions<TAppBuilder>(this TAppBuilder builder) where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            Core.Instance.AppAssembly = builder.ApplicationType.Assembly;
            builder.AfterSetup((_) =>
            {
                Application.Current.Resources.Add("Dove.ThemeDictionary", "avares://Avalonia.Extensions.Theme/ThemeDictionary.xaml".AsResource());
                Application.Current.Resources.MergedDictionaries.Add("avares://Avalonia.Extensions.Theme/ThemeDictionary.xaml".AsStyle());
                if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        while (desktop.MainWindow != null)
                        {
                            Task.Delay(80).GetAwaiter().GetResult();
                            desktop.MainWindow.Closed += MainWindow_Closed;
                            break;
                        }
                    });
                }
            });
            return builder;
        }
        private static void MainWindow_Closed(object sender, EventArgs e)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.Windows.Count(x => x is MessageBox) == desktop.Windows.Count)
                    desktop.Shutdown();
            }
            Core.Instance.Dispose();
        }
    }
}