using EntityObject.Entities.HCP_Customize;
using System;
using System.Linq;
using System.Text;

namespace EntityObject.Interface
{
    public interface IUploadedLoanVisitor
    {
        DerivedFinancial VisitorType { get; }
        void Visit(FundamentalFinancial fundamentalFin);
    }
}
