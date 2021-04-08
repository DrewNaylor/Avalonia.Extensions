using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Avalonia.Controls.Extensions.Utils
{
    internal static class IEnumerableUtils
    {
        public static int Count(this IEnumerable items)
        {
            if (items != null)
            {
                if (items is ICollection collection)
                    return collection.Count;
                else if (items is IReadOnlyCollection<object> readOnly)
                    return readOnly.Count;
                else
                    return Enumerable.Count(items.Cast<object>());
            }
            else
            {
                return 0;
            }
        }
    }
}