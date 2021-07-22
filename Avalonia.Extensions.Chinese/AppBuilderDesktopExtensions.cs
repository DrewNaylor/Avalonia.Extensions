using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;

namespace Avalonia.Extensions.Controls
{
    public static class AppBuilderDesktopExtensions
    {
        /// <summary>
        /// set chinese support fontfamily for controls
        /// </summary>
        /// <param name="supportContols">if default, it just works on <seealso cref="TextBox"/></param>
        public static TAppBuilder UseChineseInputSupport<TAppBuilder>(this TAppBuilder builder, List<IControl> supportContols = null)
            where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            builder.AfterSetup((_) =>
            {
                try
                {
                    if (supportContols == null || supportContols.Count == 0)
                    {
                        var style = new Style();
                        var selector = default(Selector).OfType<TextBox>();
                        style.Selector = selector;
                        style.Setters.Add(new Setter(TemplatedControl.FontFamilyProperty,
                            new FontFamily("avares://Avalonia.Extensions.Chinese/Assets/Fonts#WenQuanYi Micro Hei")));
                        Application.Current.Styles.Add(style);
                    }
                    else
                    {
                        foreach (var supportContol in supportContols)
                        {
                            var supportContolType = supportContol.GetType();
                            if (supportContolType.FullName.StartsWith("Avalonia.Controls", StringComparison.OrdinalIgnoreCase) ||
                                supportContolType.FullName.StartsWith("Avalonia.Extensions.Controls", StringComparison.OrdinalIgnoreCase))
                            {
                                var style = new Style();
                                var selector = default(Selector).OfType(supportContolType);
                                style.Selector = selector;
                                style.Setters.Add(new Setter(TemplatedControl.FontFamilyProperty,
                                    new FontFamily("avares://Avalonia.Extensions.Chinese/Assets/Fonts#WenQuanYi Micro Hei")));
                                Application.Current.Styles.Add(style);
                            }
                        }
                    }
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