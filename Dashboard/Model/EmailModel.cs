using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashboardSite.Model
{
    public class EmailModel
    {
        public int EmailId { get; set; }
        public string EmailTo { get; set; }
        public string EmailFrom { get; set; }
        public string EmailCC { get; set; }
        public string EmailBCC { get; set; }
        public string Subject { get; set; }
        public string ContentText { get; set; }
        public string ContentHtml { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<int> OnBehalfOfBy { get; set; }
        public Nullable<bool> Deleted_Ind { get; set; }
        public Nullable<bool> IsSent { get; set; }
        public byte MailTypeId { get; set; }
    }
}
