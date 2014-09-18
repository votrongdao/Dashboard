using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.Core.Interface
{
    interface IAuditable
    {
        Nullable<int> ModifiedBy { get; set; }
        Nullable<int> OnBehalfOfBy { get; set; }
        Nullable<DateTime> ModifiedOn { get; set; }
    }
}
