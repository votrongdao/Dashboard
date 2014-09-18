using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DashboardSite.Core.ExceptionHandling
{
    public abstract class ExceptionBase : Exception
    {
        ExceptionCategory m_ExceptionCategory = ExceptionCategory.UnhandledException;
        EventLogEntryType m_EventLogEntryType = EventLogEntryType.Error;

        public ExceptionBase(ExceptionCategory category, string message)
            : base(message)
        {
            m_ExceptionCategory = category;
        }

        public ExceptionBase(ExceptionCategory category, Exception innerException, string message)
            : base(message, innerException)
        {
            m_ExceptionCategory = category;
        }

        public ExceptionBase(ExceptionCategory category, Exception innerException, string format, params object[] values)
            : base (string.Format(format, values), innerException)
        {
            m_ExceptionCategory = category;
        }

        public override string Message
        {
            get { return base.Message; }
        }

        public string EventSoruce
        {
            get { return ConstantString.BillfoldAppEventSource; }
        }

        virtual public int CategoryId
        {
            get { return (int)m_ExceptionCategory; }
        }

        virtual public string CategoryName
        {
            get { return m_ExceptionCategory.ToString(); } 
        }

        virtual public EventLogEntryType EventlogType
        {
            get { return m_EventLogEntryType; }
            set { m_EventLogEntryType = value; }
        }

        virtual public string TroubleShootInstruction { get; set; }

        static protected string GetJson(DataRow r)
        {
            StringBuilder json = new StringBuilder();
            try
            {
                int index = 0;
                foreach (DataColumn item in r.Table.Columns)
                {
                    json.Append(String.Format("\"{0}\" : \"{1}\"",
                        item.ColumnName, (r[item.ColumnName] ?? string.Empty).ToString()));
                    if (index < r.Table.Columns.Count - 1)
                        json.Append(", ");
                    index++;
                }
            }
            catch (Exception ex)
            {
                json.Append(ex.Message);
            }
            return json.ToString();
        }

    }
}
