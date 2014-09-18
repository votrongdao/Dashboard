using EntityObject.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace EntityObject.Entities.HCP_Customize
{
    public enum DerivedFinancial
    {
        [Description("Debt Service Coverage Ratio")]
        DebtCoverageRatio = 1,
        [Description("Working Capital")]
        WorkingCapital = 2,
        [Description("Days Cash on Hand")]
        DaysCashOnHand = 3,
        [Description("Days in Accounts Receivable")]
        DaysInAccountReceivable = 4,
        [Description("Average Payment Period")]
        AvgPaymentPeriod = 5,
    }

    public abstract class FundamentalFinancial
    {
        public Dictionary<DerivedFinancial, decimal?> DerivedFinDict = new Dictionary<DerivedFinancial, decimal?>();
        public Dictionary<DerivedFinancial, decimal?> DerivedScoreDict = new Dictionary<DerivedFinancial, decimal?>();
        public decimal? ScoreSum = 0;
        
        public void FillDerived(IUploadedLoanVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
