using DashboardSite.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.BusinessService
{
    public static class AutoMapperInitialize
    {
        public static void Initialize()
        {
            AutoMapperRepositoryConfig.Configure();
        }
    }
}
