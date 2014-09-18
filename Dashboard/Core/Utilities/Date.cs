using System;

namespace DashboardSite.Core.Utilities
{
    public class Date
    {
        #region ctors/initializers
        public Date()
        {
            dt = System.DateTime.Today;
        }

        public Date(DateTime d)
        {
            dt = d.Date;
        }
        #endregion

        #region Properties/accessors
        public DateTime DateTime
        {
            get { return dt; }
            set { dt = value.Date; }
        }

        public int Quarter
        {
            get { return GetQuarter(dt.Month); }   
        }
        #endregion

        #region modifiers
        public DateTime SetToEndOfLastMonth()
        {
            return dt = GetEndOfLastMonth(dt);
        }

        public DateTime SetToEndOfNextMonth()
        {
            return dt = GetEndOfNextMonth(dt);
        }

        public DateTime SetToEndOfLastQuarter()
        {
            return dt = GetEndOfLastQuarter(dt);
        }

        public DateTime SetToEndOfNextQuarter()
        {
            return dt = GetEndOfNextQuarter(dt);
        }

        public DateTime SetToNextMonth()
        {
            return dt = dt.AddMonths(1);
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return dt.ToString("d");
        }
        #endregion

        #region Static publics methods
        public static DateTime GetEndOfQuarter(DateTime d)
        {
            return GetEndOfQuarter(d.Year, GetQuarter(d.Month));
        }

        public static DateTime GetEndOfLastQuarter(DateTime d)
        {
            int quarter = GetQuarter(d.Month);
            return quarter == 1 ? GetEndOfQuarter(d.Year - 1, 4) : GetEndOfQuarter(d.Year, quarter -1);
        }

        public static DateTime GetEndOfNextQuarter(DateTime d)
        {
            int quarter = GetQuarter(d.Month);
            return quarter == 4 ? GetEndOfQuarter(d.Year + 1, 1) : GetEndOfQuarter(d.Year, quarter + 1);
        }

        public static DateTime GetEndOfMonth(DateTime d)
        {
            return GetEndOfMonth(d.Year, d.Month);
        }

        public static DateTime GetEndOfLastMonth(DateTime d)
        {
            return d.Month == 1 ? GetEndOfMonth(d.Year - 1, 12) : GetEndOfMonth(d.Year, d.Month - 1);
        }

        public static DateTime GetEndOfNextMonth(DateTime d)
        {
            return d.Month == 12 ? GetEndOfMonth(d.Year + 1, 1) : GetEndOfMonth(d.Year, d.Month + 1);
        }

        #endregion Static publics

        #region Privates
        private DateTime dt;

        private static int GetQuarter(int Month)
        {
            if (Month <= 3)                 // 1st Quarter: January to March 31
                return 1;
            if (Month >= 4 && Month <= 6)   // 2nd Quarter: April to June 30
                return 2;
            if (Month >= 7 && Month <= 9)   // 3rd Quarter: July 1 to September 30
                return 3;
            return 4;                       // 4th Quarter: October 1 to December 31
        }

        private static DateTime GetEndOfMonth(int Year, int Month)
        {
            return new DateTime(Year, Month, DateTime.DaysInMonth(Year, Month));
        }

        private static DateTime GetEndOfQuarter(int Year, int Quarter)
        {
            return GetEndOfMonth(Year, Quarter * 3);
        }
        #endregion Privates
    }
}
