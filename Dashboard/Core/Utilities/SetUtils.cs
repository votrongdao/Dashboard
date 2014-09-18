using System;
using System.Collections.Generic;


namespace DashboardSite.Core.Utilities
{
    /// <summary>
    /// Set utilities
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SetUtils<T>
        where T: IEquatable<T>, IComparable<T>
    {
        #region Public Methods

        /// <summary>
        /// Get min value
        /// </summary>
        /// <param name="set">Set</param>
        /// <param name="minValue">Initial Value</param>
        /// <returns>Min value or Initial Value if set is empty</returns>
        public static T Min(IEnumerable<T> set, T minValue)
        {
            if (set == null)
                throw new ArgumentNullException("set");

            foreach (T val in set)
            {
                if (val.CompareTo(minValue) < 0)
                    minValue = val;
            }
            return minValue;
        }


        /// <summary>
        /// Get max value
        /// </summary>
        /// <param name="set">Set</param>
        /// <param name="maxValue">Initial valie</param>
        /// <returns>Max Value or Initial value if set is empty</returns>
        public static T Max(IEnumerable<T> set, T maxValue)
        {
            if (set == null)
                throw new ArgumentNullException("set");
            foreach (T val in set)
            {
                if (val.CompareTo(maxValue)> 0)
                    maxValue = val;
            }
            return maxValue;
        }

        /// <summary>
        /// Return items from src, which exists in both src and dest
        /// </summary>
        /// <param name="src">source set</param>
        /// <param name="dest">destination set</param>
        /// <returns></returns>
        public static IEnumerable<T> Intersect(IEnumerable<T> src, IList<T> dest)
        {
            if (src == null)
                throw new ArgumentNullException("src");
            if (dest == null)
                throw new ArgumentNullException("dest");

            if (dest.Count == 0)
                yield break;

            foreach (T item in src)
            {
                if (dest.IndexOf(item) < 0)
                    continue;
                yield return item;
            }
        }

        /// <summary>
        /// Return items from src, which not exists in dest
        /// </summary>
        /// <param name="src">source set</param>
        /// <param name="dest">destination set</param>
        /// <returns></returns>
        public static IEnumerable<T> Except(IEnumerable<T> src, IList<T> dest)
        {
            if (src == null)
                throw new ArgumentNullException("src");
            if (dest == null)
                throw new ArgumentNullException("dest");

            foreach (T item in src)
            {
                if (dest.IndexOf(item) >= 0)
                    continue;
                yield return item;   
            }
        }

        #endregion Public Methods

        #region Protected and Override Methods

        #endregion Protected and Override Methods

        #region Private Methods

        #endregion Private Methods

   }
}