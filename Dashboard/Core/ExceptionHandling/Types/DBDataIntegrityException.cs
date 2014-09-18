using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class DBDataIntegrityException : ExceptionBase 
    {
        const string MessageFormat = "Entity Data [{0}](id=[{1}]) has integrity issue in our database. "
            + "The data is either not found or duplicated. This exception is logged in silent mode without intervening user's workflow. "
            + "However, dev and data team must investigate this data integrity issue as soon as possible. Additional message: [{2}]";

        const string TroubleShootInstructionFormat = "The application load data from the database, but there is data integrety issue on the data returned."
            + "Either the data is not returned properly from stored procedure, or the data indeed has integrity issue in the tables. "
            + "To trouble shoot, you can exceute the stored procedure from the last database call (you can find inside the exception body), "
            + "then investigate the returned data to see what is wrong. "
            + "\r\n"
            + "This exception is not intrusive to user experience. User won't see this exception from UI at all. "
            + "It is logged for our internal data investigation.";

        public DBDataIntegrityException(string entityName, string entityID, string additionalMessage)
            : base(ExceptionCategory.DBDataIntegrityError, 
            string.Format(MessageFormat, entityName, entityID, additionalMessage))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }

    }
}
