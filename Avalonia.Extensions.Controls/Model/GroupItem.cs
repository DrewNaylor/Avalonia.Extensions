using System.Collections;
using System.Linq;

namespace Avalonia.Extensions.Model
{
    public sealed class GroupItem
    {
        internal GroupItem(string header, IGrouping<string, dynamic> items)
        {
            this.Header = header;
            this.Children = items.ToArray();
        }
        public GroupItem(string header, IEnumerable items)
        {
            this.Header = header;
            this.Children = items;
        }
        public string Header { get; set; }
        public IEnumerable Children { get; set; }
    }
}