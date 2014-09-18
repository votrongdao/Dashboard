using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class ViewStateException : ExceptionBase
    {
        const string MessageFormat = "Your page is expired, please browse to the page and submit again. [{0}]";

        const string TroubleShootInstructionFormat = "We don't need to trouble shoot this exception. The user viewstate is expired after viewstate cache expiration (8 hours), or session expiration (8 hours). "
            + "However, if this exception keeps coming, please escalate to development team for investigation.";

        public ViewStateException(string pageUrl)
            : base(ExceptionCategory.ViewStateError, string.Format(MessageFormat, pageUrl))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

        public ViewStateException(Exception innerException, string pageUrl)
            : base(ExceptionCategory.ViewStateError, innerException,
            string.Format(MessageFormat, pageUrl))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }
    }
}
