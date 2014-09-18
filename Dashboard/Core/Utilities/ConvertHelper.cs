using System;
using System.ComponentModel;
using System.Globalization;


namespace Core.Utilities
{
    /// <summary>
    /// Quarter of the year
    /// </summary>
    public enum QuarterOfYear
    {
        None = 0,
        [Description("Q1")]
        Q1,
        [Description("Q2")]
        Q2,
        [Description("Q3")]
        Q3,
        [Description("Q4")]
        Q4
    }

    public class ConvertHelper
    {
        /// <summary>
        /// Convert int to string, using invariant culture
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        /// <remarks>Keys are always formatted using InvariantCulture</remarks>
        static public string IntToKey(int? i)
        {
            return i.HasValue ? i.Value.ToString(CultureInfo.InvariantCulture) : string.Empty;
        }

        /// <summary>
        /// Convert key string to integer
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>int</returns>
        /// <remarks>Keys are always formatted using InvariantCulture</remarks>
        static public int? KeyToInt(string key)
        {
            key = key ?? string.Empty;
            if (key.Trim().Length <= 0)
                return null;
            return int.Parse(key, NumberStyles.Number, CultureInfo.InvariantCulture);
        }

        static public string ToString(object o)
        {
            return (o != null) ? o.ToString() : string.Empty;
        }

        static public string ToString(Nullable<int> i, string sFormat)
        {
            return i.HasValue ? i.Value.ToString(sFormat) : string.Empty;
        }

        static public string ToString(Nullable<short> i)
        {
            return i.HasValue ? i.Value.ToString() : string.Empty;
        }

        static public string ToString(Nullable<long> l, string sFormat)
        {
            return l.HasValue ? l.Value.ToString(sFormat) : string.Empty;
        }

        static public string ToString(Nullable<decimal> d, string sFormat)
        {
            return d.HasValue ? d.Value.ToString(sFormat) : string.Empty;
        }

        static public string ToString(Nullable<DateTime> dt, string sFormat)
        {
            return dt.HasValue ? dt.Value.ToString(sFormat) : string.Empty;
        }

        static public string ToString(Nullable<bool> b)
        {
            return b.HasValue ? b.Value.ToString() : string.Empty;
        }

        static public Int32 ToInt32(string str)
        {
            return ToInt32(str, -1);
        }

        static public Int32 ToInt32(string str, Int32 defaultValue)
        {
            int iRes;
            return int.TryParse(str, out iRes) ? iRes : defaultValue;
        }

        static public decimal? ToDecimal(string str)
        {
            decimal iRes;
            if(decimal.TryParse(str, out iRes))
            {
                return iRes;
            }
            else
            {
                return null;
            }
        }

