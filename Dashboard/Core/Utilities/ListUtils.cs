using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Web.UI;

namespace iPreo.Bigdough.Utilities
{
    public enum ListOutputStyle
    {
        CommaSeparated,
        NumerableList
    }
    /// <summary>
    /// Function with 0 arguments
    /// </summary>
    /// <typeparam name="TR">Result type</typeparam>
    /// <returns>result</returns>
    public delegate TR Func<TR>();


    /// <summary>
    /// Function with 1 argument
    /// </summary>
    /// <typeparam name="TR">Result type</typeparam>
    /// <typeparam name="T1">Arg 1 type</typeparam>
    /// <param name="a1">argument 1</param>
    /// <returns>result</returns>
    public delegate TR Func<T1, TR>(T1 a1);


    /// <summary>
    /// Function with 2 arguments
    /// </summary>
    /// <typeparam name="TR">Restul type</typeparam>
    /// <typeparam name="T1">Arg 1 type</typeparam>
    /// <typeparam name="T2">Arg 2 type</typeparam>
    /// <param name="a1">argument 1</param>
    /// <param name="a2">argument 2</param>
    /// <returns>result</returns>
    public delegate TR Func<T1, T2, TR>(T1 a1, T2 a2);


    /// <summary>
    /// Function with 3 arguments
    /// </summary>
    /// <typeparam name="TR">Restul type</typeparam>
    /// <typeparam name="T1">Arg 1 type</typeparam>
    /// <typeparam name="T2">Arg 2 type</typeparam>
    /// <typeparam name="T3">Arg 3 type</typeparam>
    /// <param name="a1">argument 1</param>
    /// <param name="a2">argument 2</param>
    /// <param name="a3">argument 3</param>
    /// <returns>result</returns>
    public delegate TR Func<T1, T2, T3, TR>(T1 a1, T2 a2, T3 a3);


    /// <summary>
    /// Function with 4 arguments
    /// </summary>
    /// <typeparam name="TR">Restul type</typeparam>
    /// <typeparam name="T1">Arg 1 type</typeparam>
    /// <typeparam name="T2">Arg 2 type</typeparam>
    /// <typeparam name="T3">Arg 3 type</typeparam>
    /// <typeparam name="T4">Arg 4 type</typeparam>
    /// <param name="a1">argument 1</param>
    /// <param name="a2">argument 2</param>
    /// <param name="a3">argument 3</param>
    /// <param name="a4">argument 3</param>
    /// <returns>result</returns>
    public delegate TR Func<T1, T2, T3, T4, TR>(T1 a1, T2 a2, T3 a3, T4 a4);


    public static class ListUtils<TItem>
    {
        const string COMMA_SEPARATOR = ", ";

        public static IEnumerable<string> GetIdsInCommaDelimitedStringBatches(List<int> oList, int iBatchSize)
        {
            if (oList == null) throw new ArgumentNullException("oList");
            if (iBatchSize < 0) throw new ArgumentOutOfRangeException("iBatchSize", iBatchSize, "iBatchSize must have positive value.");

            foreach (IEnumerable<int> oBatchList in ListUtils<int>.GetBatches(oList, iBatchSize))
            {
                yield return ListUtils<int>.Join(oBatchList, ",");
            }
        }

        public static IEnumerable<IEnumerable<TItem>> GetBatches(List<TItem> oList, int iBatchSize)
        {
            if (oList == null) throw new ArgumentNullException("oList");
            if (iBatchSize < 0) throw new ArgumentOutOfRangeException("iBatchSize", iBatchSize, "iBatchSize must have positive value.");

            if (oList.Count == 0) yield break;

            if (iBatchSize == 0)
            {
                yield return oList;
            }
            else
            {
                int iBatchCount = oList.Count/iBatchSize;
                int iItemsCount = iBatchCount*iBatchSize;

                for (int iIndex = 0; iIndex < iItemsCount; iIndex += iBatchSize)
                {
                    yield return GetBatchItems(oList, iIndex, iIndex + iBatchSize);
                }

                int iLastBatchSize = oList.Count%iBatchSize;
                if (iLastBatchSize != 0)
                {
                    yield return GetBatchItems(oList, iItemsCount, iItemsCount + iLastBatchSize);
                }
            }
        }

        private static IEnumerable<TItem> GetBatchItems(List<TItem> oList, int iStartIndex, int iCount)
        {
            for (int j = iStartIndex; j < iCount; j++)
                yield return oList[j];
        }

