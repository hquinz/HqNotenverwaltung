using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqNotenverwaltung.Tools
{
    class ToolsDateTime
    {
        public DateTime GetDateOfWeekday(DateTime date, DayOfWeek weekday)
        {
            int diff = (7 + (weekday - date.DayOfWeek)) % 7;
            // Wenn das Ziel vor dem aktuellen Tag liegt, in die Vergangenheit gehen
            if (diff > 0 && weekday < date.DayOfWeek)
                diff -= 7;
            return date.AddDays(diff);
        }
    }
}
