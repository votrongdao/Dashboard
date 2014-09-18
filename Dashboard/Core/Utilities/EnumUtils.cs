using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashboardSite.Core.Utilities
{
    public class EnumUtils
    {
        /// <summary>Safe Parse of the Enum value</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns>Corresponding to input string representation enum value, or Default enum value (0) if string contains invalid enum value</returns>
        public static T Parse<T>(string enumValue)
        {
            return Parse<T>(enumValue, default(T));
        }

        /// <summary>Safe Parse of the Enum value</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <param name="defaultValue">Default value to return if parsing fails</param>
        /// <returns>Corresponding to input string representation enum value, or defaultValue if string contains invalid enum value</returns>
        public static T Parse<T>(string enumValue, T defaultValue)
        {
            T enumResult = defaultValue;
            TryParse<T>(enumValue, ref enumResult);
            return enumResult;
        }

        /// <summary>Safe enum parsing from the string. String can contain either text (e.g. "User, SSO") or numeric (e.g. "3")
        /// representation of the enum</summary>
        /// <typeparam name="T">Type of the Enum</typeparam>
        /// <param name="enumValue">String representation of the enum</param>
        /// <param name="enumResult"></param>
        /// <returns></returns>
        public static bool TryParse<T>(string enumValue, ref T enumResult)
        {
            return TryParse<T>(enumValue, ref enumResult, true);
        }

        public static bool TryParse<T>(string enumValue, ref T enumResult, bool ignoreCase)
        {
            // text or numeric representation of the Enum value may fail with the exception, so supress the exception safely
            try   
            {
                enumResult = (T)Enum.Parse(typeof(T), enumValue, ignoreCase);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>The same as Enum.IsDefined(Type enumType, object enumValue)</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumValue"></param>
        /// <returns></returns>
        public static bool IsDefined<T>(object enumValue)
        {
            return Enum.IsDefined(typeof(T), enumValue);
        }

        #region Private members/funcs
        #endregion
    }
}
