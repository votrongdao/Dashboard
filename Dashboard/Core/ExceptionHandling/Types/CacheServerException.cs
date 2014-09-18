using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iPreo.Bigdough.ExceptionHandling
{
    public class CacheServerException : ExceptionBase
    {
        const string MessageFormat = "Failed to load data from or save data to cache server. Cache key = [{0}]. {1}";

        const string TroubleShootInstructionFormat = "This is a critical event! if the alerts keep coming, we need to recycle the offending process(such as Billfold Site AppPool) at server [{0}].";

        public CacheServerException(Exception innerException, string cacheKey, string message)
            : base(ExceptionCategory.CacheServerConnectionError, innerException,
            string.Format(MessageFormat, cacheKey, message))
        {
            base.TroubleShootInstruction = string.Format(TroubleShootInstructionFormat, System.Environment.MachineName);
        }
    }
}
