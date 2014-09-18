using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Xml;
using System.Linq;
using System.Data.SqlClient;

namespace DashboardSite.Core
{
    [Serializable]
    public class UserData
    {
        #region Private Data
        private DateTime m_dtCache = DateTime.UtcNow;
        private int m_iUserId;
        private string m_sUserName;
        private string m_sUserRole;
        private string m_sFirstName;
        private string m_sLastName;
        private string m_sTimezone;
        #endregion Private Data

        #region Public Properties

        public int UserId
        {
            get { return m_iUserId; }
            protected set { m_iUserId = value; }
        }

        public string UserName
        {
            get { return m_sUserName; }
            protected set { m_sUserName = value; }
        }

        public string FirstName
        {
            get { return m_sFirstName; }
            protected set { m_sFirstName = value; }
        }

        public string LastName
        {
            get { return m_sLastName; }
            protected set { m_sLastName = value; }
        }

        public string Timezone
        {
            get { return m_sTimezone; }
            protected set { m_sTimezone = value; }
        }

        public string UserRole
        {
            get { return m_sUserRole; }
            protected set { m_sUserRole = value; }
        }
        #endregion Public Properties

        public UserData(string sLogin)
        {
            Initialize(sLogin);
        }

        public virtual void Initialize(string sUserName)
        {
            m_sUserName = sUserName;
            LoadUserData(sUserName);
        }

        /// <summary>
        /// check to see if the user data is expired from cache by comparing the cache time
        /// and the current time.
        /// </summary>
        /// <returns></returns>
        public virtual bool IsExpireFromCache()
        {
            bool bIsExpire = false;
            int iTTLSeconds = 60; //default to 60 seconds
            if (ConfigurationManager.AppSettings["UserPrincipalDataTTL"] != null)
                iTTLSeconds = XmlConvert.ToInt32(ConfigurationManager.AppSettings["UserPrincipalDataTTL"]);

            DateTime dtNow = DateTime.UtcNow;
            TimeSpan timeSpan = dtNow.Subtract(m_dtCache);
            if (timeSpan.TotalSeconds > iTTLSeconds)
                bIsExpire = true;
            return bIsExpire;
        }

        #region Protected Methods

        protected static List<TVal> GetListByKey<TKey, TVal>(IDictionary<TKey, List<TVal>> dict, TKey key)
        {
            List<TVal> list;
            dict.TryGetValue(key, out list);
            if (list == null)
            {
                list = new List<TVal>();
                dict.Add(key, list);
            }
            return list;
        }

        protected static void AddToListByKey<TKey, TVal>(IDictionary<TKey, List<TVal>> dict, TKey key, TVal val)
        {
            List<TVal> list = GetListByKey(dict, key);
            if (!list.Contains(val))
                list.Add(val);
        }

        #endregion Protected Methods

        #region Private Methods

        private void LoadUserData(string sUserName)
        {
            Debugger.OutputDebugString("Load UserData from database. User=[{0}]", sUserName);
            InitState();
            if (sUserName == UserPrincipal.ANONYMOUS_USER_NAME)
                return;

            try
            {
                // want to put UserData in Core but don't want core to reference entity framework context
                // instead of using  context to call sp, use sql client instead, so we have better layered structure
                string sConnection_Dashboard_db = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(sConnection_Dashboard_db))
                {
                    SqlCommand command = new SqlCommand();
                    //int iUserID = UserPrincipal.Current.UserId;
                    conn.Open();

                    //command1.CommandType = CommandType.Text;
                    command.Connection = conn;
                    command.CommandText = "usp_AuthenticateInitial_LogIn";
                    command.CommandType = CommandType.StoredProcedure;

                    // Add the input parameter and set its properties.
                    SqlParameter parameter = new SqlParameter();

                    //Lender ID
                    parameter.ParameterName = "@UserName";
                    parameter.SqlDbType = SqlDbType.VarChar;
                    parameter.Direction = ParameterDirection.Input;

                    //Add the parameter to the Parameters collection - User Name.
                    command.Parameters.AddWithValue("UserName", sUserName);
                    //command.ExecuteNonQuery();

                    using (SqlDataReader rdr = command.ExecuteReader(CommandBehavior.CloseConnection)) 
                    {
                        rdr.Read();
                        m_iUserId = rdr.GetInt32(rdr.GetOrdinal("UserID"));
                        m_sUserName = GetNullableSqlString(rdr, rdr.GetOrdinal("UserName"));
                        m_sUserRole = GetNullableSqlString(rdr, rdr.GetOrdinal("RoleName"));
                        m_sFirstName = GetNullableSqlString(rdr, rdr.GetOrdinal("FirstName"));
                        m_sLastName = GetNullableSqlString(rdr, rdr.GetOrdinal("LastName"));
                        m_sTimezone = GetNullableSqlString(rdr, rdr.GetOrdinal("PreferredTimeZone"));
                        rdr.Close(); 
                    } 
                    
                }

            }
            catch (Exception ex)
            {
              throw new InvalidOperationException(string.Format("User name: {0} cannot load user data", sUserName));
            }
        }

        private void InitState()
        {
            //reset the cache loading time
            m_dtCache = DateTime.UtcNow;
            //m_iCompanyId = int.MinValue;
            m_iUserId = int.MinValue;
        }

        private int? GetNullableSqlInt(SqlDataReader dataReader, int fieldIndex)
        {
            object value = dataReader.GetValue(fieldIndex);
            return value is DBNull ? (int?)null : dataReader.GetInt32(fieldIndex);
        }

        private string GetNullableSqlString(SqlDataReader dataReader, int fieldIndex)
        {
            object value = dataReader.GetValue(fieldIndex);
            return value is DBNull ? null : dataReader.GetString(fieldIndex);
        }
        
        #endregion Private Methods
    } 

    #region UserDataCache class

    internal class UserDataCache
    {
        private static readonly Hashtable m_oUserDataCache = new Hashtable();

        public static bool Contains(string sUserName)
        {
            return m_oUserDataCache.Contains(sUserName);
        }

        public static UserData GetUserData(string sUserName)
        {
            return m_oUserDataCache[sUserName] as UserData;
        }

        public static void AddUserData(string sUserName, UserData oUserData)
        {
            lock (m_oUserDataCache)
            {
                m_oUserDataCache[sUserName] = oUserData;
            }
        }

        public static void RemoveFromCache(string name)
        {
            lock (m_oUserDataCache)
            {
                m_oUserDataCache.Remove(name);
            }
        }
    }

    #endregion UserDataCache class
}
