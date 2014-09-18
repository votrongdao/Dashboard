using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data;
using EntityObject.Interface;
using DashboardSite.Core;

namespace EntityObject.Entities.EntityFromDB
{
    namespace EntityObject.Entities.EntityFromDB
    {
        public partial class DashboardEntities : DbContext
        {
            private readonly int ANONYMOUSE_USER_ID = -1;
            public override int SaveChanges()
            {
                // date time all use Utc time
                var utcNowAuditDate = DateTime.UtcNow;
                var changeSet = ChangeTracker.Entries<IAuditable>();
                if (changeSet != null)
                    foreach (DbEntityEntry<IAuditable> dbEntityEntry in changeSet)
                    {

                        switch (dbEntityEntry.State)
                        {
                            case System.Data.Entity.EntityState.Added:
                            case System.Data.Entity.EntityState.Modified:
                                dbEntityEntry.Entity.ModifiedOn = utcNowAuditDate;
                                dbEntityEntry.Entity.ModifiedBy = UserPrincipal.Current.UserData != null ? UserPrincipal.Current.UserId : ANONYMOUSE_USER_ID;
                                break;
                        }
                    }

                return base.SaveChanges();
            }
        }
    }
}