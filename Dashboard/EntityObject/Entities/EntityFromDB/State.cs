//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityObject.Entities.EntityFromDB
{
    using System;
    using System.Collections.Generic;
    
    public partial class State
    {
        public State()
        {
            this.Addresses = new HashSet<Address>();
        }
    
        public string StateCode { get; set; }
        public string StateName { get; set; }
    
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
