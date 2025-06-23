using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqNotenverwaltung.StatisticsSchoolyear
{
    internal class SchooldayCount
    {
        /// <summary>
        /// Total number of specific schoolday in the schoolyear.
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// Number of specific Schooolday in odd weeks of schoolyear.
        /// </summary>
        public int CountWeekOne { get; set; }
        /// <summary>
        /// Number of specific Schoolday in even weeks of schoolyear.
        /// </summary>
        public int CountWeekTwo { get; set; }
        /// <summary>
        /// List of number of specific Schoolday in each Xmester of schoolyear (Semester, Trimester, Whatevermester).
        /// </summary>
        public List<int> CountXmester { get; set; } = [];
    }
}
