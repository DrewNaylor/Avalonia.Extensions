using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Metadata;
using System.Collections.Generic;

namespace Avalonia.Extensions.Controls
{
    public sealed class Run : AvaloniaObject
    {
        /// <summary>
        /// Defines the <see cref="Content"/> property.
        /// </summary>
        public static readonly StyledProperty<string> ContentProperty =
            AvaloniaProperty.Register<Run, string>(nameof(Content));
        /// <summary>
        /// Gets or sets the content to display.
        /// </summary>
        [Content]
        public string Content
        {
            get => GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }
    }
    public class Runs : AvaloniaList<Run>
    {
        public Runs()
        {
            ResetBehavior = ResetBehavior.Remove;
        }
        public Runs(IEnumerable<Run> items) : base(items)
        {
            ResetBehavior = ResetBehavior.Remove;
        }
    }
    public interface IRun : IControl
    {
        Runs Children { get; }
    }
}