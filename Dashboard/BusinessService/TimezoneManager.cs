using Core;
using DashboardSite.Core;
using DashboardSite.Core.Interface;
using DashboardSite.Core.Utilities;
using DashboardSite.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DashboardSite.BusinessService
{
    public static class TimezoneManager
    {
        //public static DateTime GetPreferredTimeFromUtc(DateTime utcDt)
        //{
        //    var timeZoneId = UserPrincipal.Current.Timezone;
        //    if (!string.IsNullOrEmpty(timeZoneId))
        //    {
        //        TimeZoneInfo destTimezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        //        return TimeZoneInfo.ConvertTimeFromUtc(utcDt, destTimezone);
        //    }
        //    else
        //    {
        //        return utcDt;
        //    }
        //}

        //public static DateTime GetUtcTimeFromPreferred(DateTime myDt)
        //{
        //    var timeZoneId = UserPrincipal.Current.Timezone;
        //    if (!string.IsNullOrEmpty(timeZoneId))
        //    {
        //        TimeZoneInfo myTimezone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        //        return TimeZoneInfo.ConvertTimeToUtc(myDt, myTimezone);
        //    }
        //    else
        //    {
        //        return myDt;
        //    }
        //}
    }
}