        /// <summary>
        /// Check if item exists
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="predicate"><see cref="Predicate{T}"/></param>
        /// <returns>True, if item was found</returns>
        public static bool Exists(IEnumerable list, Predicate<TItem> predicate)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (predicate == null)
                throw new ArgumentNullException("predicate");

            foreach (TItem item in list)
            {
                if (predicate(item))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Filter items
        /// </summary>
        /// <param name="list">source</param>
        /// <param name="match">match predicate</param>
        /// <returns>Filtered data</returns>
        public static IEnumerable<TItem> FilterOutBase(IEnumerable list, Predicate<TItem> match)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (match == null)
                throw new ArgumentNullException("match");

            foreach (TItem item in list)
            {
                if (match(item))
                    yield return item;
            }
        }


        /// <summary>
        /// Filter items
        /// </summary>
        /// <param name="list">source</param>
        /// <param name="match">match predicate</param>
        /// <returns>Filtered data</returns>
        public static IEnumerable<TItem> FilterOut(IEnumerable<TItem> list, Predicate<TItem> match)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (match == null)
                throw new ArgumentNullException("match");

            foreach (TItem item in list)
            {
                if (match(item))
                    yield return item;
            }
        }

        /// <summary>
        /// Filter items
        /// </summary>
        /// <param name="list">source</param>
        /// <param name="match">match predicate</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns>Filtered data</returns>
        public static IEnumerable<K> FilterOut<K>(IEnumerable<TItem> list, Predicate<TItem> match, Func<TItem, K> keySelector)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (match == null)
                throw new ArgumentNullException("match");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            foreach (TItem item in list)
            {
                if (match(item))
                    yield return keySelector(item);
            }
        }

        /// <summary>
        /// Get Keys for list
        /// </summary>
        /// <typeparam name="K">type of Key</typeparam>
        /// <param name="list">source</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns>IEnumerable of keys</returns>
        public static IEnumerable<K> GetKeys<K>(IEnumerable<TItem> list, Func<TItem, K> keySelector)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            foreach (TItem item in list)
                yield return keySelector(item);
        }


        /// <summary>
        /// Filter items by criteria
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="match">Match <see  cref="Predicate{T}"/></param>
        /// <returns>List of filtered items</returns>
        public static List<TItem> Filter(IEnumerable list, Predicate<TItem> match)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (match == null)
                throw new ArgumentNullException("match");

            List<TItem> oList = new List<TItem>();
            foreach (TItem oItem in list)
            {
                if (match(oItem))
                    oList.Add(oItem);
            }
            return oList;
        }


        /// <summary>
        /// Fins first item
        /// </summary>
        /// <param name="list">List </param>
        /// <param name="match">Match <see  cref="Predicate{T}"/></param>
        /// <returns>Item of null if not found</returns>
        public static TItem FindFirst(IEnumerable list, Predicate<TItem> match)
        {
            #region Check params

            if (list == null)
                throw new ArgumentNullException("list");
            if (match == null)
                throw new ArgumentNullException("match");

            #endregion Check params


            foreach (TItem oItem in list)
            {
                if (match(oItem))
                    return oItem;
            }

            return default(TItem);
        }


        /// <summary>
        /// return distinct valies from list
        /// </summary>
        /// <param name="source">source</param>
        /// <returns></returns>
        public static IEnumerable<TItem> Distinct(IEnumerable<TItem> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            IDictionary<TItem, object> distinct = new SortedList<TItem, object>();
            foreach (TItem item in source)
            {
                distinct[item] = null;
            }

            foreach (TItem key in distinct.Keys)
            {
                yield return key;
            }
        }


        /// <summary>
        /// return distinct valies from list
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="keySelector">Key selector</param>
        /// <returns></returns>
        public static IEnumerable<K> Distinct<K>(IEnumerable<TItem> source, Func<TItem, K> keySelector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");

            IDictionary<K, object> distinct = new SortedList<K, object>();
            foreach (TItem item in source)
            {
                distinct[keySelector(item)] = null;
            }

            foreach (K key in distinct.Keys)
            {
                yield return key;
            }
        }


        /// <summary>
        /// Fins first item
        /// </summary>
        /// <param name="list">List </param>
        /// <param name="match">Match <see  cref="Predicate{T}"/></param>
        /// <returns>Item of null if not found</returns>
        public static TItem FindFirst(IEnumerable<TItem> list, Predicate<TItem> match)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (match == null)
                throw new ArgumentNullException("match");

            foreach (TItem item in list)
            {
                if (match(item))
                    return item;
            }
            return default(TItem);
        }


        /// <summary>
        /// Delete all items from list
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="match">Match <see  cref="Predicate{T}"/></param>
        /// <return>Number of items deleted</return>
        public static int DeleteAll(IList list, Predicate<TItem> match)
        {
            #region Check params

            if (list == null)
                throw new ArgumentNullException("list");
            if (match == null)
                throw new ArgumentNullException("match");

            #endregion Check params


            List<TItem> deleted = Filter(list, match);
            if (deleted.Count == 0)
                return 0;

            foreach (TItem item in deleted)
            {
                list.Remove(item);
            }
            return deleted.Count;
        }


        /// <summary>
        /// Apply action for each item in  array
        /// </summary>
        /// <param name="list"></param>
        /// <param name="action"></param>
        public static void ForEach(IEnumerable list, Action<TItem> action)
        {
            #region Check params

            if (list == null)
                throw new ArgumentNullException("list");

            if (action == null)
                throw new ArgumentNullException("action");

            #endregion Check params


            foreach (TItem item in list)
            {
                action(item);
            }
        }


        /// <summary>
        /// Add item if not exists
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="item">item</param>
        public static void AddNonExisting(IList<TItem> list, TItem item)
        {
            if (!list.Contains(item))
            {
                list.Add(item);
            }
        }


        /// <summary>
        /// Add item if not exists
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="match">Matcher</param>
        /// <param name="item">item</param>
        /// <returns>true if item was added, false if item already exists</returns>
        public static bool AddNonExisting(ICollection<TItem> list, TItem item, Predicate<TItem> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");

            bool bNotFound = !Exists(list, match);
            if (bNotFound)
            {
                list.Add(item);
            }

            return bNotFound;
        }


        /// <summary>
        /// Extreact item from list
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="pos">Item pos</param>
        /// <returns>Item removed from list</returns>
        public static TItem ExtractAt(List<TItem> list, int pos)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            TItem item = list[pos];
            list.RemoveAt(pos);
            return item;
        }


        public static TItem Extract(List<TItem> list, Predicate<TItem> match)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (match == null)
                throw new ArgumentNullException("match");

            int pos = list.FindIndex(match);
            return pos >= 0 ? ExtractAt(list, pos) : default(TItem);
        }

        public static IList<TItem> ExtractPage(IList list, int nPage, int nPageSize)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (nPageSize <= 0)
                throw new ArgumentOutOfRangeException("nPageSize", nPageSize, "Page size should be grater than zero");

            List<TItem> oList = new List<TItem>(list.Count);
            foreach (TItem oItem in list)
            {
                oList.Add(oItem);
            }
            int nOffset = nPage * nPageSize;
            int nDynPageSize = (nOffset + nPageSize) < oList.Count ? nPageSize : oList.Count - nOffset;
            return oList.GetRange(nOffset, nDynPageSize);
        }

        public static List<TItem> Sort(List<TItem> list, bool bOrder, Func<TItem, object> func)
        {
            return bOrder ? list.OrderBy(x => func(x)).ToList() : list.OrderByDescending(x => func(x)).ToList();
        }

        public static IList<TItem> ExtractPage(IList<TItem> list, int nPage, int nPageSize)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (nPageSize <= 0)
                throw new ArgumentOutOfRangeException("nPageSize", nPageSize, "Page size should be grater than zero");
            int nOffset = nPage * nPageSize;
            int nDynPageSize = (nOffset + nPageSize) < list.Count ? nPageSize : list.Count - nOffset;
            return (nDynPageSize > 0) ? list.ToList().GetRange(nOffset, nDynPageSize) : new List<TItem>();
        }

        public static IEnumerable<TItem> Generate(int count, Func<int, TItem> generator)
        {
            if (generator == null)
                throw new ArgumentNullException("generator");

            for (int i = 0; i < count; i++)
            {
                yield return generator(i);
            }
        }


        /// <summary>
        /// Convert source list to integer list
        /// </summary>
        /// <param name="srcList"></param>
        /// <returns></returns>
        public static List<int> ToIntList(IEnumerable<TItem> srcList)
        {
            List<int> result = new List<int>();
            foreach (TItem item in srcList)
            {
                result.Add(int.Parse(item.ToString()));
            }
            return result;
        }


        /// <summary>
        /// Convert source list to string list
        /// </summary>
        /// <param name="srcList"></param>
        /// <returns></returns>
        public static List<string> ToStringList(IEnumerable<TItem> srcList)
        {
            List<string> result = new List<string>();
            foreach (TItem item in srcList)
            {
                result.Add(item.ToString());
            }
            return result;
        }




        /// <summary>
        /// Join to String
        /// </summary>
        /// <param name="lst">List</param>
        /// <param name="delimiter">item delimiter</param>
        /// <returns>delimiter-separated string</returns>
        public static string Join(IEnumerable<TItem> lst, string delimiter)
        {
            StringBuilder sb = new StringBuilder();

            foreach (TItem entry in lst)
            {
                TextUtils.AddWithSeparator(sb, entry.ToString(), delimiter);
            }
            return sb.ToString();
        }


        /// <summary>
        /// Convert to an array
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static TItem[] ToArray(IEnumerable<TItem> lst)
        {
            List<TItem> tmpList = new List<TItem>();
            foreach (TItem entry in lst)
            {
                tmpList.Add(entry);
            }
            return tmpList.ToArray();
        }


        /// <summary>
        /// Convert array to List<>
        /// </summary>
        /// <param name="src">array</param>
        /// <returns></returns>
        public static List<TItem> ArrayToList(TItem[] src)
        {
            List<TItem> rslt = new List<TItem>();
            for (int i = 0; i < src.Length; i++)
                rslt.Add(src[i]);
            return rslt;
        }


        /// <summary>
        /// Gets a portion of the fullSet, which is not included into subSet
        /// Ideally suits for enums
        /// </summary>
        /// <param name="fullSet">complete array of items</param>
        /// <param name="subSet">selected items</param>
        /// <returns>members of fullSet that are non-entrant into the subSet</returns>
        public static List<TItem> Difference(List<TItem> fullSet, List<TItem> subSet)
        {
            List<TItem> rslt = new List<TItem>();
            for (int i = 0; i < fullSet.Count; i++)
            {
                if (!subSet.Contains(fullSet[i]))
                    rslt.Add(fullSet[i]);
            }
            return rslt;
        }

        public static List<TItem> CreateListByParams(params TItem[] lst)
        {
            List<TItem> result = new List<TItem>();
            foreach (TItem item in lst)
            {
                if (Equals(item, null)) continue;
                result.Add(item);
            }
            return result;
        }

        public static string ListToString(params TItem[] lst)
        {
            return ListToString(lst, ListOutputStyle.NumerableList);
        }


        public static string ListToString(IEnumerable<TItem> lst, ListOutputStyle style)
        {
            StringBuilder result = new StringBuilder();

            int i = 0;
            foreach (TItem item in lst)
            {
                string s = item as string;
                if (!Equals(item, null) && (s != string.Empty))
                {
                    switch (style)
                    {
                        case ListOutputStyle.NumerableList:
                            {
                                if (i > 0)
                                    result.Append("\n");
                                result.Append(string.Format("[{0}] - {1}", i, item));
                                i++;
                            }
                            break;
                        case ListOutputStyle.CommaSeparated:
                            {
                                if (i > 0)
                                    result.Append(COMMA_SEPARATOR);
                                result.Append(item);
                            }
                            break;
                    }
                }
            }
            return result.ToString();
        }

        public static string ListToString(IEnumerable<TItem> lst)
        {
            return ListToString(lst, ListOutputStyle.NumerableList);
        }

        /// <summary>
        /// Load data into dictionary
        /// </summary>
        /// <typeparam name="TId">ID type</typeparam>
        /// <param name="list">Source data</param>
        /// <param name="getObjectId"><see cref="Func{T1,TR}"/>Retreive object id delegate</param>
        /// <param name="destDic">Destination container</param>
        /// <returns>Destination container <paramref name="destDic"/></returns>
        public static IDictionary<TId, TItem> LoadToDictionaryBase<TId>(IEnumerable list, Func<TItem, TId> getObjectId,
            IDictionary<TId, TItem> destDic)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (getObjectId == null)
                throw new ArgumentNullException("getObjectId");
            if (destDic == null)
                throw new ArgumentNullException("destDic");

            foreach (TItem item in list)
            {
                destDic[getObjectId(item)] = item;
            }
            return destDic;
        }


        /// <summary>
        /// Load data to List
        /// </summary>
        /// <param name="src">Source data</param>
        /// <param name="dest">Destination list</param>
        /// <returns>Destination List</returns>
        public static ICollection<TItem> LoadToList(IEnumerable src, ICollection<TItem> dest)
        {
            if (src == null)
                throw new ArgumentNullException("src");
            if (dest == null)
                throw new ArgumentNullException("dest");

            foreach (TItem item in src)
            {
                dest.Add(item);
            }

            return dest;
        }


        /// <summary>
        /// Collection mapper
        /// </summary>
        /// <typeparam name="TDest">Destination type</typeparam>
        /// <param name="source">Source collection</param>
        /// <param name="mapper"><see cref="Func{TR,T1}"/> Object mapper</param>
        /// <returns>Destination collection</returns>
        public static IEnumerable<TDest> MapTo<TDest>(IEnumerable<TItem> source, Func<TItem, TDest> mapper)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (mapper == null)
                throw new ArgumentNullException("mapper");

            foreach (TItem item in source)
            {
                yield return mapper(item);
            }
        }


        /// <summary>
        /// Cast enumerable to specified type
        /// </summary>
        /// <param name="source">source data</param>
        /// <returns>IEnumerable with items cased to TItem</returns>
        /// <exception><see cref="InvalidCastException"/> if source type cannot be casted specified type </exception>
        public static IEnumerable<TItem> Cast(IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            foreach (TItem o in source)
            {
                yield return o;
            }
        }


        /// <summary>
        /// Retreive objects of specified type from enumerable
        /// </summary>
        /// <param name="source">source data</param>
        /// <returns>enumerable of object with specified type. If object cannot be converted, it is skipped</returns>
        public static IEnumerable<TItem> OfType(IEnumerable source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            foreach (object o in source)
            {
                if (o is TItem)
                    yield return (TItem)o;
            }
        }


        /// <summary>
        /// Таке first N items from collection
        /// </summary>
        /// <param name="source">Collection</param>
        /// <param name="num">Number of numbers to take</param>
        /// <returns>New collection</returns>
        public static IEnumerable<TItem> Take(IEnumerable<TItem> source, int num)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (num < 0)
                throw new ArgumentException("Cannot be negative", "num");

            int cnt = 0;
            foreach (TItem item in source)
            {
                if (cnt >= num)
                    yield break;
                cnt++;
                yield return item;
            }
        }


        /// <summary>
        /// Return first N items
        /// </summary>
        /// <param name="set">collection</param>
        /// <param name="count">number of items to return</param>
        /// <returns>First N items (if collections contains less than N items, 
        /// as many as possible number of items will be returned)</returns>
        public static IEnumerable FirstN(IEnumerable set, int count)
        {
            if (set == null)
                throw new ArgumentNullException("set");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, "Count must have positive value");

            int i = 0;
            foreach (object item in set)
            {
                if (i >= count)
                    yield break;
                i++;
                yield return item;
            }
        }
        /// <summary>
        /// Return last N items
        /// </summary>
        /// <param name="set">collection</param>
        /// <param name="count">number of items to return</param>
        /// <returns>Last N items (if collection contains less than N items, 
        /// as many as possible numbers will be returned)</returns>
        public static IList<TItem> LastN(IList<TItem> set, int count)
        {
            if (set == null)
                throw new ArgumentNullException("set");
            if (count < 0)
                throw new ArgumentOutOfRangeException("count", count, "Count must have positive value");

            IList<TItem> newList = new List<TItem>();

            for (int i = set.Count - 1; count != 0; i--)
            {
                newList.Insert(0, set[i]);
                count--;
            }

            return newList;
        }

        /// <summary>
        /// Check, if collection is empty
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty(IEnumerable list)
        {
            if (list == null) throw new ArgumentNullException("list");
            IEnumerator enumerator = list.GetEnumerator();
            return !enumerator.MoveNext();
        }


        public static string GetDelimitedValues(IEnumerable source, string sPropertyName)
        {
            return GetDelimitedValues(source, sPropertyName, ", ");
        }

        public static string GetDelimitedValues(IEnumerable source, string sPropertyName, string delimiter)
        {
            StringBuilder sb = new StringBuilder();

            PropertyInfo oPropertyInfo = typeof(TItem).GetProperty(sPropertyName);
            if (oPropertyInfo == null)
            {
                throw new Exception(string.Format("Type {0} does not contain property {1}.", typeof(TItem), sPropertyName));
            }

            foreach (object item in source)
            {
                TextUtils.AddWithSeparatorObj(sb, DataBinder.Eval(item, sPropertyName), delimiter);
            }

            return sb.ToString();
        }

        public static IEnumerable<TProperty> GetValues<TProperty>(IEnumerable<TItem> source, string sPropertyName)
        {
            PropertyInfo oPropertyInfo = typeof(TItem).GetProperty(sPropertyName);

            if (oPropertyInfo == null)
            {
                throw new Exception(string.Format("Type {0} does not contain property {1}.", typeof(TItem),
                    sPropertyName));
            }

            foreach (TItem item in source)
            {
                yield return (TProperty)oPropertyInfo.GetValue(item, null);
            }
        }


        /// <summary>
        /// Load to list
        /// </summary>
        /// <param name="set">collection</param>
        /// <returns>List</returns>
        public static IList<TItem> ToList(IEnumerable<TItem> set)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            IList<TItem> list = new List<TItem>(set);
            return list;
        }


        /// <summary>
        /// Convert to dictionary
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IDictionary<K, TItem> ToDictionary<K>(IEnumerable<KeyValuePair<K, TItem>> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            IDictionary<K, TItem> dic = new Dictionary<K, TItem>();
            return ToDictionary(source, dic);
        }


        /// <summary>
        /// Convert to dictionary
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <param name="source"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static IDictionary<K, TItem> ToDictionary<K>(IEnumerable<KeyValuePair<K, TItem>> source, IDictionary<K, TItem> dic)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (dic == null) throw new ArgumentNullException("dic");

            foreach (KeyValuePair<K, TItem> pair in source)
            {
                dic[pair.Key] = pair.Value;
            }
            return dic;
        }

        /// <summary>
        /// Return empty sequence
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<TItem> EmptySequence()
        {
            yield break;
        }

        /// <summary>
        /// Sort enumarable by specified property
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="oList"></param>
        /// <param name="sSortProperty"></param>
        /// <param name="bDescending"></param>
        /// <returns></returns>
        public static IEnumerable<T> Sort<T>(IEnumerable<T> oList, string sSortProperty, bool bDescending)
        {
            List<T> oListForSorting = new List<T>(oList);
            Sort(oListForSorting, sSortProperty, bDescending);
            return oListForSorting;
        }

        /// <summary>
        /// Sorts list by specified property using default comparer
        /// </summary>
        /// <typeparam name="T">Type of item in the list</typeparam>
        /// <param name="oList"></param>
        /// <param name="sSortProperty"></param>
        /// <param name="bDescending"></param>
        public static void Sort<T>(List<T> oList, string sSortProperty, bool bDescending)
        {
            Sort(oList, sSortProperty, bDescending, Comparer.Default);
        }

        /// <summary>
        /// Sorts list by specified property
        /// </summary>
        /// <typeparam name="T">Type of item in the list</typeparam>
        /// <param name="oList"></param>
        /// <param name="sSortProperty"></param>
        /// <param name="bDescending"></param>
        /// <param name="oComparer"></param>
        public static void Sort<T>(List<T> oList, string sSortProperty, bool bDescending, IComparer oComparer)
        {
            if (oList == null)
            {
                throw new ArgumentNullException("oList");
            }

            if (sSortProperty == null)
            {
                throw new ArgumentNullException("sSortProperty");
            }

            if (oList.Count > 0)
            {
                PropertyInfo pi = null;

                foreach (T oItem in oList)
                {
                    if (oItem is ValueType || oItem != null)
                    {
                        pi = oList[0].GetType().GetProperty(sSortProperty);

                        if (pi == null)
                        {
                            throw new Exception(string.Format("Property {0} can't be found in type {1}.", sSortProperty, typeof(T)));
                        }

                        break;
                    }
                }

                // if pi == null all items in list are nulls
                if (pi != null)
                {
                    oList.Sort(delegate(T o1, T o2)
                    {
                        object v1 = pi.GetValue(o1, null);
                        object v2 = pi.GetValue(o2, null);
                        int i = oComparer.Compare(v1, v2);
                        return bDescending ? -i : i;
                    });
                }

            }

        }

    }
}
