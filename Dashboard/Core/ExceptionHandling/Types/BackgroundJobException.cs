using System;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class BackgroundJobException : ExceptionBase
    {
        const string MessageFormat = "The exception is thrown from background job execution [JobId={0}]. " 
            + "Since it is executed from a background thread, user will not see the exception from UI. "
            + "However, this exception is critical, and we must investigate what goes wrong as soon as possible. "
            + "Additional message: [{1}]";
        const string TroubleShootInstructionFormat = "The detail job info is kept in dbCRM.tbl_bd_job_request table. "
            + "You can query the error (contains debug trace) through the jobId. "
            + "The error is stored in result_txt column as XML data (html encoded). "
            + "You can use an online HtmlDecode service (like http://www.string-functions.com/htmldecode.aspx) to decode the xml data for better reading. "
            + "When you review the trace info, pay attention to the last few lines, that is where the exception is thrown.";

        public BackgroundJobException(Exception innerException, int jobId)
            : base(ExceptionCategory.BackgroundJobError, innerException,
            string.Format(MessageFormat, jobId, string.Empty))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

        public BackgroundJobException(Exception innerException, int jobId, string additionalInfo)
            : base(ExceptionCategory.BackgroundJobError, innerException,
            string.Format(MessageFormat, jobId, additionalInfo))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

    }

}
