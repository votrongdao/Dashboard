using System.Collections.Generic;

namespace DashboardSite.Core.Utilities
{
    public static class DictUtils
    {
        public static TItem GetOrCreate<TKey, TItem>(this Dictionary<TKey, TItem> dict, TKey key) where TItem : new()
        {
            if (!dict.ContainsKey(key))
            {
                dict[key] = new TItem();
            }
            return dict[key];
        }

        public static void SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value, bool bOverWritePrevious)
        {
            if (dict.ContainsKey(key))
            {
                if (bOverWritePrevious)
                {
                    dict.Remove(key);
                    dict.Add(key, value);
                }
            }
            else
            {
                dict.Add(key, value);
            }
        }
    }
}
