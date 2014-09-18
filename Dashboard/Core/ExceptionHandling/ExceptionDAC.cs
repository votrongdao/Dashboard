using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Win32;
using System.Xml.XPath;
using System.IO;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class ExceptionDAC
    {
        static public int SaveException(Exception ex, string sDetailMessage)
        {
            ErrorLog errLog = new ErrorLog();
            errLog.error_dttm = System.DateTime.UtcNow;
            errLog.user_name = System.Threading.Thread.CurrentPrincipal.Identity.Name;
            errLog.error_message = ex.Message;
            errLog.parameters = sDetailMessage;
            errLog.object_nm = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
            errLog.error_severity = (int)System.Diagnostics.EventLogEntryType.Error;
            if (ex is ExceptionBase)
            {
                ExceptionBase eb = ex as ExceptionBase;
                errLog.error_num = eb.CategoryId;
                errLog.error_severity = (int)eb.EventlogType;
            }
            string errXml = errLog.ToXml();

            int errorId = SaveDb(ConstantString.DBErrorLogStoredProcedure, errLog.user_name, errXml);
            return errorId;
        }
        #region private methods
        static private string m_dbUserConnectionString;
        static private string dbUserConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(m_dbUserConnectionString))
                {
                    string sDbPoliciesPath = GetDbPolicyPath();
                    string sPolicyName = ConstantString.DBCustomerConnectorPolicyName;
                    if (string.IsNullOrEmpty(sDbPoliciesPath) || string.IsNullOrEmpty(sPolicyName))
                        return string.Empty;
                    XPathDocument xpdPolicies = new XPathDocument(sDbPoliciesPath);
                    XPathNavigator navigator = xpdPolicies.CreateNavigator();
                    string xpath = string.Format("/phxDatabasePolicies/policy[@name='{0}']/connections/connection[1]", sPolicyName);
                    XPathNodeIterator iterator = navigator.Select(xpath);
                    iterator.MoveNext();
                    m_dbUserConnectionString = iterator.Current.InnerXml;
                }
                return m_dbUserConnectionString;
            }
        }

        static private string GetDbPolicyPath()
        {
            string rslt = string.Empty;
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\i-Deal\Configuration");
            try
            {
                if (key == null)
                {
                    return rslt;
                }
                rslt = key.GetValue("") as string;
                key.Close();
                if (string.IsNullOrEmpty(rslt))
                {
                    return rslt;
                }
                rslt = Path.Combine(rslt, "dbpolicy.xml");
            }
            finally
            {
                if (key != null)
                {
                    ((IDisposable)key).Dispose();
                }
            }
            return rslt;
        }

        static private int SaveDb(string spName, string userName, string errXml)
        {
            int idVal = -1;
            using (SqlConnection conn = new SqlConnection(dbUserConnectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = spName;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pUserName", userName);
                    cmd.Parameters.AddWithValue("@pImpUser", string.Empty);
                    cmd.Parameters.AddWithValue("@pXml", errXml);

                    SqlParameter idOutParam = new SqlParameter("@pErrorLogId", SqlDbType.Int);
                    idOutParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(idOutParam);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    if (idOutParam.Value != null)
                    {
                        int.TryParse(idOutParam.Value.ToString(), out idVal);
                    }
                }
            }
            return idVal;
        }

        #endregion private methods

        [XmlRoot(ElementName = "ErrorLog", IsNullable = false), Serializable]
        public class ErrorLog
        {
            public int error_log_id { get; set; }
            public int? usr_id { get; set; }
            public string user_name { get; set; }
            public int? company_id { get; set; }
            public string object_nm { get; set; }
            public int? error_num { get; set; }
            public int? error_severity { get; set; }
            public int? error_state { get; set; }
            public string error_message { get; set; }
            public string parameters { get; set; }
            public string company_nm { get; set; }

            public DateTime _error_dttm = DateTime.UtcNow;
            public DateTime error_dttm
            {
                get { return DateTime.SpecifyKind(DateTime.Parse(_error_dttm.ToString("u")), DateTimeKind.Utc); }
                set { _error_dttm = value; }
            }

            public string ToXml()
            {
                StringBuilder sb = new StringBuilder(2048);
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = false;
                settings.OmitXmlDeclaration = true;
                using (XmlWriter xw = XmlWriter.Create(sb, settings))
                {
                    XmlSerializer xs = new XmlSerializer(GetType());
                    xs.Serialize(xw, this);
                }
                return sb.ToString();
            }
        }
    }
}
