using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.Model
{
    public class UserInfoModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> LenderID { get; set; }
        public Nullable<int> ServicerID { get; set; }
        public int UserID { get; set; }
    }
}
