#region Using statements

using System;
using System.Globalization;

#endregion Using statements

namespace WeekNumber
{
    internal class Week
    {
        #region Private variable that holds active week

        private int _week;

        #endregion Private variable that holds active week

        #region Constructor that initiates active week

        /// <summary>
        /// Initiates the week to current
        /// </summary>
        public Week()
        {
            _week = Current();
        }

        #endregion Constructor that initiates active week

        #region Internal static week strings

        internal const string CalendarWeekRuleString = nameof(CalendarWeekRule);
        internal const string FirstDay = nameof(CalendarWeekRule.FirstDay);
        internal const string FirstFourDayWeek = nameof(CalendarWeekRule.FirstFourDayWeek);
        internal const string FirstFullWeek = nameof(CalendarWeekRule.FirstFullWeek);
        internal const string DayOfWeekString = nameof(DayOfWeek);
        internal const string Monday = nameof(DayOfWeek.Monday);
        internal const string Tuesday = nameof(DayOfWeek.Tuesday);
        internal const string Wednesday = nameof(DayOfWeek.Wednesday);
        internal const string Thursday = nameof(DayOfWeek.Thursday);
        internal const string Friday = nameof(DayOfWeek.Friday);
        internal const string Saturday = nameof(DayOfWeek.Saturday);
        internal const string Sunday = nameof(DayOfWeek.Sunday);

        #endregion Internal static week strings

        #region Public function to check if week has changed

        /// <summary>
        /// Returns if week was changed since last check
        /// </summary>
        /// <returns>true|false</returns>
        public bool WasChanged()
        {
            bool changed = _week != Current();
            if (changed)
            {
                _week = Current();
            }
            return changed;
        }

        #endregion Public function to check if week has changed

        #region Public function that returns current week based on calendar rule

        /// <summary>
        /// Get current week based on calendar rules in application settings
        /// </summary>
        /// <returns>Current week as int based on calendar rules in application settings</returns>
        public static int Current()
        {
            DayOfWeek dayOfWeek;
            CalendarWeekRule calendarWeekRule;
            dayOfWeek = Enum.TryParse(Settings.GetSetting(DayOfWeekString), true, out dayOfWeek) ?
                dayOfWeek : DayOfWeek.Monday;
            calendarWeekRule = Enum.TryParse(Settings.GetSetting(CalendarWeekRuleString), true,
                out calendarWeekRule) ? calendarWeekRule : CalendarWeekRule.FirstFourDayWeek;
            int week = CultureInfo.CurrentCulture.Calendar.
                GetWeekOfYear(DateTime.Now, calendarWeekRule, dayOfWeek);
            if (week == 53 && (!YearHas53Weeks(DateTime.Now.Year)))
            {
                week = 1;
            }
            return week;
        }

        #endregion Public function that returns current week based on calendar rule

        #region Private helper functions to determine if it is week 1 or 53

        private static bool YearHas53Weeks(int year)
        {
            return Weeks(year) == 53;
        }

        private static int Weeks(int year)
        {
            int w = 52;
            if (P(year) == 4 || P(year - 1) == 3)
            {
                w++;
            }
            return w; // returns the number of weeks in that year
        }

        private static int P(int year)
        {
            return (int)(year + Math.Floor(year / 4f) - Math.Floor(year / 100f) + Math.Floor(year / 400f)) % 7;
        }

        #endregion Private helper functions to determine if it is week 1 or 53
    }
}