using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Avalonia.Extensions.Controls
{
    public static class AppBuilderDesktopExtensions
    {
        /// <summary>
        /// set chinese support fontfamily for controls
        /// </summary>
        /// <param name="supportContols">if default, it just works on <seealso cref="TextBox"/></param>
        public static TAppBuilder UseChineseInputSupport<TAppBuilder>(this TAppBuilder builder, IEnumerable<Type> supportContols = null)
            where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            builder.AfterSetup((_) =>
            {
                try
                {
                    if (supportContols == null || supportContols.Count() == 0)
                        supportContols = new List<Type> { typeof(TextBox), typeof(TextPresenter) };
                    ApplyStyle(supportContols);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
            return builder;
        }
        private static void ApplyStyle(IEnumerable<Type> supportContols)
        {
            foreach (var supportContol in supportContols)
            {
                if (supportContol.FullName.StartsWith("Avalonia.Controls", StringComparison.OrdinalIgnoreCase) ||
                    supportContol.FullName.StartsWith("Avalonia.Extensions.Controls", StringComparison.OrdinalIgnoreCase))
                {
                    var style = new Style();
                    var selector = default(Selector).OfType(supportContol);
                    style.Selector = selector;
                    style.Setters.Add(new Setter(TemplatedControl.FontFamilyProperty,
                        new FontFamily("avares://Avalonia.Extensions.Chinese/Assets/Fonts#WenQuanYi Micro Hei")));
                    Application.Current.Styles.Add(style);
                }
            }
        }
    }
}