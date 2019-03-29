using System;
using System.Globalization;

namespace ExpenseManager.EF.Helpers
{
    public class WeekOfMonthDeterminant
    {
        //public static int GetWeekFromDateTime(DateTime date)
        //{
        //    var weekVal = System.Threading.Thread.CurrentThread.CurrentCulture.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
        //     return weekVal;
        //}


        public int GetWeekOfMonth(DateTime time)
        {
            var datestr = time.ToString(CultureInfo.InvariantCulture).Split('/');
            int intVal;
            int outInt;

            var result = int.TryParse(datestr[1], out outInt);

            if (result)
            {
                intVal = outInt;
            }
            else
            {
                return 0;
            }

            if (intVal < 7)
            {
                return 1;
            }

            if (intVal > 7 && intVal < 14)
            {
                return 2;
            }

            if (intVal == 14)
            {
                return 2;
            }

            if (intVal > 14 && intVal < 21)
            {
                return 3;
            }

            if (intVal == 21)
            {
                return 3;
            }

            if (intVal > 21 && intVal < 28)
            {
                return 4;
            }

            if (intVal == 28)
            {
                return 4;
            }

            if (intVal > 28)
            {
                return 4;
            }

            return 0;
            //var first = new DateTime(time.Year, time.Month, 1);
            //var next = GetWeekFromDateTime(time);
            //var netc = GetWeekFromDateTime(first) - 1 ;
            //var val = next - netc;
            //return val;
        }

        public int GetWeekInMonth(DateTime date)
        {
            var beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate(date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

    }
}