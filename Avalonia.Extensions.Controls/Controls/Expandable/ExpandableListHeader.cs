using Avalonia.Controls;

namespace Avalonia.Extensions.Controls
{
    public sealed class ExpandableListHeader : ClickableView
    {
        private string Header { get; }
        public ExpandableListHeader(string header)
        {
            this.Header = header;
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();
            TextBlock textBlock = new TextBlock
            {
                Text = Header,
                Margin = new Thickness(32, 0, 0, 0)
            };
            Children.Add(textBlock);
        }
    }
}