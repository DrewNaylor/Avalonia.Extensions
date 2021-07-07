using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;
using Avalonia.Styling;
using System;

namespace Avalonia.Extensions.Controls
{
    public static class ApplicationUtils
    {
        public static void SetChineseInputSupport(this Application application)
        {
            try
            {
                var style = new Style();
                var selector = default(Selector).OfType<TextBox>();
                style.Selector = selector;
                style.Setters.Add(new Setter(TemplatedControl.FontFamilyProperty,
                    new FontFamily("avares://Avalonia.Extensions.Chinese/Assets/Fonts#WenQuanYi Micro Hei")));
                application.Styles.Add(style);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}