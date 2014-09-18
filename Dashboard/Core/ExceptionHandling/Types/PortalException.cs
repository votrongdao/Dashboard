using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashboardSite.Core.ExceptionHandling
{
    public class PortalException : ExceptionBase
    {
        public PortalException(ExceptionCategory category, string message)
            : base(category, message)
        {
        }

        public PortalException(ExceptionCategory category, Exception innerException, string message)
            : base(category, innerException, message)
        {
        }

        public PortalException(ExceptionCategory category, Exception innerException, string format, params object[] values)
            : base(category, innerException, string.Format(format, values))
        {
        }
    }
}