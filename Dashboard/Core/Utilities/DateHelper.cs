using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DashboardSite.Core.Utilities
{
    public static class DateHelper
    {
        public static int MonthsToDays(int months)
        {
            return 0;
        }

        public static int DaysFromBeginningOfYearTill(DateTime datetime)
        {
            DateTime beginningOfYear = new DateTime(datetime.Year, 1, 1);
            return (datetime - beginningOfYear).Days + 1;
        }
    }
}