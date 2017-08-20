using System;
using System.Globalization;

namespace ProcessPlayer.Data.Functions
{
    public static class DateTimeExtensions
    {
        #region private methods

        private static readonly TypeConverter dateConverter = new TypeConverter();

        #endregion

        #region private methods

        private static DateTime parseDate(string s)
        {
            return dateConverter.ParseDateTime(s);
        }

        #endregion

        #region public static methods

        public static DateTime date(int year, int month, int day)
        {
            return new DateTime(year, month, day);
        }

        public static object dateValue(string s)
        {
            return parseDate(s);
        }

        public static object dateValue(object s)
        {
            if (s == DBNull.Value || s == null)
                return null;

            if (s is DateTime)
                return (DateTime)s;

            return parseDate(Convert.ToString(s));
        }

        public static object day(object d)
        {
            if (d == DBNull.Value || d == null)
                return null;

            if (d is DateTime)
                return day((DateTime)d);

            var s = Convert.ToString(d);

            return day(parseDate(s));
        }

        public static int day(DateTime d)
        {
            return d.Day;
        }

        public static int days360(DateTime startDate, DateTime endDate, int method)
        {
            int d1 = startDate.Day
                , d2 = endDate.Day
                , m1 = startDate.Month
                , m2 = endDate.Month
                , y1 = startDate.Year
                , y2 = endDate.Year;

            if (d1 == 31)
                d1 = 30;

            if (d2 == 31 && d1 == 30)
                d2 = 30;

            return (360 * (y2 - y1) + 30 * (m2 - m1) + (d2 - d1)); // 360;
        }

        public static DateTime addMonths(DateTime startDate, int months)
        {
            return startDate.AddMonths(months);
        }

        public static object addMonths(object startDate, int months)
        {
            if (startDate == DBNull.Value || startDate == null)
                return null;

            if (startDate is DateTime)
                return addMonths((DateTime)startDate, months);

            var s = Convert.ToString(startDate);

            return addMonths(parseDate(s), months);
        }

        public static DateTime edate(DateTime startDate, int months)
        {
            return startDate.AddMonths(months).Date;
        }

        public static object eomonth(object startDate, int months)
        {
            if (startDate == DBNull.Value || startDate == null)
                return null;

            if (startDate is DateTime)
                return eomonth((DateTime)startDate, months);

            var s = Convert.ToString(startDate);

            return eomonth(parseDate(s), months);
        }

        public static DateTime eomonth(DateTime startDate, int months)
        {
            DateTime edate = startDate.AddMonths(months + 1);

            return edate.AddDays(-edate.Day).Date;
        }

        public static object hour(object d)
        {
            if (d == DBNull.Value || d == null)
                return null;

            if (d is DateTime)
                return hour((DateTime)d);

            var s = Convert.ToString(d);

            return hour(parseDate(s));
        }

        public static int hour(DateTime d)
        {
            return d.Hour;
        }

        public static object minute(object d)
        {
            if (d == DBNull.Value || d == null)
                return null;

            if (d is DateTime)
                return minute((DateTime)d);

            var s = Convert.ToString(d);

            return minute(parseDate(s));
        }

        public static int minute(DateTime d)
        {
            return d.Minute;
        }

        public static object month(object d)
        {
            if (d == DBNull.Value || d == null)
                return null;

            if (d is DateTime)
                return month((DateTime)d);

            var s = Convert.ToString(d);

            return month(parseDate(s));
        }

        public static int month(DateTime d)
        {
            return d.Month;
        }

        public static int networkdays(DateTime startDate, DateTime endDate, int holidays)
        {
            throw new NotImplementedException();
        }

        public static DateTime now()
        {

            var ticks = DateTime.Now.Ticks;
            var dticks = ticks / 100000.0;
            var result = (long)System.Math.Round(dticks) * 100000;

            return new DateTime(result);
        }

        public static object second(object d)
        {
            if (d == DBNull.Value || d == null)
                return null;

            if (d is DateTime)
                return second((DateTime)d);

            var s = Convert.ToString(d);

            return second(parseDate(s));
        }

        public static int second(DateTime d)
        {
            return d.Millisecond >= 500 ? d.Second + 1 : d.Second;
        }

        public static long time(int hours, int minutes, int seconds)
        {
            return new TimeSpan(hours, minutes, seconds).Ticks;
        }

        public static long timeValue(string timeText)
        {
            TimeSpan timeSpan;

            if (TimeSpan.TryParse(timeText, out timeSpan))
                return timeSpan.Ticks;
            return 0;
        }

        public static DateTime today()
        {
            return DateTime.Today;
        }

        public static int weekday(DateTime d, int returnType)
        {
            int weekday;

            switch (returnType)
            {
                case 2:
                    return (int)d.DayOfWeek; // вс - 0
                case 3:
                    return (weekday = (int)d.DayOfWeek - 1) < 0 ? 7 : weekday; // пн - 0
                default:
                    return (weekday = (int)d.DayOfWeek + 1) > 7 ? 0 : weekday; // сб - 0
            }
        }

        public static int weeknum(DateTime d, int returnType)
        {
            switch (returnType)
            {
                case 2:
                    return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                default:
                    return CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
            }
        }

        public static int workday(DateTime d, int days, int holidays)
        {
            throw new NotImplementedException();
        }

        public static object year(object d)
        {
            if (d == DBNull.Value || d == null)
                return null;

            if (d is DateTime)
                return year((DateTime)d);

            var s = Convert.ToString(d);

            return year(parseDate(s));
        }

        public static int year(DateTime d)
        {
            return d.Year;
        }

        public static int yearfrac(DateTime startDate, DateTime endDate, int basis)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
