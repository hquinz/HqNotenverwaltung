using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqNotenverwaltung.Tools
{
    //Todo Make static
    public static class ToolsDateTime
    {
        /// <summary>
        /// Calculates the date of the specified weekday relative to the given date.
        /// </summary>
        /// <param name="date">The reference date from which the calculation is performed.</param>
        /// <param name="weekday">The target day of the week to calculate.</param>
        /// <returns>A <see cref="DateTime"/> representing the date of the specified <paramref name="weekday"/> relative to the
        /// provided <paramref name="date"/>. If the target weekday is earlier in the week than the reference date, the
        /// result will be in the past; otherwise, it will be in the future or the same day.</returns>
        public static DateTime GetDateOfWeekday(DateTime date, DayOfWeek weekday)
        {
            int diff = (7 + (weekday - date.DayOfWeek)) % 7;
            // Wenn das Ziel vor dem aktuellen Tag liegt, in die Vergangenheit gehen
            if (diff > 0 && weekday < date.DayOfWeek)
                diff -= 7;
            return date.AddDays(diff);
        }
    }
}
