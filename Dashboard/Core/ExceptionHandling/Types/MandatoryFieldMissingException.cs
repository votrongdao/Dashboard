using System;
namespace Core.ExceptionHandling
{
    public class MandatoryFieldMissingException : ExceptionBase
    {
        const string MessageFormat = "Mandatory field [{0}] is missing from Class [{1}]. "
            + "Please fill in all mandatory fields if it is originated from user input form.";

        const string TroubleShootInstructionFormat = "This is most likely due to application bugs in our code. "
            + "we either have missing critical data from database, or allow user to submit a request without entering all mandatory fields. "
            + "Please review the stack trace to identify the missing field for the entity.";

        public MandatoryFieldMissingException(string entityTypeName, string entityFieldName)
            : base(ExceptionCategory.MandatoryFieldMissingError,
            string.Format(MessageFormat, entityFieldName, entityTypeName))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }
    }
}
