using System;
using System.Collections.Generic;
using System.Collections;


namespace DashboardSite.Core.Utilities
{
    /// <summary>
    /// Convert list of objects to another type
    /// </summary>
    /// <typeparam name="TIn">Source type</typeparam>
    /// <typeparam name="TOut">destination type</typeparam>
    public static class ListConverter<TIn, TOut>
    {

        public delegate bool FilterDelegate(TIn src, out TOut dst);

        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="srcList">Source list</param>
        /// <param name="dstList">Destinatin list</param>
        /// <param name="converter">Converter delegate</param>
        /// <remarks>Converter dalegate should hande nulls correctly!</remarks>
        public static void Convert(IEnumerable<TIn> srcList, IList<TOut> dstList, Func<TIn, TOut> converter)
        {
            if (srcList == null)
                throw new ArgumentNullException("srcList");

            if (dstList == null)
                throw new ArgumentNullException("dstList");

            if (converter == null)
                throw new ArgumentNullException("converter");

            foreach (TOut destObj in Transform(srcList, converter))
            {
                dstList.Add(destObj);
            }
        }

        /// <summary>
        /// Convert from <typeparamref name="TIn"/> to <typeparamref name="TOut"/>
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="converter">converter</param>
        /// <returns>output</returns>
        public static IEnumerable<TOut> Transform(IEnumerable<TIn> input, Func<TIn, TOut> converter)
        {
            if (input == null)
                throw new ArgumentNullException("input");
            if (converter == null)
                throw new ArgumentNullException("converter");

            foreach (TIn src in input)
            {
                yield return converter(src);
            }
        }

        /// <summary>
        /// Convert
        /// </summary>
        /// <param name="srcList">Source list</param>
        /// <param name="dstList">Destinatin list</param>
        /// <param name="converter">Converter delegate</param>
        /// <remarks>Converter dalegate should hande nulls correctly!</remarks>
        public static void ConvertBase(IEnumerable srcList, IList dstList, Func<TIn, TOut> converter)
        {
            if (srcList == null)
                throw new ArgumentNullException("srcList");

            if (dstList == null)
                throw new ArgumentNullException("dstList");

            if (converter == null)
                throw new ArgumentNullException("converter");

            foreach (TIn srcObj in srcList)
            {
                TOut destObj = converter(srcObj);
                if(destObj!=null)
                    dstList.Add(destObj);
            }
        }

        /// <summary>
        /// Filter and convert
        /// </summary>
        /// <param name="collection">Input collection</param>
        /// <param name="filter">Filter and converter</param>
        /// <returns>Converted list</returns>
        public static List<TOut> Filter(IEnumerable collection, FilterDelegate filter)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            if (filter == null)
                throw new ArgumentNullException("filter");

            List<TOut> result = new List<TOut>();

            foreach (TIn oSrc in collection)
            {
                TOut oDst;
                if (filter(oSrc, out oDst))
                {
                    result.Add(oDst);
                }
            }
            return result;
        }
    }
}
