using AutoMapper;
using DashboardSite.Core;
using DashboardSite.Model;
using DashboardSite.Repository.Interface;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityObject.Entities.EntityFromDB;

namespace DashboardSite.Repository
{
    // put lookup tables here. static tables, such as roles, states etc
    // all should in the form of key value pair lists
    // will add cache feature for lookup table later
    public class RoleRepository : BaseRepository<webpages_Roles>
    {
        public RoleRepository()
            : base(new UnitOfWork())
        {
        }
        public RoleRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IEnumerable<KeyValuePair<int, string>> GetRoles()
        {
            // don't call ToList, it will cause linq to execute db call
            // usse IEnumberable to delay calls (so you can use results in other linq operations)
            return this.GetAll().Select(p => new KeyValuePair<int, string>(p.RoleId, p.RoleName));
        }
    }

    public class SecQuestionRepository : BaseLookupCache<SecurityQuestion>
    {
        public SecQuestionRepository()
            : base(new UnitOfWork())
        {
        }
        public SecQuestionRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override string IdColName
        {
            get
            {
                return "SecurityQuestionID";
            }
        }

        protected override string ValColName
        {
            get
            {
                return "SecurityQuestionDescription";
            }
        }

        public IEnumerable<SecurityQuestionLookup> GetAllSecQuestions()
        {
            var entityQuestions = this.GetLookupList();
            return Mapper.Map<IEnumerable<SecurityQuestionLookup>>(entityQuestions);
        }
    }

    public class EmailTypeRepository : BaseLookupCache<EmailType>
    {
        public EmailTypeRepository()
            : base(new UnitOfWork())
        {
        }
        public EmailTypeRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        protected override string IdColName
        {
            get
            {
                return "EmailTypeId";
            }
        }

        protected override string ValColName
        {
            get
            {
                return "EmailTypeId";
            }
        }

        protected override string CdColName
        {
            get
            {
                return "EmailTypeCd";
            }
        }

        public IEnumerable<EmailTypeLookup> GetAllEmailTypes()
        {
            var entityQuestions = this.GetLookupList();
            return Mapper.Map<IEnumerable<EmailTypeLookup>>(entityQuestions);
        }
    }
}
