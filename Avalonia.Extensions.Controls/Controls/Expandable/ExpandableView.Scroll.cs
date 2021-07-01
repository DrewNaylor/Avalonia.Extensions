using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Media;

namespace Avalonia.Extensions.Controls
{
    public partial class ExpandableView
    {
        /// <summary>
        /// Defines the <see cref="Template"/> property.
        /// </summary>
        public static readonly StyledProperty<IControlTemplate> TemplateProperty =
            AvaloniaProperty.Register<ExpandableView, IControlTemplate>(nameof(Template));
        /// <summary>
        /// Gets or sets the template that defines the control's appearance.
        /// </summary>
        public IControlTemplate Template
        {
            get => GetValue(TemplateProperty);
            set => SetValue(TemplateProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="BorderBrush"/> property.
        /// </summary>
        public static readonly StyledProperty<IBrush> BorderBrushProperty = Border.BorderBrushProperty.AddOwner<ExpandableView>();
        /// <summary>
        /// Gets or sets the brush used to draw the control's border.
        /// </summary>
        public IBrush BorderBrush
        {
            get => GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="BorderThickness"/> property.
        /// </summary>
        public static readonly StyledProperty<Thickness> BorderThicknessProperty =
            Border.BorderThicknessProperty.AddOwner<ExpandableView>();
        /// <summary>
        /// Gets or sets the thickness of the control's border.
        /// </summary>
        public Thickness BorderThickness
        {
            get => GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }
        /// <summary>
        /// Defines the <see cref="Scroll"/> property.
        /// </summary>
        public static readonly DirectProperty<ExpandableView, IScrollable> ScrollProperty =
            AvaloniaProperty.RegisterDirect<ExpandableView, IScrollable>(nameof(Scroll), o => o.Scroll);
        private IScrollable _scroll;
        /// <summary>
        /// Gets the scroll information for the <see cref="ExpandableView"/>.
        /// </summary>
        public IScrollable Scroll
        {
            get => _scroll;
            private set => SetAndRaise(ScrollProperty, ref _scroll, value);
        }
        protected void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            Scroll = e.NameScope.Find<IScrollable>("PART_ScrollViewer");
        }
        public override void ApplyTemplate()
        {
            base.ApplyTemplate();

            if (Template != null)
            {
                var (_, nameScope) = Template.Build(this);
                if (nameScope == null)
                    nameScope = new NameScope();
                var e = new TemplateAppliedEventArgs(nameScope);
                OnApplyTemplate(e);
            }
        }
    }
}