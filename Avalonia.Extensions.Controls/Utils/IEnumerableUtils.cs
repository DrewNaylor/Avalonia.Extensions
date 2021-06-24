using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Avalonia.Extensions.Controls
{
    public static class IEnumerableUtils
    {
        public static double All(this IEnumerable<double> items)
        {
            double total = 0;
            foreach (var item in items)
                total += item;
            return total;
        }
        public static int IndexOf(this IEnumerable items, object item)
        {
            if (items != null && item != null)
            {
                if (items is IList list)
                    return list.IndexOf(item);
                else
                {
                    int index = 0;
                    foreach (var i in items)
                    {
                        if (Equals(i, item))
                            return index;
                        ++index;
                    }
                }
            }
            return -1;
        }
        public static object ElementAt(this IEnumerable source, int index)
        {
            var i = -1;
            var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext() && ++i < index) ;
            if (i == index)
                return enumerator.Current;
            throw new ArgumentOutOfRangeException(nameof(index));
        }
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