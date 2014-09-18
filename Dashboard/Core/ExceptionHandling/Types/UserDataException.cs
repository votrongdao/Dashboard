using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class UserDataException : ExceptionBase
    {
        const string MessageFormat = "User principal data [{0}]cannot be loaded from database. "
            + "This is critical error, since the user session is not initialized into a valide state. "
            + "It may cause other unexpected error from the application.";

        const string TroubleShootInstructionFormat = "You can call dbUser.dbo.prod_bd_GetUserData [{0}] stored procedure to review the returned user data, "
            + "then determine what data segment causes the loading error. If the data is nullable, please make sure the thrown function handles such nullable value.";

        public UserDataException(Exception innerException, string userName)
            : base (ExceptionCategory.UserDataError, innerException, 
            string.Format(MessageFormat, userName))
        {
            base.TroubleShootInstruction = string.Format(TroubleShootInstructionFormat, userName); 
        }
    }
}
