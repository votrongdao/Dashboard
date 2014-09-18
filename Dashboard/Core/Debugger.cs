using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace DashboardSite.Core
{
    public class Debugger
    {
        const int CALL_CONTEXT_DEBUG_STRING_THREADHOLD = 5000;
        const string DEBUG_MODE = "DEBUG_MODE";
        const string DEBUG_OUTPUT = "DEBUG_OUTPUT";
        const string DEBUG_START_TIME = "DEBUG_START_TIME";
        const string DEBUG_DB_CALL_COUNTER = "DEBUG_DB_CALL_COUNTER";
        const string DEBUG_DB_LAST_CALL = "DEBUG_DB_LAST_CALL";

        static public bool DebugMode
        {
            get
            {
                bool bDebugMode = false;
                object oDebugMode = CallContext.GetData(Debugger.DEBUG_MODE);
                if (oDebugMode != null)
                    bDebugMode = (bool)oDebugMode;
                return bDebugMode;
            }
            set
            {
                CallContext.SetData(Debugger.DEBUG_MODE, value);
            }
        }

        static public StringCollection GetDebugStringCollection()
        {
            List<KeyValuePair<TimeSpan, string>> oDebugData = GetDebugData();

            StringCollection oStringCollection = new StringCollection();

            foreach (KeyValuePair<TimeSpan, string> oItem in oDebugData)
            {
                oStringCollection.Add(GetDebugEntryString(oItem));
            }

            return oStringCollection;
        }

        static public void AddDebugData(List<KeyValuePair<TimeSpan, string>> oDebugData)
        {
            List<KeyValuePair<TimeSpan, string>> oExistingDebugData = GetDebugData();
            oExistingDebugData.AddRange(oDebugData);
        }

        static private string GetDebugEntryString(KeyValuePair<TimeSpan, string> oEntry)
        {
            return string.Format("[{0}] -- {1}", oEntry.Key, oEntry.Value ?? string.Empty);
        }

        static public List<KeyValuePair<TimeSpan, string>> GetDebugData()
        {
            List<KeyValuePair<TimeSpan, string>> oDebugData = CallContext.GetData(Debugger.DEBUG_OUTPUT) as List<KeyValuePair<TimeSpan, string>>;

            if (oDebugData == null)
            {
                oDebugData = new List<KeyValuePair<TimeSpan, string>>();
                CallContext.SetData(Debugger.DEBUG_OUTPUT, oDebugData);
                CallContext.SetData(Debugger.DEBUG_START_TIME, System.DateTime.Now);
            }

            return oDebugData;
        }

        static public void OutputDebugString(string sFormat, params object[] args)
        {
            if (!DebugMode)
                return;
            string sDebugString = string.Format(sFormat, args);
            OutputDebugString(sDebugString);
        }

        static public void OutputDebugString(string sDebugString)
        {
            OutputDebugString(sDebugString, false);
        }

        static public void OutputDebugString(string sDebugString, bool bForceLogging)
        {
            if (!DebugMode && !bForceLogging)
                return;

            List<KeyValuePair<TimeSpan, string>> oDebugData = GetDebugData();

            DateTime dtStartTime = (DateTime)CallContext.GetData(Debugger.DEBUG_START_TIME);
            TimeSpan ts = System.DateTime.Now - dtStartTime;

            KeyValuePair<TimeSpan, string> oNewEntry = new KeyValuePair<TimeSpan, string>(ts, sDebugString);

            oDebugData.Add(oNewEntry);

            //avoid the debug string blow up if the call context is long lived, like inside window service.
            if (oDebugData.Count > CALL_CONTEXT_DEBUG_STRING_THREADHOLD)
                oDebugData.RemoveAt(0);
#if DEBUG
            if (DebugMode)
                System.Diagnostics.Debug.WriteLine(GetDebugEntryString(oNewEntry));
#endif
        }

//        /// <summary>
//        /// OutputDBQueryString - Log SQL call, always log the call so it can be reported if any exception happens.
//        /// </summary>
//        /// <param name="eDBInstance"></param>
//        /// <param name="sStoredProcedure"></param>
//        /// <param name="oArgs"></param>
//        static public void OutputDBQueryString(DatabaseConnector.DBInstance eDBInstance, string sStoredProcedure, Dictionary<string, object> oArgs)
//        {
//#if DEBUG
//            IncrementDBCallCounter();
//#endif
//            StringBuilder sb = new StringBuilder();
//            if (SqlHelper.UseReadOnlyDatabase)
//                sb.AppendLine("[READONLY MODE] ");
//            sb.AppendFormat("USE {0} EXEC {1} ", eDBInstance, sStoredProcedure);
//            foreach (KeyValuePair<string, object> oArg in oArgs)
//            {
//                string sParam = oArg.Key;
//                if (sParam == null)
//                    continue;

//                if (oArg.Value == null)
//                    sb.AppendFormat(" {0} = {1},", oArg.Key, "NULL");
//                else //only extract the first 5000 chars to avoid taking too much memory.
//                    sb.AppendFormat(oArg.Value is string || oArg.Value is DateTime || oArg.Value is Guid ? " {0} = '{1}'," : " {0} = {1},", oArg.Key, oArg.Value);
//            }
//            string sDBString = sb.ToString();
//            PersistDBLastCall(sDBString);
//            OutputDebugString(sDBString);
//        }

        static public int DBCallCount
        {
            get
            {
                int iCount = 0;
                object o = CallContext.GetData(DEBUG_DB_CALL_COUNTER);
                if (o == null)
                    return 0;
                int.TryParse(o.ToString(), out iCount);
                return iCount;
            }
        }

        static public string GetLastDBError()
        {
            string sDBLastCall = CallContext.GetData(DEBUG_DB_LAST_CALL) as string;
            return sDBLastCall ?? string.Empty;
        }

        #region Private Methods
        static private void IncrementDBCallCounter()
        {
            int iCount = 1;
            object o = CallContext.GetData(DEBUG_DB_CALL_COUNTER);
            if (o == null)
            {
                CallContext.SetData(DEBUG_DB_CALL_COUNTER, iCount);
                return;
            }
            bool bGood = int.TryParse(o.ToString(), out iCount);
            if (bGood)
            {
                iCount++;
            }
            CallContext.SetData(DEBUG_DB_CALL_COUNTER, iCount);
        }

        static private void PersistDBLastCall(string sLastCall)
        {
            CallContext.SetData(DEBUG_DB_LAST_CALL, sLastCall);
        }
        #endregion Private Methods

    }

}
