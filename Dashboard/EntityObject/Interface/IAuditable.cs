using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityObject.Interface
{
    interface IAuditable
    {
        int ModifiedBy { get; set; }
        Nullable<int> OnBehalfOfBy { get; set; }
        DateTime ModifiedOn { get; set; } 
    }
}
