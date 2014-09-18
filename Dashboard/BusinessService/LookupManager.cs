using DashboardSite.BusinessService.Interface;
using DashboardSite.Model;
using DashboardSite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.BusinessService
{
    public class LookupManager : ILookupManager
    {
        public IEnumerable<SecurityQuestionLookup> GetAllSecurityQuestions()
        {
            var secQuestionRepo = new SecQuestionRepository();
            return secQuestionRepo.GetAllSecQuestions();
        }

        public IEnumerable<EmailTypeLookup> GetAllEmailTypes()
        {
            var emailTypeRepo = new EmailTypeRepository();
            return emailTypeRepo.GetAllEmailTypes();
        }
    }
}
