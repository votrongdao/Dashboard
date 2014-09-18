using System;
using System.Data;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class EntitlementCheckException : ExceptionBase
    {
        const string MessageFormat = "Entitlement Check Failure - User does not have entitlement to the data requested. "
            + "\r\n"
            + "Please send the alert to development team for further investigation. "
            + "User Company Id=[{0}]; Data Company Id=[{1}]; EntityType=[{2}]. This error is unexpected. ";

        const string TroubleShootInstructionFormat = "Entitlement Check Failure is most likely due to application bugs "
            + "in our stored procedure to return OTHER company data to current user, "
            + "or there are data discrepency in our database table. "
            + "The following are steps to trouble shoot entitlement error. \r\n" 
            + "1) If you see User Company Id = -1, it means there is no user context, or user session is expired. If this is the case, you can ignore this exception. "
            + "\r\n"
            + "2) If you see Data Company Id = -1, it means the stored procedure doesn't return the data company_id fields. "
            + "You should review the stored procedure code to make sure a valid company_id returned."
            + "\r\n"
            + "3) If you see [User Company Id] and [Data Company Id] is not matched. Run the last DB stored procedure "
            + "that you can find inside the exception body, compare the company_id returned, with the user container company_id. "
            + "You should find they are not match. Please work with data team to figure out why such mismatch occurs." 
            + "\r\n\r\n"
            + "We should figure out why such mis-match occurse, and fix our data or stored procedure codes accordingly. "
            + "The following is the entity data failed entitlement check. "
            + "\r\n"
            + "[{0}]";

        public EntitlementCheckException(int userCompanyId, int dataCompanyId, string dataType, DataRow dataRow)
            : base(ExceptionCategory.EntitlementCheckError,
            string.Format(MessageFormat, userCompanyId, dataCompanyId, dataType))
        {
            base.TroubleShootInstruction = string.Format(TroubleShootInstructionFormat, GetJson(dataRow));
        }
    }
}
