using System.Collections.Generic;
using System.Linq;

namespace iPreo.Bigdough.Utilities
{
    public static class LinqUtils
    {
        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> enumerable, System.Func<T, TKey> keySelector, bool ascending)
        {
            return enumerable.OrderBy(keySelector, null, ascending);
        }

        public static IOrderedEnumerable<T> OrderBy<T, TKey>(this IEnumerable<T> enumerable, System.Func<T, TKey> keySelector, IComparer<TKey> keyComparer, bool ascending)
        {
            return ascending ? enumerable.OrderBy(keySelector, keyComparer) : enumerable.OrderByDescending(keySelector, keyComparer);
        }

        public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> enumerable, System.Func<T, TKey> keySelector, bool ascending)
        {
            return enumerable.ThenBy(keySelector, null, ascending);
        }

        public static IOrderedEnumerable<T> ThenBy<T, TKey>(this IOrderedEnumerable<T> enumerable, System.Func<T, TKey> keySelector, IComparer<TKey> keyComparer, bool ascending)
        {
            return ascending ? enumerable.ThenBy(keySelector, keyComparer) : enumerable.ThenByDescending(keySelector, keyComparer);
        }
    }
}
