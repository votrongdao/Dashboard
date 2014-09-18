using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class BillfoldException : ExceptionBase
    {
        public BillfoldException(ExceptionCategory category, string message)
            : base(category, message)
        {
        }

        public BillfoldException(ExceptionCategory category, Exception innerException, string message)
            : base(category, innerException, message)
        {
        }

        public BillfoldException(ExceptionCategory category, Exception innerException, string format, params object[] values)
            : base(category, innerException, string.Format(format, values))
        {
        }
    }
}
