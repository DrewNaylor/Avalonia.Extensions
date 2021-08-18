using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Extensions.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Runtime.InteropServices;

namespace Avalonia.Extensions.Segoe
{
    public static class AppBuilderDesktopExtensions
    {
        public static TAppBuilder UseSegoeFont<TAppBuilder>(this TAppBuilder builder)
       where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            builder.AfterSetup((_) =>
            {
                try
                {
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
            return builder;
        }
    }
}