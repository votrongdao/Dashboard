/****************************************************************************
 * $Header: /iPreo Bigdough System/Utilities/iPreo.Bigdough.Utilities/DateRange.cs 1     5/12/09 11:50a Salnikog $
 *
 * Created By George Salnikov
 * Created on 10 May, 2009
 * All rights reserved, Ipreo
 *
 * DESCRIPTION. Range of dates
 * $Log: /iPreo Bigdough System/Utilities/iPreo.Bigdough.Utilities/DateRange.cs $
 * 
 * 1     5/12/09 11:50a Salnikog
 * 
 ****************************************************************************/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;


namespace iPreo.Bigdough.Utilities
{
    public class DateRange
    {
        public DateTime Beg;
        public DateTime End;

        public bool Valid
        {
            get { return Beg != DateTime.MinValue; }
        }

        public bool InRange(DateTime val)
        {
            val = val.Date;
            return val >= Beg && val <= End;
        }

        #region ctors
        public DateRange()
        {
            Beg = DateTime.MinValue;
            End = DateTime.MaxValue;
        }

        public DateRange(DateTime dtBeg, DateTime dtEnd)
        {
            Beg = dtBeg;
            End = dtEnd;
        }

        public DateRange(DateTime? dtBeg, DateTime? dtEnd)
        {
            Beg = dtBeg ?? DateTime.MinValue;
            End = dtEnd ?? (dtBeg ?? DateTime.MaxValue);
        }

        public DateRange(string val)
        {
            Beg = DateTime.MinValue;
            End = DateTime.MaxValue;
            Regex rx = new Regex(@"[\d\" + CultureInfo.CurrentCulture.DateTimeFormat.DateSeparator + "]*");
            MatchCollection mc = rx.Matches(val);
            if (mc.Count <= 0)
                return;

            if (!DateTime.TryParse(mc[0].Value, out Beg))
            {
                Beg = DateTime.MinValue;
                return;
            }

            for (int i = 1 ; i < mc.Count ; ++i)
                if (DateTime.TryParse(mc[i].Value, out End))
                    return;

            End = Beg = Beg.Date;
        }
        #endregion ctors
    }
}
