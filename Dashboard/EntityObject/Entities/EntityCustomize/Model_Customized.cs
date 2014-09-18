using EntityObject.Interface;
using DashboardSite.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntityObject.Entities.EntityFromDB
{
    public partial class Authentication : IAuditable
    { 
        // add denormalized field here (join results from other tables)
        public string RoleName { get; set; }
    }

    public partial class Address : IAuditable
    { }
    public partial class Email : IAuditable
    { }
}