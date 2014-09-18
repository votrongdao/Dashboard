using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashboardSite.Core.ExceptionHandling
{
    public class ConstantString
    {
        public const string BillfoldAppEventSource = "Billfold Application";
        
        public const string ApplicationLog = "Application";
        public const string DEBUG_DB_LAST_CALL = "DEBUG_DB_LAST_CALL";

        public const string EX_ErrorId = "ErrorId";
        public const string EX_LastDatabaseCall = "LastDatabaseCall";
        public const string EX_MachineName = "MachineName";
        public const string EX_TimeStamp = "TimeStamp";
        public const string EX_FullName = "FullName";
        public const string EX_AppDomainName = "AppDomainName";
        public const string EX_ThreadIdentity = "ThreadIdentity";
        public const string EX_WindowsIdentity = "WindowsIdentity";

        public const string EX_HttpRequestUrl = "HttpRequestUrl";
        public const string EX_HttpRequestTimeout = "HttpRequestTimeout";

        public const string LOCALIZATION_CODE_KEY = "CODE";
        public const string LOCALIZATION_PARAMS_KEY = "PARAMS";


        public const string ERROR_LOG_DETAIL_MESSAGE = "MachineName=[{0}]\r\nIdentityName=[{1}]\r\n\r\nStack Trace Info: [{2}]\r\nLast DB Call: [{3}]";
        public const string ERROR_LOG_INNER_DETAIL_MESSAGE = "\r\nInner Message:{0}\r\nStack Trace Info: [{1}]";

        public const string ERROR_LOG_USERAGENT_KEY = "UserAgent";

        public const string TEXT_SEPARATOR = "*********************************************";

    }
}
