using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class EntityNotFoundException : ExceptionBase
    {
        const string MessageFormat = "Entity Data [{0}](id=[{1}]) is not found, or has been deleted from Database. "
            + "Please check \r\n"
            + "1) if user's account/contact list has an obsolete record which was deleted from the system; "
            + "2) if user bookmark an URL with an deleted record in the URL query string, when the user goes to the URL again, "
            + "the application may fail to load such deleted record.";

        const string TroubleShootInstructionFormat = "The application is try to load data from the database, "
            + "but database stored procedure returns empty dataset to business layer, which generates this exception."
            + "To trouble shoot, you can exceute the stored procedure from the last database call that you can find inside the exception body. "
            + "Then investigate why the data is not returned back from calling stored procedure. ";

        public EntityNotFoundException(string sEntityName, string sEntityID)
            : base(ExceptionCategory.EntityNotFoundError, string.Format(MessageFormat, sEntityName, sEntityID))
        {
            base.TroubleShootInstruction = TroubleShootInstructionFormat;
        }
    }
}
