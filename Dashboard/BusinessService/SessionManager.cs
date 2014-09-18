using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace DashboardSite.BusinessService
{
    public class SessionHelper
    {
        public const string SESSION_KEY_NEW_ROLES = "new_roles_key";
        public const string SESSION_KEY_REMOVE_ROLES = "remove_roles_key";
        public const string SESSION_KEY_NEW_LENDERS = "new_lenders_key";
        public const string SESSION_KEY_LAR_FHA_LINKS = "lar_fha_link_key";
        #region Session helper methods

        /// <summary>
        /// Gets value in Session and removes it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T SessionExtract<T>(string key)
        {
            return SessionExtract<T>(key, default(T));
        }

        /// <summary>
        /// Gets value in Session and removes it, if there is no such a value or type missmatch, returns default value
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="key">Session key</param>
        /// <param name="defaultValue">Default return value</param>
        /// <returns></returns>
        public static T SessionExtract<T>(string key, T defaultValue)
        {
            try
            {
                return SessionGet(key, defaultValue);
            }
            finally
            {
                HttpContext.Current.Session.Remove(key);
            }
        }

        /// <summary>
        /// Gets value from session without removing
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="key">Session key</param>
        /// <returns></returns>
        public static T SessionGet<T>(string key)
        {
            return SessionGet(key, default(T));
        }

        /// <summary>
        /// Gets value from session without removing,  if there is no such a value or type missmatch, returns default value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T SessionGet<T>(string key, T defaultValue)
        {
            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    Object obj = HttpContext.Current.Session[key];
                    if (obj != null)
                    {
                        return (T) obj;
                    }
                }
                catch (Exception ex)
                {
                    throw new ApplicationException(string.Format("Cannot get \"{0}\" from session", key), ex);
                }
            }
            return defaultValue;
        }

        #endregion
    }
}
