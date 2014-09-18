using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.Model
{
    public class EmailTypeLookup
    {
        public byte EmailTypeId { get; set; }
        public string EmailTypeCd { get; set; }
        public string EmailTypeDescription { get; set; }
        public Nullable<bool> Deleted_ind { get; set; }
    }

    public class SecurityQuestionLookup
    {
        public int SecurityQuestionID { get; set; }
        public string SecurityQuestionDescription { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<int> Deleted_Ind { get; set; }
    }
}
