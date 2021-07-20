using Avalonia.Controls;
using System;
using System.Collections.Generic;

namespace Avalonia.Extensions.Controls
{
    public static class StyleUtils
    {
        public static TSource GetItem<TSource>(this IList<IResourceProvider> resources, Func<TSource, bool> predicate)
        {
            TSource result = default;
            var enumerator = resources.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (enumerator.Current is TSource obj && predicate.Invoke(obj))
                {
                    result = obj;
                    break;
                }
            }
            return result;
        }
    }
}