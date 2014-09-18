using System;

namespace DashboardSite.Core.ExceptionHandling
{
    public class InvalidArgumentException : ExceptionBase
    {
        const string MessageFormat = "Invalid Argument [{0}] is passed into calling Method [{1}]. {2}. "
            + "This is unexpected. Please escalate to dev team for support.";
        const string TroubleShootInstructionFormat = "This is most likely due to application bugs in our code. "
            + "The exception is thrown from the calling method argument validation "
            + "to make sure all the fields required are passed in valid value. The passing data is either NULL/empty or invalid. "
            + "Please review the stack trace to identify the invalid field from the code base.";

        public InvalidArgumentException(string argumentName, string methodName)
            : base(ExceptionCategory.InvalidArgumentError,
            string.Format(MessageFormat, argumentName, methodName, "Null value"))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

        public InvalidArgumentException(string argumentName, string methodName, string additionalInfo)
            : base(ExceptionCategory.InvalidArgumentError,
            string.Format(MessageFormat, argumentName, methodName, additionalInfo))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

        public InvalidArgumentException(Exception innerException, string argumentName, string methodName, string additionalInfo)
            : base(ExceptionCategory.InvalidArgumentError, innerException,
            string.Format(MessageFormat, argumentName, methodName, additionalInfo))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

    }

}
