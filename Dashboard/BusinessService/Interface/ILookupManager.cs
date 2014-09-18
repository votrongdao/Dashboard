using DashboardSite.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.BusinessService.Interface
{
    public interface ILookupManager
    {
        IEnumerable<SecurityQuestionLookup> GetAllSecurityQuestions();
        IEnumerable<EmailTypeLookup> GetAllEmailTypes();
    }
}
