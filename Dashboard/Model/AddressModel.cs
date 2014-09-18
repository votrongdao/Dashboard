using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.Model
{
    public class AddressModel
    {
        public int AddressID { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string County_Code { get; set; }
        public string ZIP { get; set; }
        public string ZIP4_Code { get; set; }
        public string CountryId { get; set; }
        public string PhonePrimary { get; set; }
        public string PhoneAlternate { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        //public Nullable<int> AddressIdentifierID { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
    }
}
