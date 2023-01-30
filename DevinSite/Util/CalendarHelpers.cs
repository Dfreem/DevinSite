using System;
using System.Collections.Immutable;

namespace DevinSite.Util
{
    public static class CalendarHelpers
    {
        static int SinceSunday => DateTime.Now.DayOfWeek - DayOfWeek.Sunday;
        static DateTime start => DateTime.Now.AddDays(-SinceSunday);

        public static DateTime[] GetThisWeek()
        {
            DateTime[] thisWeek = new DateTime[7];
            for (int i = 0; i < 7; i++)
            {
                thisWeek[i] = start.AddDays(i);
            }
            return thisWeek;
        }

        public static DayOfWeek[] GetWeekDays()
        {
            DayOfWeek[] weekdays = new DayOfWeek[7];
            DateTime[] days = GetThisWeek();
            for (int i = 0; i < 7; i++)
            {
                weekdays[i] = days[i].DayOfWeek;
            }
            return weekdays;
        }

        public static string[] GetShortDayStrings()
        {
            string[] stringDays = new string[7];
            DateTime[] dates = GetThisWeek();
            for (int i = 0; i < 7; i++)
            {
                stringDays[i] = dates[i].DayOfWeek.ToString().Substring(0, 3);
            }
            return stringDays;
        }
    }
}

