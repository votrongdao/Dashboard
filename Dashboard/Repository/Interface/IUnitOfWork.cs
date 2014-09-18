using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DashboardSite.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        void Save();
    }
}
