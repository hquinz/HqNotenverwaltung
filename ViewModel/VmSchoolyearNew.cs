using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Tools;
using System.ComponentModel;

namespace HqNotenverwaltung.ViewModel
{
    internal class VmSchoolyearNew : INotifyPropertyChanged
    {
        // HACK : Check Dates if they are correct (e.g. End Semester after Start autum Break...)

        public Array SemesteredOptions => Enum.GetValues(typeof(EnumSemestered));

        private double? yearStart;
        public double? YearStart
        {
            get => yearStart;
            set
            {
                yearStart = value;
                if (yearStart is null) return;
                initializeSchoolyear((int)yearStart);
                OnPropertyChanged(nameof(DateYearStart));
            }
        }
        public EnumSemestered SemesteredSelected { get; set; }
        private DateTime dateYearStart;
        public DateTime DateYearStart
        {
            get => dateYearStart;
            set
            {
                dateYearStart = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Monday);
                OnPropertyChanged(nameof(DateYearStart));
            }
        }
        private DateTime dateAutumnBreakStart;
        public DateTime DateAutumnBreakStart
        {
            get => dateAutumnBreakStart;
            set
            {
                dateAutumnBreakStart = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Monday);
                DateAutumnBreakEnd = dateAutumnBreakStart.AddDays(5);
                OnPropertyChanged(nameof(DateAutumnBreakStart));
            }
        }
        // HACK: make Private set for DateAutumnBreakEnd
        private DateTime dateAutumnBreakEnd;
        public DateTime DateAutumnBreakEnd
        {
            get => dateAutumnBreakEnd;
            set
            {
                dateAutumnBreakEnd = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                OnPropertyChanged(nameof(DateAutumnBreakEnd));
            }
        }
        private DateTime dateTechnicalSchoolStart;
        public DateTime DateTechnicalSchoolStart
        {
            get => dateTechnicalSchoolStart;
            set
            {
                dateTechnicalSchoolStart = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Monday);
                OnPropertyChanged(nameof(DateTechnicalSchoolStart));
            }
        }
        private DateTime dateSemesterEnw;
        public DateTime DateSemesterEnd
        {
            get => dateSemesterEnw;
            set
            {
                dateSemesterEnw = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                OnPropertyChanged(nameof(DateSemesterEnd));
            }
        }
        private DateTime dateVocationalSchoolEnd;
        public DateTime DateVocationalSchoolEnd
        {
            get => dateVocationalSchoolEnd;
            set
            {
                dateVocationalSchoolEnd = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                OnPropertyChanged(nameof(DateVocationalSchoolEnd));
            }
        }
        private DateTime dateYearEnd;
        public DateTime DateYearEnd
        {
            get => dateYearEnd;
            set
            {
                dateYearEnd = ToolsDateTime.GetDateOfWeekday(value, DayOfWeek.Friday);
                OnPropertyChanged(nameof(DateYearEnd));
            }
        }

        public VmSchoolyearNew(int schoolyear)
        {
            this.YearStart = schoolyear;
        }

        private void initializeSchoolyear(int year)
        {
            DateYearStart = new DateTime(2000 + year, 9, 7);
            DateAutumnBreakStart = new DateTime(2000 + year, 10, 26);
            //DateAutumnBreakEnd = DateAutumnBreakStart.AddDays(5); //Trasnsferd to setter DateAutumnBreakStart
            DateTechnicalSchoolStart = new DateTime(2000 + year, 11, 10);
            DateSemesterEnd = new DateTime(2001 + year, 2, 14);
            DateVocationalSchoolEnd = new DateTime(2001 + year, 5, 2);
            DateYearEnd = new DateTime(2001 + year, 7, 3);
        }

        internal void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