        static public long? ToInt64(string str)
        {
            long iRes;
            if (long.TryParse(str, out iRes))
            {
                return iRes;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Convert to specified type
        /// </summary>
        /// <param name="destType">estination type</param>
        /// <param name="input">input value</param>
        /// <returns>value</returns>
        static public object ToType(Type destType, object input)
        {
            if (IsNullable(destType))
            {
                if (input == null)
                    return null;
                if (input is string && (input as string) == string.Empty)
                    return null;
                destType = destType.GetGenericArguments()[0];
            }

            if (destType == typeof(bool))
            {
                return Convert.ToBoolean(input);
            }
            if (destType == typeof(Int32))
            {
                return Convert.ToInt32(input);
            }
            if (destType == typeof(string))
            {
                return Convert.ToString(input);
            }
            if (destType == typeof(Int64))
            {
                return Convert.ToInt64(input);
            }
            if (destType == typeof(DateTime))
            {
                return Convert.ToDateTime(input);
            }

            throw new InvalidCastException(string.Format("Can not cast {0} to {1}",
                input, destType.FullName));
        }

        private static bool IsNullable(Type destType)
        {
            return (destType.IsGenericType && destType.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        static public string ToURL(string url)
        {
            if (!string.IsNullOrEmpty(url) && (url[0] != '/') && (url.IndexOf(":", StringComparison.Ordinal) == -1))
            {
                return string.Format("http://{0}", url);
            }
            else 
                return url;
        }

        /// <summary>
        /// Returns date representing quarter of year
        /// </summary>
        /// <param name="quarter">Quarter</param>
        /// <param name="year">Year</param>
        /// <param name="beginOfQuarter">begin / end of quarter</param>
        /// <returns></returns>
        static public DateTime QuarterToDate(QuarterOfYear quarter, int year, bool beginOfQuarter)
        {
            if (year < 0)
                throw new ArgumentOutOfRangeException("year", year, "Year cannot be negative");
            int day = 1;
            int month;
            switch(quarter)
            {
                case QuarterOfYear.Q1:
                    month = 1;
                    break;
                case QuarterOfYear.Q2:
                    month = 4;
                    break;
                case QuarterOfYear.Q3:
                    month = 7;
                    break;
                case QuarterOfYear.Q4:
                    month = 10;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("quarter");
            }

            DateTime res = new DateTime(year, month, day);
            if (!beginOfQuarter)
                res = res.AddMonths(3).AddDays(-1);
            return res;
        }

        /// <summary>
        /// Extracts QuarterOfYear by Month number
        /// </summary>
        /// <param name="month">Month number</param>
        /// <returns>QuarterOfYear</returns>
        public static QuarterOfYear GetQuarter(int month)
        {
            QuarterOfYear result = QuarterOfYear.None;
            if (month >= 10)
                result = QuarterOfYear.Q4;
            else if (month >= 7)
                result = QuarterOfYear.Q3;
            else if (month >= 4)
                result = QuarterOfYear.Q2;
            else if (month >= 1)
                result = QuarterOfYear.Q1;
            return result;
        }

        /// <summary>
        /// Check if passed quarter of passed year is available to display in currentDate.
        /// (Quarter can be displayed only if it already finished now. 
        /// Example: 2009 Q1 will be available for users only from 4/1/09
        /// </summary>
        /// <param name="year">year of quarter</param>
        /// <param name="quarter">quarter</param>
        /// <param name="currentDate">Current Date</param>
        /// <returns>is Quarter available</returns>
        public static bool IsQuarterAvailable(int year, QuarterOfYear quarter, DateTime currentDate)
        {
            int currentYear = currentDate.Year;
            QuarterOfYear currentQuarter = GetQuarter(currentDate.Month);
            if (year < currentYear)
                return true;
            if (year == currentYear && quarter < currentQuarter)
                return true;
            return false;
        }

        /// <summary>
        /// Compare Quarters (works like String.Compare())
        /// </summary>
        /// <param name="first">first Quarter</param>
        /// <param name="second">second Quarter</param>
        /// <returns>int</returns>
        public static int CompareQuarters(QuarterOfYear first, QuarterOfYear second)
        {
            if ((int)first > (int)second)
                return 1;
            if ((int)first < (int)second)
                return -1;
            return 0;
        }

        /// <summary>
        /// Counts Quarters in year
        /// </summary>
        /// <param name="quarter">quarter</param>
        /// <param name="fromStart">count from start of the year or from the end</param>
        /// <returns>number of Quarters</returns>
        public static int CountTotalQuarters(QuarterOfYear quarter, bool fromStart)
        {
            int result = 0;
            switch (quarter)
            {
                case QuarterOfYear.Q1: result = 1; break;
                case QuarterOfYear.Q2: result = 2; break;
                case QuarterOfYear.Q3: result = 3; break;
                case QuarterOfYear.Q4: result = 4; break;
            }
            if (!fromStart)
                result = 4 - result + 1;
            return result;
        }

        /// <summary>
        /// Convert to string suitable for sorting
        /// </summary>
        /// <param name="i">Integer</param>
        /// <returns>SortKey</returns>
        public static string IntToSortKey(int? i)
        {
            if (!i.HasValue)
                return string.Empty;
            UInt32 val = (uint)(i.Value - int.MinValue);
            return val.ToString("00000000000", CultureInfo.InvariantCulture);
        }

        public static int? DecimalToInt(decimal? value)
        {
            return value.HasValue ? (int) value : (int?) null;
        }
        public static long? DecimalToLong(decimal? value)
        {
            return value.HasValue ? (long)value : (long?)null;
        }

    }
}
