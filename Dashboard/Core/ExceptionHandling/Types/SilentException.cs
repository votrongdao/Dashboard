using System;

namespace DashboardSite.Core.ExceptionHandling
{
    public class SilentException : ExceptionBase
    {
        const string MessageFormat = "The exception is unknown but not critical, we capture it here for trouble shooting investigation. "
            + "The exception is also not thrown to user so it won't break the user UI workflow in production mode. Additional message: [{0}]";

        const string TroubleShootInstructionFormat = "Usually this exception is logged as temporary solution since we don't know why the excepton is thrown. "
            + "Once we identify the root clause, we should replace this exception with better explained exception.";

        public SilentException(string message)
            : base(ExceptionCategory.SilentException,
                    string.Format(MessageFormat, message))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

        public SilentException(Exception innerException)
            : base(ExceptionCategory.SilentException, innerException,
                    string.Format(MessageFormat, string.Empty))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

        public SilentException(Exception innerException, string additionalInfo)
            : base(ExceptionCategory.SilentException, innerException,
            string.Format(MessageFormat, additionalInfo))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

    }

}
