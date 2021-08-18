using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Extensions.Styles;
using Avalonia.Media;
using Avalonia.Styling;
using System;

namespace Avalonia.Extensions.Controls
{
    /// <summary>
    /// https://github.com/AvaloniaUI/Avalonia/tree/feature/icons/src
    /// </summary>
    public class FontIcon : TemplatedControl, IStyling
    {
        public static readonly StyledProperty<string> GlyphProperty =
             AvaloniaProperty.Register<FontIcon, string>(nameof(Glyph));
        public static readonly StyledProperty<FontIconCode> IconProperty =
             AvaloniaProperty.Register<FontIcon, FontIconCode>(nameof(Icon));
        public string Glyph
        {
            get => GetValue(GlyphProperty);
            set => SetValue(GlyphProperty, value);
        }
        public FontIconCode Icon
        {
            get => GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        Type IStyleable.StyleKey => typeof(TextBlock);
        public FontIcon()
        {
            this.InitStyle();
            SetValue(FontFamilyProperty, "Segoe MDL2 Assets");
            IconProperty.Changed.AddClassHandler<FontIcon>(OnIconChanged);
        }
        private void OnIconChanged(object sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is FontIconCode iconCode)
            {
                switch (iconCode)
                {
                    case FontIconCode.NavigationButton:
                        SetValue(GlyphProperty, "&#xE700;");
                        break;
                    case FontIconCode.Wifi:
                        SetValue(GlyphProperty, "&#xE701;");
                        break;
                    case FontIconCode.Bluetooth:
                        SetValue(GlyphProperty, "&#xE702;");
                        break;
                    case FontIconCode.ArrowDown:
                        SetValue(GlyphProperty, "&#xE70D;");
                        break;
                    case FontIconCode.ArrowUp:
                        SetValue(GlyphProperty, "&#xE70E;");
                        break;
                    case FontIconCode.Edit:
                        SetValue(GlyphProperty, "&#xE70F;");
                        break;
                    case FontIconCode.Add:
                        SetValue(GlyphProperty, "&#xE710;");
                        break;
                    case FontIconCode.Cancel:
                        SetValue(GlyphProperty, "&#xE711;");
                        break;
                    case FontIconCode.More:
                        SetValue(GlyphProperty, "&#xE712;");
                        break;
                    case FontIconCode.Setting:
                        SetValue(GlyphProperty, "&#xE713;");
                        break;
                    case FontIconCode.Mail:
                        SetValue(GlyphProperty, "&#xE715;");
                        break;
                    case FontIconCode.People:
                        SetValue(GlyphProperty, "&#xE716;");
                        break;
                    case FontIconCode.Phone:
                        SetValue(GlyphProperty, "&#xE717;");
                        break;
                    case FontIconCode.Shop:
                        SetValue(GlyphProperty, "&#xE719;");
                        break;
                    case FontIconCode.Stop:
                        SetValue(GlyphProperty, "&#xE71A;");
                        break;
                    case FontIconCode.Filter:
                        SetValue(GlyphProperty, "&#xE71C;");
                        break;
                    case FontIconCode.ZoomIn:
                        SetValue(GlyphProperty, "&#xE71E;");
                        break;
                    case FontIconCode.ZoomOut:
                        SetValue(GlyphProperty, "&#xE71F;");
                        break;
                    case FontIconCode.Record:
                        SetValue(GlyphProperty, "&#xE720;");
                        break;
                    case FontIconCode.Search:
                        SetValue(GlyphProperty, "&#xE721;");
                        break;
                    case FontIconCode.Camera:
                        SetValue(GlyphProperty, "&#xE722;");
                        break;
                    case FontIconCode.Send:
                        SetValue(GlyphProperty, "&#xE724;");
                        break;
                    case FontIconCode.Next:
                        SetValue(GlyphProperty, "&#xE72A;");
                        break;
                    case FontIconCode.Forward:
                        SetValue(GlyphProperty, "&#xE72B;");
                        break;
                    case FontIconCode.Refresh:
                        SetValue(GlyphProperty, "&#xE72C;");
                        break;
                    case FontIconCode.Share:
                        SetValue(GlyphProperty, "&#xE72D;");
                        break;
                    case FontIconCode.Lock:
                        SetValue(GlyphProperty, "&#xE72F;");
                        break;
                    case FontIconCode.Favorite:
                        SetValue(GlyphProperty, "&#xE734;");
                        break;
                    case FontIconCode.Check:
                        SetValue(GlyphProperty, "&#xE73E;");
                        break;
                    case FontIconCode.FullScreen:
                        SetValue(GlyphProperty, "&#xE740;");
                        break;
                    case FontIconCode.Up:
                        SetValue(GlyphProperty, "&#xE74A;");
                        break;
                    case FontIconCode.Down:
                        SetValue(GlyphProperty, "&#xE74B;");
                        break;
                    case FontIconCode.Delete:
                        SetValue(GlyphProperty, "&#xE74D;");
                        break;
                    case FontIconCode.Save:
                        SetValue(GlyphProperty, "&#xE74E;");
                        break;
                    case FontIconCode.SaveAs:
                        SetValue(GlyphProperty, "&#xE792;");
                        break;
                    case FontIconCode.Play:
                        SetValue(GlyphProperty, "&#xE768;");
                        break;
                    case FontIconCode.Pause:
                        SetValue(GlyphProperty, "&#xE769;");
                        break;
                    case FontIconCode.Emoji:
                        SetValue(GlyphProperty, "&#xE76E;");
                        break;
                    case FontIconCode.OpenWith:
                        SetValue(GlyphProperty, "&#xE7AC;");
                        break;
                    case FontIconCode.Redo:
                        SetValue(GlyphProperty, "&#xE7A6;");
                        break;
                    case FontIconCode.Undo:
                        SetValue(GlyphProperty, "&#xE7A7;");
                        break;
                    case FontIconCode.Waring:
                        SetValue(GlyphProperty, "&#xE7BA;");
                        break;
                    case FontIconCode.Power:
                        SetValue(GlyphProperty, "&#xE7E8;");
                        break;
                    case FontIconCode.Car:
                        SetValue(GlyphProperty, "&#xE7EC;");
                        break;
                    case FontIconCode.CC:
                        SetValue(GlyphProperty, "&#xE7F0;");
                        break;
                    case FontIconCode.Monitor:
                        SetValue(GlyphProperty, "&#xE7F4;");
                        break;
                    case FontIconCode.Speaker:
                        SetValue(GlyphProperty, "&#xE7F5;");
                        break;
                    case FontIconCode.EarPhone:
                        SetValue(GlyphProperty, "&#xE7F6;");
                        break;
                    case FontIconCode.Laptop:
                        SetValue(GlyphProperty, "&#xE7F8;");
                        break;
                    case FontIconCode.Game:
                        SetValue(GlyphProperty, "&#xE7FC;");
                        break;
                    case FontIconCode.History:
                        SetValue(GlyphProperty, "&#xE81C;");
                        break;
                    case FontIconCode.Recent:
                        SetValue(GlyphProperty, "&#xE823;");
                        break;
                    case FontIconCode.Like:
                        SetValue(GlyphProperty, "&#xE8E1;");
                        break;
                    case FontIconCode.DisLike:
                        SetValue(GlyphProperty, "&#xE8C6;");
                        break;
                    case FontIconCode.Copy:
                        SetValue(GlyphProperty, "&#xE8C8;");
                        break;
                    case FontIconCode.Cut:
                        SetValue(GlyphProperty, "&#xE8E0;");
                        break;
                    case FontIconCode.Help:
                        SetValue(GlyphProperty, "&#xE23;");
                        break;
                    case FontIconCode.:
                        SetValue(GlyphProperty, "&#xE916;");
                        break;
                    case FontIconCode.Fingerprint:
                        SetValue(GlyphProperty, "&#xE928;");
                        break;
                }
            }
        }
        public class FontIconSource : AvaloniaObject
        {
            public static readonly StyledProperty<string> GlyphProperty =
                FontIcon.GlyphProperty.AddOwner<FontIconSource>();
            /// <summary>
            /// Defines the <see cref="FontFamily"/> property.
            /// </summary>
            internal static readonly StyledProperty<FontFamily> FontFamilyProperty =
                TextBlock.FontFamilyProperty.AddOwner<FontIconSource>();
            /// <summary>
            /// Defines the <see cref="FontSize"/> property.
            /// </summary>
            public static readonly StyledProperty<double> FontSizeProperty =
                TextBlock.FontSizeProperty.AddOwner<FontIconSource>();
            /// <summary>
            /// Defines the <see cref="FontStyle"/> property.
            /// </summary>
            public static readonly StyledProperty<FontStyle> FontStyleProperty =
                TextBlock.FontStyleProperty.AddOwner<FontIconSource>();
            /// <summary>
            /// Defines the <see cref="FontWeight"/> property.
            /// </summary>
            public static readonly StyledProperty<FontWeight> FontWeightProperty =
                TextBlock.FontWeightProperty.AddOwner<FontIconSource>();
            public string Glyph
            {
                get => GetValue(GlyphProperty);
                set => SetValue(GlyphProperty, value);
            }
            /// <summary>
            /// Gets or sets the font family used to draw the control's text.
            /// </summary>
            internal FontFamily FontFamily
            {
                get => GetValue(FontFamilyProperty);
                set => SetValue(FontFamilyProperty, value);
            }
            /// <summary>
            /// Gets or sets the size of the control's text in points.
            /// </summary>
            public double FontSize
            {
                get => GetValue(FontSizeProperty);
                set => SetValue(FontSizeProperty, value);
            }
            /// <summary>
            /// Gets or sets the font style used to draw the control's text.
            /// </summary>
            public FontStyle FontStyle
            {
                get => GetValue(FontStyleProperty);
                set => SetValue(FontStyleProperty, value);
            }
            /// <summary>
            /// Gets or sets the font weight used to draw the control's text.
            /// </summary>
            public FontWeight FontWeight
            {
                get => GetValue(FontWeightProperty);
                set => SetValue(FontWeightProperty, value);
            }
            public static StyledProperty<IBrush?> ForegroundProperty =
                TemplatedControl.ForegroundProperty.AddOwner<FontIconSource>();
            public IBrush? Foreground
            {
                get => GetValue(ForegroundProperty);
                set => SetValue(ForegroundProperty, value);
            }
            public IDataTemplate IconElementTemplate { get; } = new FuncDataTemplate<FontIconSource>((source, _) => new FontIcon
            {
                [!ForegroundProperty] = source[!ForegroundProperty],
                [!GlyphProperty] = source[!GlyphProperty],
                [!FontFamilyProperty] = source[!FontFamilyProperty],
                [!FontSizeProperty] = source[!FontSizeProperty],
                [!FontStyleProperty] = source[!FontStyleProperty],
                [!FontWeightProperty] = source[!FontWeightProperty]
            });
        }
    }
}