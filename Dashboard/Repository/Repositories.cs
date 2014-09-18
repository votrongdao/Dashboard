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
    // for simple repository classes, put in this file,
    // for repositories need override base class and add features
    // put in seperate files

    public class StatesRepository : BaseRepository<State>, IStatesRepository
    {
        public StatesRepository()
            : base(new UnitOfWork())
        {
        }
        public StatesRepository(UnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public List<KeyValuePair<string, string>> GetAllStates()
        {
            return this.GetAll().Select(p => new KeyValuePair<string, string>(p.StateCode, p.StateName)).ToList();
        }
    }
}
