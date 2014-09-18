using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class SQLExcecuteException: ExceptionBase
    {
        const string MessageFormat = "SQL stored procedure call failed. DatabaseInstance=[{0}], StoredProcedureName=[{1}]. "
            + "Please send to dev team for investigation.";
        
        const string TroubleShootInstructionFormat = "The application is try to execute SQL stored procedure to a database, but database throws exception to business layer."
            + "To trouble shoot, you can exceute the failed stored procedure to find out why it fails. "
            + "The stored procedure call can be found in the Last Database Call section inside the exception body. "
            + "\r\n"
            + "If the error is related the database timeout (2 minutes), there may be heavy database job running during the time of failure. "
            + "Then we should see if we need to can fine tune the stored procedure, such as using (NOLOCK) hint whenever possible. "
            + "\r\n" 
            + "Please escalate to data team for DB request timeout exception if such exceptions floods in. it may be critical";

        public SQLExcecuteException(Exception innerException, string dbInstance, string spName)
            : base(ExceptionCategory.SQLExcecuteError, innerException,
            string.Format(MessageFormat, dbInstance, spName))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }
    }
}
