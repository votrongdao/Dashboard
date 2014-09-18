using DashboardSite.Core;
using DashboardSite.Model;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using DashboardSite.Repository.Interface;
using EntityObject.Entities.EntityFromDB;

namespace DashboardSite.Repository
{
    public class EmailRepository : BaseRepository<Email>, IEmailRepository
    {
        public EmailRepository()
            : base(new UnitOfWork())
        {
        }
        public EmailRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public void SaveEmail(EmailModel email)
        {
            var emailEntity = Mapper.Map<Email>(email);
            AttachNew(emailEntity);
        }
    }
}