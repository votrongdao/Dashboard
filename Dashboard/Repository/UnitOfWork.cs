using DashboardSite.Core;
using DashboardSite.Repository.Interface;
using EntityObject.Entities.EntityFromDB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.Repository
{
    //public class UnitOfWork : IUnitOfWork
    public class UnitOfWork : IDisposable
    {
        private DbContext _context;
        public UnitOfWork()
        {
            _context = new DashboardEntities();
            //this.Context.ContextOptions.LazyLoadingEnabled = true;
        }

        internal DbContext Context
        {
            get { return _context; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
