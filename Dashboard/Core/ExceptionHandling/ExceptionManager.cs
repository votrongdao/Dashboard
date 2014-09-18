using System;
using System.Web;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace DashboardSite.Core.ExceptionHandling
{
	public static class ExceptionManager
	{
        const int EVT_MAX_SIZE = 15000;
        private static List<string> m_IgnoreExceptionList = null;

        public static void WriteToEventLog(string message, string source, EventLogEntryType type)
        {
            string cs = source;
            EventLog elog = new EventLog();
            if (!EventLog.SourceExists(cs))
            {
                EventLog.CreateEventSource(cs, "Application");
            }
            elog.Source = cs;
            elog.EnableRaisingEvents = true;
            elog.WriteEntry(message, type);
            //EventLog.WriteEntry(cs, message, EventLogEntryType.Error);
        }

        /// <summary>
        /// HandleException
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        static public int HandleException(Exception ex)
        {
            return HandleException(ex, true);
        }

        /// <summary>
        /// SafeHandleException
        /// Rethrow exception in debug mode, otherwise publish it to eventvwr.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        static public int SafeHandleException(Exception ex)
        {
#if DEBUG
            throw ex;
#else
            return HandleException(ex, true);
#endif
        }

        /// <summary>
        /// HandleException
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="category"></param>
        /// <param name="publishToEventViewer"></param>
        /// <returns></returns>
        static public int HandleException(Exception ex, bool publishToErrorLog)
        {
            string sDetailExceptionMessage = ex.Message;
            InitFilterOutExceptionArray();
            int iErrorId = -1;
            try
            {
                //Log to eventvwr also if required.
                NameValueCollection addlInfo = PopulateAdditionalInfo(ex);
                sDetailExceptionMessage = GenerateDetailExeptionMessage(ex, addlInfo);

                //Don't log the error in the exclusion list
                if (CanIgnoreException(sDetailExceptionMessage))
                    return -1;

                //if (publishToErrorLog)
                //{
                //    //Save exception into ErrorLog table for future reference
                //    iErrorId = ExceptionDAC.SaveException(ex, sDetailExceptionMessage);
                //}
                PublishException(ex, sDetailExceptionMessage, iErrorId);


            }
            catch (Exception)
            {
                WriteToLog(ConstantString.BillfoldAppEventSource, "Exception thrown from HandleException. " + sDetailExceptionMessage, EventLogEntryType.Error, (int)ExceptionCategory.SystemConfigSettingError);
            }
            return iErrorId;
        }

        /// <summary>
        /// Gets exception's localization code.
        /// </summary>
        /// <param name="ex">Exception instance</param>
        /// <returns>Localization code if it exists otherwise empty string.</returns>
        public static string GetLocalizationCode(Exception ex)
        {
            return IsLocalized(ex) ? (string)ex.Data[ConstantString.LOCALIZATION_CODE_KEY] : string.Empty;
        }

        /// <summary>
        /// Gets exception's localization parameters.
        /// </summary>
        /// <param name="ex">Exception instance</param>
        /// <returns>Localization parameters if they exist otherwise empty array.</returns>
        public static object[] GetLocalizationParameters(Exception ex)
        {
            return hasParameters(ex) ? (object[])ex.Data[ConstantString.LOCALIZATION_PARAMS_KEY] : new object[0];
        }

        /// <summary>
        /// Checks if exception contains localization info
        /// </summary>
        /// <param name="ex">Exception instance</param>
        /// <returns></returns>
        public static bool IsLocalized(Exception ex)
        {
            return ex.Data.Contains(ConstantString.LOCALIZATION_CODE_KEY) && (ex.Data[ConstantString.LOCALIZATION_CODE_KEY] is string);
        }

        /// <summary>
        /// Adds localization info to Exception
        /// </summary>
        /// <param name="ex">Exception instance</param>
        /// <param name="sLocalizationKey">Localization Key string</param>
        /// <param name="oLocalizationParameters">Optional localization parameters</param>
        /// <returns>Filled original exception.</returns>
        public static Exception AddLocalizationInfo(Exception ex, string sLocalizationKey, params object[] oLocalizationParameters)
        {
            ex.Data[ConstantString.LOCALIZATION_CODE_KEY] = sLocalizationKey;
            ex.Data[ConstantString.LOCALIZATION_PARAMS_KEY] = oLocalizationParameters;
            return ex;
        }

        #region private methods
        private static NameValueCollection PopulateAdditionalInfo(Exception ex)
        {
            NameValueCollection additionalInfo = new NameValueCollection();

            if (ex.Data.Contains(ConstantString.ERROR_LOG_USERAGENT_KEY))
            {
                additionalInfo.Add(ConstantString.ERROR_LOG_USERAGENT_KEY, ex.Data[ConstantString.ERROR_LOG_USERAGENT_KEY].ToString());
            }

            // Add environment information to the information collection.
            try { additionalInfo.Add(ConstantString.EX_MachineName, Environment.MachineName); }
            catch (Exception) { }
            try { additionalInfo.Add(ConstantString.EX_TimeStamp, DateTime.Now.ToString()); }
            catch (Exception) { }
            try { additionalInfo.Add(ConstantString.EX_FullName, System.Reflection.Assembly.GetExecutingAssembly().FullName); }
            catch (Exception) { }
            try { additionalInfo.Add(ConstantString.EX_AppDomainName, AppDomain.CurrentDomain.FriendlyName); }
            catch (Exception) { }
            try { additionalInfo.Add(ConstantString.EX_ThreadIdentity, System.Threading.Thread.CurrentPrincipal.Identity.Name); }
            catch (Exception) { }
            try { additionalInfo.Add(ConstantString.EX_WindowsIdentity, System.Security.Principal.WindowsIdentity.GetCurrent().Name); }
            catch (Exception) { }

            //Record httpContext information if the exception is related to http request.
            try
            {
                if (HttpContext.Current != null)
                {
                    HttpContext ctx = HttpContext.Current;
                    if (ctx.Request != null)
                    {
                        additionalInfo.Add(ConstantString.EX_HttpRequestUrl, ctx.Request.Url.OriginalString);
                    }
                    if (ctx.Server != null)
                    {
                        additionalInfo.Add(ConstantString.EX_HttpRequestTimeout, ctx.Server.ScriptTimeout.ToString());
                    }

                }
            }
            catch (Exception) { }
            
            return additionalInfo;
        }


        static private string GenerateDetailExeptionMessage(Exception exception, NameValueCollection additionalInfo)
        {
            // Create StringBuilder to maintain publishing information.
            StringBuilder strInfo = new StringBuilder();
            if (exception == null)
            {
                strInfo.AppendFormat("{0}No Exception object has been provided.{0}", Environment.NewLine);
            }
            else
            {
                #region Record the contents of the AdditionalInfo collection
                // Record the contents of the AdditionalInfo collection.
                if (additionalInfo != null)
                {
                    // Record General information.
                    strInfo.AppendFormat("{0}{0}General Information {0}{1}{0}Additional Info:", Environment.NewLine, ConstantString.TEXT_SEPARATOR);
                    foreach (string i in additionalInfo)
                    {
                        strInfo.AppendFormat("{0}{1}: {2}", Environment.NewLine, i, additionalInfo.Get(i));
                    }
                }
                #endregion

                #region Loop through each exception class in the chain of exception objects
                // Loop through each exception class in the chain of exception objects.
                Exception currentException = exception;	// Temp variable to hold InnerException object during the loop.
                int intExceptionCount = 1;				// Count variable to track the number of exceptions in the chain.
                do
                {
                    // Write title information for the exception object.
                    strInfo.AppendFormat("{0}{0}{1}) Exception Information{0}{2}", Environment.NewLine, intExceptionCount.ToString(), ConstantString.TEXT_SEPARATOR);
                    strInfo.AppendFormat("{0}Exception Type: {1}", Environment.NewLine, currentException.GetType().FullName);

                    #region Loop through the public properties of the exception object and record their value
                    // Loop through the public properties of the exception object and record their value.
                    PropertyInfo[] aryPublicProperties = currentException.GetType().GetProperties();
                    foreach (PropertyInfo p in aryPublicProperties)
                    {
                        // Do not log information for the InnerException or StackTrace. This information is 
                        // captured later in the process.
                        if (p.Name != "InnerException" && p.Name != "StackTrace")
                        {
                            if (p.GetValue(currentException, null) == null)
                            {
                                strInfo.AppendFormat("{0}{1}: NULL", Environment.NewLine, p.Name);
                            }
                            else
                            {
                                strInfo.AppendFormat("{0}{1}: {2}", Environment.NewLine, p.Name, p.GetValue(currentException, null));
                            }
                        }
                    }
                    #endregion
                    #region Record the Exception StackTrace
                    // Record the StackTrace with separate label.
                    if (currentException.StackTrace != null)
                    {
                        strInfo.AppendFormat("{0}{0}StackTrace Information{0}{1}", Environment.NewLine, ConstantString.TEXT_SEPARATOR);
                        strInfo.AppendFormat("{0}{1}", Environment.NewLine, currentException.StackTrace);
                    }
                    #endregion

                    // Reset the temp exception object and iterate the counter.
                    currentException = currentException.InnerException;
                    intExceptionCount++;
                } while (currentException != null);
                #endregion
            }

            string sLastDBError = GetLastDBError();
            if (!string.IsNullOrEmpty(sLastDBError))
            {
                //truncate to first 10K char if DBError string is more than 10k since Eventvwr only support 32k. 
                if (sLastDBError.Length > 10000)
                    sLastDBError = sLastDBError.Substring(0, 10000);
                strInfo.AppendFormat("{0}{0}Last Database Call{0}{1}", Environment.NewLine, ConstantString.TEXT_SEPARATOR);
                strInfo.AppendFormat("{0}{1}", Environment.NewLine, sLastDBError);
            }

            return strInfo.ToString();
        }

        static private void PublishException(Exception exception, string sDetailLogMessage, int errorId)
        {
            string sEventSource = ConstantString.BillfoldAppEventSource;
            if (!EventLog.SourceExists(sEventSource))
            {
                throw new ApplicationException(string.Format("The system does not have the Eventsource setup properly. Please create EventSource [{0}] in the server",
                    sEventSource));
            }

            EventLogEntryType logType = EventLogEntryType.Error;
            string evtCategory = ExceptionCategory.UnhandledException.ToString();
            int eventId = (int)ExceptionCategory.UnhandledException;

            StringBuilder sb = new StringBuilder();
            ExceptionBase exBase = FindBillfoldInnerException(exception);
            if (exBase != null)
            {
                logType = exBase.EventlogType;
                eventId = exBase.CategoryId;
                evtCategory = exBase.CategoryName;
                string message = exBase.Message;
                if (message.Length > 500)
                    message = message.Substring(0, 500);
                sb.AppendLine(message);
            }

            //record detail exception
            if (sDetailLogMessage.Length > EVT_MAX_SIZE)
            {
                sDetailLogMessage = sDetailLogMessage.Substring(0, EVT_MAX_SIZE);
                sb.Append(sDetailLogMessage);
                sb.AppendFormat("{0}{1}{0}Message is truncated at 30k eventlog size. If the LastDatabaseCall is truncated, you can login to System Admin site, go to errorLog page, and query the full log by Error Id found in the General Information section. ",
                    Environment.NewLine, ConstantString.TEXT_SEPARATOR);
            }
            else
                sb.AppendLine(sDetailLogMessage);

            sb.AppendFormat("{0}{1}{0}Recorded Error ID(dbUser..tbl_bd_error_log.error_log_id): {2}{0}",
                Environment.NewLine, ConstantString.TEXT_SEPARATOR, errorId);

            // Write the entry to the event log.   
            WriteToLog(sEventSource, sb.ToString(), logType, eventId);

        }

        private static ExceptionBase FindBillfoldInnerException(Exception exception)
        {
            if (exception is ExceptionBase)
                return exception as ExceptionBase;

            Exception exInner = exception;
            while (exInner.InnerException != null)
            {
                exInner = exInner.InnerException;
                if (exInner is ExceptionBase)
                    return exInner as ExceptionBase;
            }
            return null;
        }
        /// <summary>
        /// Helper function to write an entry to the Event Log.
        /// </summary>
        /// <param name="entry">The entry to enter into the Event Log.</param>
        /// <param name="type">The EventLogEntryType to be used when the entry is logged to the Event Log.</param>
        static private void WriteToLog(string eventSrc, string logMessage, EventLogEntryType logType, int eventId)
        {
            try
            {
                string eventSrcToUse;
                if (!string.IsNullOrEmpty(eventSrc))
                    eventSrcToUse = eventSrc;
                else
                    eventSrcToUse = ConstantString.ApplicationLog;


                // Write the entry to the Event Log.
                EventLog.WriteEntry(eventSrcToUse, logMessage, logType, eventId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static bool hasParameters(Exception ex)
        {
            return ex.Data.Contains(ConstantString.LOCALIZATION_PARAMS_KEY) 
                && (ex.Data[ConstantString.LOCALIZATION_PARAMS_KEY] is object[]);
        }

        static private string GetLastDBError()
        {
            string sDBLastCall = System.Runtime.Remoting.Messaging.CallContext.GetData(ConstantString.DEBUG_DB_LAST_CALL) as string;
            return sDBLastCall ?? string.Empty;
        }
        private static void InitFilterOutExceptionArray()
        {
            if (m_IgnoreExceptionList == null)
            {
                m_IgnoreExceptionList = new List<string>();
                m_IgnoreExceptionList.Add("Invalid postback or callback argument");
                m_IgnoreExceptionList.Add("Thread was being aborted");
                m_IgnoreExceptionList.Add("Your page is expired");
            }
        }

        private static bool CanIgnoreException(string errorMessage)
        {
            bool canIgnore = false;
            foreach (string s in m_IgnoreExceptionList)
            {
                if (errorMessage.Contains(s))
                {
                    canIgnore = true;
                    break;
                }
            }
            return canIgnore;
        }
        #endregion
    }
}
