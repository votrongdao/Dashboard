using Core;
using DashboardSite.Core;
using DashboardSite.Core.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DashboardSite.Model
{
    public class UpdateRoleModel
    {
        public IList<EnumType> RolesToUpdate { get; set; }
    }

    public class UserViewModel //: IValidatableObject
    {
        public IList<SelectListItem> AllRoles { get; set; }
        [Display(Name = "States")]
        public IList<SelectListItem> AllStates { get; set; }
        public UpdateRoleModel RoleUpdateModel { get; set; }
        public AddressModel AddressModel { get; set; }
        //[Required]
        [Display(Name = "Role")]
        public string SelectedRoles { get; set; }       // comma delimited roles
        [Required]
        [Display(Name = "Time Zone")]
        public string SelectedTimezone { get; set; }
        public string SelectedTimezoneId { get; set; }

        //[Required]
        [Display(Name = "User Type")]
        public string SelectedRoleSource { get; set; }

        public int? AddressID { get; set; }

        public bool? IsAccountLocked { get; set; }

        public bool? IsRegisterComplete { get; set; }

        public bool? Deleted_Ind { get; set; }

        public string PasswordHist { get; set; }

        public UserViewModel()
        {
            //AllRoles = new List<SelectListItem>();
            AllStates = new List<SelectListItem>();
            AddressModel = new AddressModel();
        }

        [Required]
        [EmailAddress(ErrorMessage = "Not a valid email address")]
        [DataType(DataType.EmailAddress)]        
        [Display(Name = "User Name")]
        
        public string UserName { get; set; }

        public int UserID { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        public string Title 
        {
            get { return AddressModel.Title; }
            set { AddressModel.Title = value; } 
        }

        [Required]
        public string Organization
        {
            get { return AddressModel.Organization; }
            set { AddressModel.Organization = value; }
        }

        [Required]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber
        {
            get { return AddressModel.PhonePrimary; }
            set { AddressModel.PhonePrimary = value; }
        }


        [Required]
        [Display(Name = "Address")]
        public string AddressLine1
        {
            get { return AddressModel.AddressLine1; }
            set { AddressModel.AddressLine1 = value; }
        }

        [Required]
        public string City
        {
            get { return AddressModel.City; }
            set { AddressModel.City = value; }
        }

        [Required]
        [Display(Name = "State")]
        public string SelectedStateCd
        {
            get { return AddressModel.StateCode; }
            set { AddressModel.StateCode = value; }
        }

        [Required]
        [Display(Name = "Zip Code")]
        [DataType(DataType.PostalCode)]
        public string ZipCode
        {
            get { return AddressModel.ZIP; }
            set { AddressModel.ZIP = value; }
        }

        [Required]
        [EmailAddress(ErrorMessage="Not a valid email address")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [System.ComponentModel.DataAnnotations.Compare("UserName", ErrorMessage = "User name must be the same as email address.")]
        public string EmailAddress
        {
            get { return AddressModel.Email; }
            set { AddressModel.Email = value; }
        }

       
        [RegularExpression(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[\[\]\^\$\.\|\?\*\+\(\)\\~`\!@#%&\-_+={}'""<>:;,]).{8,}$", ErrorMessage = "Password should be at least 8 characters and contains at least one upper case letter, one lower case letter, one number, one special character.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}